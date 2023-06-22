using FinancialTracker.Common.Exceptions;
using FinancialTracker.Models.Enums;

namespace FinancialTracker.Common.Errors
{
    public static partial class Errors
    {
        public static class SavingsAccountError
        {
            public static ProblemDetailsException StringAmountToFloatFailed =>
                new ProblemDetailsException(ErrorType.Failure,
                    "SavingsAccount.StringAmountToFloatFailed",
                    "Cannot convert string amount to a numerical value. String value must only consist of numbers");
            
            public static ProblemDetailsException AmountOverdraw =>
                new ProblemDetailsException(ErrorType.Failure,
                    "SavingsAccount.AmountOverdraw",
                    "Cannot overdraw your account, your balance must always be higher than $0.00");
            
            public static ProblemDetailsException CannotFindAccount =>
                new ProblemDetailsException(ErrorType.NotFound,
                    "SavingsAccount.CannotFindAccount",
                    "Cannot find an existing Savings Account");
            
            public static ProblemDetailsException CannotVerifyLoginCredentials =>
                new ProblemDetailsException(ErrorType.Validation,
                    "SavingsAccount.CannotVerifyLoginCredentials",
                    "Login credentials provided are invalid. Please try again.");
            
            public static ProblemDetailsException BalanceMustBeZero =>
                new ProblemDetailsException(ErrorType.Validation,
                    "SavingsAccount.BalanceMustBeZero",
                    "The balance of your account must be $0.00.");
            
            public static ProblemDetailsException SqlErrorCannotCompleteTransfer =>
                new ProblemDetailsException(ErrorType.Failure,
                    "SavingsAccount.SqlErrorCannotCompleteTransfer",
                    "There is an error with your request. Validate your request and try again.");
        }
    }
}
