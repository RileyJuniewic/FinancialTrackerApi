using FinancialTracker.Common.Contracts.User;
using FinancialTracker.Models;

public interface IUserService
{
    Task<User> CreateUserAsync(CreateUserRequest request);
    Task DeleteUserAsync(DeleteUserRequest request);
    Task<User> EditUserAsync(EditUserRequest request);
    Task<User> LoginUserAsync(LoginUserRequest request);
    Task LogoutUserAsync(LogoutUserRequest request);
}

public class UserService : IUserService
{
    public async Task<User> CreateUserAsync(CreateUserRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteUserAsync(DeleteUserRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<User> EditUserAsync(EditUserRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<User> LoginUserAsync(LoginUserRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task LogoutUserAsync(LogoutUserRequest request)
    {
        throw new NotImplementedException();
    }
}