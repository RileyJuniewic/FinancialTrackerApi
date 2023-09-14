public interface ITransactionService
{
    void AddTransaction();
    void GetTransaction();
    void GetTransactions();
    void EditTransaction();
    void DeleteTransaction();
}

public class TrasactionService : ITransactionService
{
    
}