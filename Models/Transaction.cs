using FinancialTracker.Common.Errors;
using FinancialTracker.Models.Enums;

namespace FinancialTracker.Models
{
    public class Transaction
    {
        public string Id { get; private set; } = string.Empty;
        public string SavingsAccountId { get; private set; } = string.Empty;
        public string TransactionType { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Amount { get; private set; } = string.Empty;
        public string NewBalance { get; private set; } = string.Empty;
        public DateTime Date { get; private set; }

        public Transaction() 
        {
        }

        private Transaction(string id, string savingsAccountId, string type, string description, string amount, string newBalance, DateTime date)
        {
            Id = id;
            SavingsAccountId = savingsAccountId;
            TransactionType = type;
            Description = description;
            Amount = amount;
            NewBalance = newBalance;
            Date = date;
        }

        public static Transaction CreateNewTransaction(string userId, string savingsAccountId, TransactionType type, string description, string amount, SavingsAccount account)
        {
            var transactionType = SetTransactionType(type);
            
            var newBalance = transactionType switch
            {
                "Withdrawal" => account.Withdraw(amount),
                "Deposit" => account.Deposit(amount),
                _ => throw Errors.TransactionError.InvalidTransactionType
            };
            
            return new Transaction(Guid.NewGuid().ToString(), savingsAccountId, transactionType, description, amount, newBalance, DateTime.UtcNow);
        }

        public static Transaction CreateExistingTransaction(string id, string userId, string savingsAccountId, TransactionType type, string description, string amount, string newBalance, DateTime date)
        {
            var transactionType = SetTransactionType(type);
            return new Transaction(id, savingsAccountId, transactionType, description, amount, newBalance, date);
        }

        public static string CreateId() =>
            Guid.NewGuid().ToString();

        private static string SetTransactionType(TransactionType transactionType)
        {
            return transactionType switch
            {
                Enums.TransactionType.Deposit => "Deposit",
                Enums.TransactionType.Withdrawal => "Withdrawal",
                Enums.TransactionType.Transfer => "Transfer",
                _ => throw Errors.TransactionError.InvalidTransactionType
            };
        }
    }
}
