using Dapper;
using FinancialTracker.Common.Contracts.Authentication;
using FinancialTracker.Common.Errors;
using FinancialTracker.Models;
using FinancialTracker.Persistance;
using FinancialTracker.Services.Common;
using System.Data;
using BC = BCrypt.Net.BCrypt;

namespace FinancialTracker.Services
{
    public interface IUserService
    {
        Task<User> Login(LoginRequest request);
        Task<User> Login();
        Task<User> VerifyLogin(LoginRequest request);
        Task<AuthenticationResponse> Register(RegisterRequest request);
    }

    public class UserService : IUserService
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IAuthCookieService _cookieService;
        private readonly IHttpContextHelperService _httpContext;

        public UserService(ISqlDataAccess sqlDataAccess, IJwtTokenService jwtTokenService, IAuthCookieService cookieService, IHttpContextHelperService httpContext)
        {
            _sqlDataAccess = sqlDataAccess;
            _jwtTokenService = jwtTokenService;
            _cookieService = cookieService;
            _httpContext = httpContext;
        }

        public async Task<User> Login(LoginRequest request)
        {
            var user = await GetUserByEmail(request.Email);
            if (user is null || !CheckPassword(user, request.Password)) 
                throw Errors.UserError.InvalidCredentials;

            await _cookieService.CreateSignInCookie(user);
            return ScrubPassword(user);
        }

        public async Task<User> Login()
        {
            var userClaim = _httpContext.GetClaimUserId();
            var user = await GetUserById(userClaim.Value);
            if (user is null) throw Errors.UserError.UserNotFound;
            return ScrubPassword(user);
        }

        public async Task<User> VerifyLogin(LoginRequest request)
        {
            var user = await GetUserByEmail(request.Email);
            if (user is null || !CheckPassword(user, request.Password)) 
                throw Errors.UserError.InvalidCredentials; 
            return ScrubPassword(user);
        }

        public async Task<AuthenticationResponse> Register(RegisterRequest request)
        {
            var newUser = User.CreateNewUser(request.FirstName, request.LastName, request.Email, request.Password);
            var existingUser = await GetUserByEmail(newUser.Email);
            if (existingUser is not null) 
                throw Errors.UserError.DuplicateEmail;

            newUser.SetPassword(HashPassword(newUser.Password));
            await _sqlDataAccess.GetConnection().ExecuteAsync("AddUser", newUser, commandType: CommandType.StoredProcedure);
            return new AuthenticationResponse(ScrubPassword(newUser), _jwtTokenService.GenerateToken(newUser));
        }

        private static User ScrubPassword(User user)
        {
            user.ScrubPassword();
            return user;
        }

        private static string HashPassword(string password) =>
            BC.EnhancedHashPassword(password, 12, BCrypt.Net.HashType.SHA384);

        private static bool CheckPassword(User user, string password) =>
            BC.Verify(password, user.Password, true);

        private async Task<User?> GetUserByEmail(string email) =>
            (await _sqlDataAccess.GetConnection()
                .QueryAsync<User>("GetUserByEmail", new { @email }, commandType: CommandType.StoredProcedure)).FirstOrDefault();
        
        private async Task<User?> GetUserById(string id) =>
            (await _sqlDataAccess.GetConnection()
                .QueryAsync<User>("GetUserById", new { @id }, commandType: CommandType.StoredProcedure)).FirstOrDefault();

    }
}
