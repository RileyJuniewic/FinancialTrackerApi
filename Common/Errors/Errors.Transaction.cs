using ErrorOr;

namespace FinancialTracker.Common.Errors
{
    public static partial class Errors
    {
        public static class TransactionError
        {
            public static Error InvalidTransactionType => Error.NotFound(code: "Transaction.InvalidTransactionType", description: "Transaction type provided is not registered as a valid type.");
        }
    }
}
