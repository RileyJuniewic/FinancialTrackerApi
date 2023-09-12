using FinancialTracker.Common.Contracts.SavingsAccount.Request;
using FinancialTracker.Common.Contracts.Transaction;
using FinancialTracker.Models;
using FinancialTracker.Persistance;

namespace FinancialTracker.Services;

public interface ITransactionService
{
    Task<Transaction> AddTransaction(AddTransactionRequest request);
    Task<Transaction> DeleteTransaction(DeleteTransactionRequest request);
    Task<Transaction> EditTransaction(EditTransactionRequest request);
    Task<Transaction> GetTransaction(GetTransactionRequest request);
    Task<IEnumerable<Transaction>> GetTransactions(GetTransactionsRequest request);
}

public class TransactionService : ITransactionService
{
    private readonly IAccountService _accountService;
    private readonly ISqlDataAccess _dataAccess;
    
    public TransactionService(IAccountService accountService, ISqlDataAccess dataAccess)
    {
        _accountService = accountService;
        _dataAccess = dataAccess;
    }
    
    public async Task<Transaction> AddTransaction(AddTransactionRequest request)
    {
        //var account = await GetAccount(request.AccountId);
        var account = await GetAccount(new Guid());
        var transaction =
            Transaction.CreateNewTransaction(request.Type, request.Description, request.Amount, request.Date, account);
        
        //! Do some code to save transaction to db
        
        return transaction;
    }

    public async Task<Transaction> DeleteTransaction(DeleteTransactionRequest request)
    {
        var account = await _accountService.GetSavingsAccount(request.SavingsAccountId);
        
        //! unsure of implementation quite yet
        
        throw new NotImplementedException();
    }

    public async Task<Transaction> EditTransaction(EditTransactionRequest request)
    {
        //var account = await GetAccount(request.AccountId);
        var account = await GetAccount(new Guid());
        var transaction = await GetTransaction(new Guid()); //! Need to edit models for guid
        //! May be able to just send extra data from client
            //! Create ExistingTransaction with new data
            //! Override existing transaction in db
            //! Return transaction
            
        //! Edit Transaction here
        //! Save edited transaction
        //! Return transaction
        throw new NotImplementedException();
    }

    public async Task<Transaction> GetTransaction(GetTransactionRequest request)
    {
        var account = await GetAccount(new Guid());
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Transaction>> GetTransactions(GetTransactionsRequest request)
    {
        var account = await GetAccount(new Guid());
        throw new NotImplementedException();
    }

    private async Task<Transaction> GetTransaction(Guid id) =>
        //! Get Transaction from db
        //! Return
        throw new NotImplementedException();

    private async Task<Account> GetAccount(Guid id) =>
        //! Need to edit to string in future
        await _accountService.GetSavingsAccount(id.ToString());
}