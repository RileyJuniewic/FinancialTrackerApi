using Microsoft.IdentityModel.Tokens;

namespace FinancialTracker.Models
{
    public class Account
    {
        public string Id { get; private set; } = string.Empty;
        public string UserId { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public decimal Balance { get; private set; }

        public Account()
        {
        }

        private Account(string id, string userId, string name, decimal balance)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Balance = balance;
        }

        public static Account CreateNew(string userId, string name, decimal balance = 0) =>
            new Account(Guid.NewGuid().ToString(), userId, name, balance);

        public static Account Create(string id, string userId, string name, decimal balance) =>
            new Account(id, userId, name, balance);

        public decimal Deposit(decimal amount) => Balance += amount;

        public decimal Withdraw(decimal amount) => (Balance -= amount) > 0 ? Balance : throw new Exception("Account balance cannot be negative");

        public void ChangeName(string name)
        {
            if (name.IsNullOrEmpty())
                throw new Exception("New account name must not be empty.");
            Name = name;
        }
    }
}
