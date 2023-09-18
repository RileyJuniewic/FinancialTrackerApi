using Microsoft.IdentityModel.Tokens;

namespace FinancialTracker.Models
{
    public class Account
    {
        public string Id { get; private set; }
        public string UserId { get; private set; }
        public string Name { get; private set; }
        public decimal Balance { get; private set; }

        private Account(string id, string userId, string name, decimal balance)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Balance = balance;
        }

    }
}
