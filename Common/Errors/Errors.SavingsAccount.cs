using ErrorOr;

namespace FinancialTracker.Common.Errors
{
    public static partial class Errors
    {
        public static class SavingsAccountError
        {
            public static Error StringAmountToFloatFailed => 
                Error.Failure(code: "SavingsAccount.StringAmountToFloatFailed", 
                    description: "Cannot convert string amount to a numerical value. String value must only consist of numbers");
            public static Error AmountOverdraw => 
                Error.Failure(code: "SavingsAccountError.AmountOverdraw", 
                    description: "Cannot overdraw your account, your balance must always be higher than $0.00");
            public static Error CannotFindAccount => 
                Error.Failure(code: "SavingsAccountError.CannotFindAccount",
                    description: "Cannot find an existing Savings Account");
            public static Error CannotVerifyLoginCredentials => 
                Error.Failure(code: "SavingsAccountError.CannotVerifyLoginCredentials",
                    description: "Login credentials provided are invalid. Please try again.");
            public static Error BalanceMustBeZero => 
                Error.Failure(code: "SavingsAccountError.BalanceMustBeZero",
                    description: "The balance of your account must be $0.00.");
            public static Error SqlErrorCannotCompleteTransfer => 
                Error.Failure(code: "SavingsAccountError.SqlErrorCannotCompleteTransfer",
                    description: "There is an error with your request. Validate your request and try again.");
        }
    }
}
