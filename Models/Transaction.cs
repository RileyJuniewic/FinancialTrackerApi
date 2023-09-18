using FinancialTracker.Models.Enums;

namespace FinancialTracker.Models
{
    public class Transaction
    {
        public string Id { get; private set; }
        public string SavingsAccountId { get; private set; }
        public string TransactionType { get; private set; }
        public string Description { get; private set; }
        public decimal Amount { get; private set; }
        public decimal NewBalance { get; private set; }
        public DateTime Date { get; private set; }
        
        private Transaction(string id, string savingsAccountId, string type, string description, decimal amount, decimal newBalance, DateTime date)
        {
            Id = id;
            SavingsAccountId = savingsAccountId;
            TransactionType = type;
            Description = description;
            Amount = amount;
            NewBalance = newBalance;
            Date = date;
        }
    }
}
