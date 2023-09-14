public interface IAccountService
{
    void AddAccount();
    void GetAccount();
    void GetAccounts();
    void EditAccount();
    void DeleteAccount();
}

public class AccountService : IAccountService
{
    
}