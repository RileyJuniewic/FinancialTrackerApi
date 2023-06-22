using FinancialTracker.Common.Exceptions;
using FinancialTracker.Models.Enums;

namespace FinancialTracker.Common.Errors
{
    public static partial class Errors
    {
        public static class HttpContextError
        {
            public static ProblemDetailsException CannotFindClaimUserId =>
                new ProblemDetailsException(ErrorType.Failure,
                    "SavingsAccount.CannotFindClaimUserId",
                    "HttpContext cannot find UserId in claims");
            
            public static ProblemDetailsException HttpContextNull =>
                new ProblemDetailsException(ErrorType.Unexpected,
                    "SavingsAccount.HttpContextNull",
                    "HttpContext could not be located");
        }
    }
}
