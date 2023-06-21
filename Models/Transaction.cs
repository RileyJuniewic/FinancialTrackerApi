using ErrorOr;
using FinancialTracker.Common.Errors;
using FinancialTracker.Models.Enums;

namespace FinancialTracker.Models
{
    public class Transaction
    {
        public string Id { get; private set; } = string.Empty;
        public string UserId { get; private set; } = string.Empty;
        public string SavingsAccountId { get; private set; } = string.Empty;
        public string TransactionType { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Amount { get; private set; } = string.Empty;
        public DateTime Date { get; private set; }

        public Transaction() 
        {
        }

        private Transaction(string id, string userId, string savingsAccountId, string type, string description, string amount, DateTime date)
        {
            Id = id;
            UserId = userId;
            SavingsAccountId = savingsAccountId;
            TransactionType = type;
            Description = description;
            Amount = amount;
            Date = date;
        }

        public static ErrorOr<Transaction> CreateNewTransaction(string userId, string savingsAccountId, TransactionType type, string description, string amount)
        {
            if (SetTransactionType(type) is var transType && transType.IsError) { return transType.Errors; }
            return new Transaction(Guid.NewGuid().ToString(), userId, savingsAccountId, transType.Value, description, amount, DateTime.UtcNow);
        }

        public static ErrorOr<Transaction> CreateTransaction(string id, string userId, string savingsAccountId, TransactionType type, string description, string amount, DateTime date)
        {
            if (SetTransactionType(type) is var transType && transType.IsError) { return transType.Errors; }
            return new Transaction(id, userId, savingsAccountId, transType.Value, description, amount, date);
        }

        public static string CreateId() =>
            Guid.NewGuid().ToString();

        private static ErrorOr<string> SetTransactionType(TransactionType transactionType)
        {
            return transactionType switch
            {
                Enums.TransactionType.Deposit => "Deposit",
                Enums.TransactionType.Withdrawal => "Withdrawal",
                Enums.TransactionType.Transfer => "Transfer",
                _ => Errors.TransactionError.InvalidTransactionType
            };
        }
    }
}
