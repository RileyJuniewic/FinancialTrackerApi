using Microsoft.IdentityModel.Tokens;

namespace FinancialTracker.Models
{
    public class SavingsAccount
    {
        public string Id { get; private set; } = string.Empty;
        public string UserId { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Balance { get; private set; } = string.Empty;

        public SavingsAccount()
        {
        }

        private SavingsAccount(string id, string userId, string name, string balance)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Balance = balance;
        }

        public static SavingsAccount CreateNew(string userId, string name, string balance = "0.00") =>
            new SavingsAccount(Guid.NewGuid().ToString(), userId, name, balance);

        public static SavingsAccount Create(string id, string userId, string name, string balance) =>
            new SavingsAccount(id, userId, name, balance);

        public string Deposit(string amount)
        {
            amount = StringToCurrencyString(amount);
            var numericalBalance = StringToFloat(Balance);
            var numericalAmount = StringToFloat(amount);

            numericalBalance += numericalAmount;
            return Balance = FloatToCurrencyString(numericalBalance);
        }

        public string Withdraw(string amount)
        {
            amount = StringToCurrencyString(amount);
            var numericalBalance = StringToFloat(Balance);
            var numericalAmount = StringToFloat(amount);
            var newNumericalBalance = numericalBalance - numericalAmount;
            if (newNumericalBalance < 0)
                throw new Exception("Amount overdraw");

            return Balance = FloatToCurrencyString(newNumericalBalance);
        }

        public void ChangeName(string name)
        {
            if (name.IsNullOrEmpty())
                throw new Exception("New account name must not be empty.");
            Name = name;
        }

        public static string StringToCurrencyString(string value)
        {
            var floatValue = StringToFloat(value);
            return FloatToCurrencyString(floatValue);
        }
        
        public static string FloatToCurrencyString(float value) =>
            $"{Convert.ToDecimal(value):C}".Remove(0, 1);

        public static float StringToFloat(string value)
        {
            try 
            { 
                var floatValue = float.Parse(value);
                if (floatValue < 0) 
                    throw new Exception("Amount must be greater than 0.00");
                return floatValue;
            }
            catch (Exception) 
            { throw new Exception("Failed to parse string to float"); }
        }
    }
}
