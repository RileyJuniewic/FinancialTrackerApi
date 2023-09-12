using Microsoft.IdentityModel.Tokens;

namespace FinancialTracker.Models
{
    public class SavingsAccount
    {
        public string Id { get; private set; } = string.Empty;
        public string UserId { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public decimal Balance { get; private set; }

        public SavingsAccount()
        {
        }

        private SavingsAccount(string id, string userId, string name, decimal balance)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Balance = balance;
        }

        public static SavingsAccount CreateNew(string userId, string name, decimal balance = 0) =>
            new SavingsAccount(Guid.NewGuid().ToString(), userId, name, balance);

        public static SavingsAccount Create(string id, string userId, string name, decimal balance) =>
            new SavingsAccount(id, userId, name, balance);

        public decimal Deposit(decimal amount) => Balance += amount;

        public decimal Withdraw(decimal amount)
        {
            Balance -= amount;
            if (Balance < 0)
                throw new Exception("Account withdrawal overflow");
            return Balance;
        }

        public void ChangeName(string name)
        {
            if (name.IsNullOrEmpty())
                throw new Exception("New account name must not be empty.");
            Name = name;
        }
    }
}
