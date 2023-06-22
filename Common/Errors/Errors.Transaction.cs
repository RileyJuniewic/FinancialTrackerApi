using FinancialTracker.Common.Exceptions;
using FinancialTracker.Models.Enums;

namespace FinancialTracker.Common.Errors
{
    public static partial class Errors
    {
        public static class TransactionError
        {
            public static ProblemDetailsException InvalidTransactionType =>
                new ProblemDetailsException(ErrorType.Validation,
                    "SavingsAccount.InvalidTransactionType",
                    "Transaction type provided is not registered as a valid type.");
            
        }
    }
}
