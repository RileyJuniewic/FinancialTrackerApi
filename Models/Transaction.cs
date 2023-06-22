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

        public static Transaction CreateNewTransaction(string userId, string savingsAccountId, TransactionType type, string description, string amount)
        {
            var transactionType = SetTransactionType(type);
            return new Transaction(Guid.NewGuid().ToString(), userId, savingsAccountId, transactionType, description, amount, DateTime.UtcNow);
        }

        public static Transaction CreateTransaction(string id, string userId, string savingsAccountId, TransactionType type, string description, string amount, DateTime date)
        {
            var transactionType = SetTransactionType(type);
            return new Transaction(id, userId, savingsAccountId, transactionType, description, amount, date);
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
