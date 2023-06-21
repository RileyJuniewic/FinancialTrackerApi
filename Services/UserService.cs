using Dapper;
using ErrorOr;
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
        Task<ErrorOr<User>> Login(LoginRequest request);
        Task<ErrorOr<User>> VerifyLogin(LoginRequest request);
        Task<ErrorOr<AuthenticationResponse>> Register(RegisterRequest request);
    }

    public class UserService : IUserService
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IAuthCookieService _cookieService;

        public UserService(ISqlDataAccess sqlDataAccess, IJwtTokenService jwtTokenService, IAuthCookieService cookieService)
        {
            _sqlDataAccess = sqlDataAccess;
            _jwtTokenService = jwtTokenService;
            _cookieService = cookieService;
        }

        public async Task<ErrorOr<User>> Login(LoginRequest request)
        {
            if (await GetUser(request.Email) is var user && (user is null || !CheckPassword(user, request.Password))) 
            { return Errors.UserError.InvalidCredentials; }

            await _cookieService.CreateSignInCookie(user);
            return ScrubPassword(user);
        }
        
        public async Task<ErrorOr<User>> VerifyLogin(LoginRequest request)
        {
            if (await GetUser(request.Email) is var user && (user is null || !CheckPassword(user, request.Password))) 
            { return Errors.UserError.InvalidCredentials; }
            return user;
        }

        public async Task<ErrorOr<AuthenticationResponse>> Register(RegisterRequest request)
        {
            var user = User.CreateNewUser(request.FirstName, request.LastName, request.Email, request.Password);
            if ((await GetUser(user.Email)) is var takenUser && takenUser is not null) 
            { return Errors.UserError.DuplicateEmail; }

            user.SetPassword(HashPassword(user.Password));
            await _sqlDataAccess.GetConnection().ExecuteAsync("AddUser", user, commandType: CommandType.StoredProcedure);
            return new AuthenticationResponse(ScrubPassword(user), _jwtTokenService.GenerateToken(user));
        }

        private User ScrubPassword(User user)
        {
            user.ScrubPassword();
            return user;
        }

        private string HashPassword(string password) =>
            BC.EnhancedHashPassword(password, 12, BCrypt.Net.HashType.SHA384);

        private bool CheckPassword(User user, string password) =>
            BC.Verify(password, user.Password, true, BCrypt.Net.HashType.SHA384);

        private async Task<User?> GetUser(string email) =>
            (await _sqlDataAccess.GetConnection()
                .QueryAsync<User>("GetUserByEmail", new { @email = email }, commandType: CommandType.StoredProcedure)).FirstOrDefault();

    }
}
