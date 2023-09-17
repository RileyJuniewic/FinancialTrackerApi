using FinancialTracker.Common.Contracts.Account;
using FinancialTracker.Models;

namespace FinancialTracker.Services;

public interface IAccountService
{
    Task<Account> AddAccountAsync(AddAccountRequest request);
    Task<Account> GetAccountAsync(GetAccountRequest request);
    Task<IEnumerable<Account>> GetAllAccountsAsync(GetAllAccountsRequest request);
    Task<Account> EditAccountAsync(EditAccountRequest request);
    Task<Account> DeleteAccountAsync(DeleteAccountRequest request);
    Task<bool> ValidateAccountAsync(ValidateAccountRequest request);
}

public class AccountService : IAccountService
{
    public async Task<Account> AddAccountAsync(AddAccountRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Account> GetAccountAsync(GetAccountRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync(GetAllAccountsRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Account> EditAccountAsync(EditAccountRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Account> DeleteAccountAsync(DeleteAccountRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ValidateAccountAsync(ValidateAccountRequest request)
    {
        throw new NotImplementedException();
    }
}