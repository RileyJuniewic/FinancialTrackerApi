using Dapper;
using FinancialTracker.Common.Contracts.Authentication;
using FinancialTracker.Models;
using FinancialTracker.Persistance;
using FinancialTracker.Services.Common;
using System.Data;
using System.Security.Authentication;
using BC = BCrypt.Net.BCrypt;

namespace FinancialTracker.Services
{
    public interface IUserService
    {
        Task<User> LoginAsync(LoginRequest request);
        Task<User> LoginAsync();
        Task LogoutAsync();
        Task<User> RegisterAsync(RegisterRequest request);
        Task<User> VerifyLoginAsync(LoginRequest request);
        Task VerifyCredentialsAsync(LoginRequest request);
    }

    public class UserService : IUserService
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        private readonly IAuthCookieService _cookieService;
        private readonly IHttpContextHelperService _httpContext;

        public UserService(ISqlDataAccess sqlDataAccess, IAuthCookieService cookieService, IHttpContextHelperService httpContext)
        {
            _sqlDataAccess = sqlDataAccess;
            _cookieService = cookieService;
            _httpContext = httpContext;
        }

        public async Task<User> LoginAsync(LoginRequest request)
        {
            var user = await GetUserByEmail(request.Email);
            if (user is null || !CheckPassword(user, request.Password)) 
                throw new AuthenticationException("Invalid credentials");

            await _cookieService.SignInAsync(user);
            return ScrubPassword(user);
        }

        public async Task<User> LoginAsync()
        {
            var userId = _httpContext.GetClaimUserId();
            var user = await GetUserById(userId.Value);
            if (user is null) throw new AuthenticationException("User session invalid");
            return ScrubPassword(user);
        }

        public async Task LogoutAsync() => await _cookieService.SignOutAsync();
        
        public async Task<User> RegisterAsync(RegisterRequest request)
        {
            var newUser = User.CreateNewUser(request.FirstName, request.LastName, request.Email, request.Password);
            var existingUser = await GetUserByEmail(newUser.Email);
            if (existingUser is not null) 
                throw new DuplicateNameException("Email already in use.");

            newUser.SetPassword(HashPassword(newUser.Password));
            await _sqlDataAccess.GetConnection().ExecuteAsync("AddUser", newUser, commandType: CommandType.StoredProcedure);
            return ScrubPassword(newUser);
        }

        public async Task<User> VerifyLoginAsync(LoginRequest request)
        {
            var user = await GetUserByEmail(request.Email);
            if (user is null || !CheckPassword(user, request.Password)) 
                throw new AuthenticationException("Invalid credentials"); 
            return ScrubPassword(user);
        }

        public async Task VerifyCredentialsAsync(LoginRequest request)
        {
            var senderUser = await VerifyLoginAsync(request);
            var userId = _httpContext.GetClaimUserId();

            if (senderUser.Id != userId.Value)
                throw new AuthenticationException("Invalid credentials");
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
