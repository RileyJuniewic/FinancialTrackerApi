using FinancialTracker.Common.Exceptions;
using FinancialTracker.Models.Enums;

namespace FinancialTracker.Common.Errors
{
    public static partial class Errors
    {
        public static class UserError
        {
            public static ProblemDetailsException InvalidCredentials =>
                new ProblemDetailsException(ErrorType.Validation,
                    "SavingsAccount.InvalidCredentials",
                    "Login failed due to invalid credentials.");
            
            public static ProblemDetailsException UserNotFound =>
                new ProblemDetailsException(ErrorType.Failure,
                    "SavingsAccount.UserNotFound",
                    "User was not found in search.");
            
            public static ProblemDetailsException DuplicateEmail =>
                new ProblemDetailsException(ErrorType.Conflict,
                    "SavingsAccount.DuplicateEmail",
                    "Email is already registered.");
            
        }
    }
}
