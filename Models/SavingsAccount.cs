using ErrorOr;
using FinancialTracker.Common.Contracts.SavingsAccount;
using FinancialTracker.Common.Errors;

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

        public ErrorOr<string> Deposit(string amount)
        {
            if (StringToFloat(Balance) is var numericalBalanceResult && numericalBalanceResult.IsError)
            { return numericalBalanceResult.Errors; }

            var numericalBalance = numericalBalanceResult.Value;

            if (StringToFloat(amount) is var numericalAmount && numericalAmount.IsError) 
            { return numericalAmount.Errors; }

            numericalBalance += numericalAmount.Value;
            return Balance = FloatToCurrencyString(numericalBalance);
        }

        public ErrorOr<string> Withdraw(string amount)
        {
            if (StringToFloat(Balance) is var numericalBalanceResult && numericalBalanceResult.IsError) 
            { return numericalBalanceResult.Errors; }

            var numericalBalance = numericalBalanceResult.Value;

            if (StringToFloat(amount) is var numericalAmount && numericalAmount.IsError) 
            { return numericalAmount.Errors; }

            if (numericalBalance - numericalAmount.Value is var newNumericalBalance && newNumericalBalance < 0)
            { return Errors.SavingsAccountError.AmountOverdraw; }

            numericalBalance = newNumericalBalance;
            return Balance = FloatToCurrencyString(numericalBalance);
        }

        public ErrorOr<string> Transfer(SavingsAccount receivingAccount, string amount)
        {
            var thisBalance = Withdraw(amount);
            if (thisBalance.IsError) return thisBalance.Errors;
            
            var newReceiverBalance = receivingAccount.Deposit(amount);
            if (newReceiverBalance.IsError) return newReceiverBalance.Errors;

            return Balance;
        }

        public static string StringToCurrencyString(string value)
        {
            var floatValue = StringToFloat(value);
            if (floatValue.IsError) throw new Exceptions.SavingsAccountException(Errors.SavingsAccountError.StringAmountToFloatFailed);
            return FloatToCurrencyString(floatValue.Value);
        }
        
        public static string FloatToCurrencyString(float value) =>
            $"{Convert.ToDecimal(value):C}".Remove(0, 1);

        public static float StringToFloat(string value)
        {
            try 
            { 
                var floatValue = float.Parse(value);
                if (floatValue < 0) throw new Exceptions.SavingsAccountException(Errors.SavingsAccountError.StringAmountToFloatFailed)
                return floatValue;
            }
            catch (Exception) 
            { return Errors.SavingsAccountError.StringAmountToFloatFailed; }
        }
    }
}
