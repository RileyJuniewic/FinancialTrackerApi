using FinancialTracker.Common.Contracts.Transaction;
using FinancialTracker.Models;

namespace FinancialTracker.Services;

public interface ITransactionService
{
    Task<Transaction> AddTransaction(AddTransactionRequest request);
    Task<Transaction> GetTransaction(GetTransactionRequest request);
    Task<IEnumerable<Transaction>> GetAllTransactions(GetAllTransactionsRequest request);
    Task<Transaction> EditTransaction(EditTransactionRequest request);
    Task<Transaction> DeleteTransaction(DeleteTransactionRequest request);
}

public class TransactionService : ITransactionService
{
    public async Task<Transaction> AddTransaction(AddTransactionRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Transaction> GetTransaction(GetTransactionRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactions(GetAllTransactionsRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Transaction> EditTransaction(EditTransactionRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Transaction> DeleteTransaction(DeleteTransactionRequest request)
    {
        throw new NotImplementedException();
    }
}