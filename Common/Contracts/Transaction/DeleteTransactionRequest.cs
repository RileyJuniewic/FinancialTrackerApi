using FinancialTracker.Common.Contracts.Common;

namespace FinancialTracker.Common.Contracts.Transaction;

public record DeleteTransactionRequest
    (
        Guid AccountId
        ) : IAuthenticatedRequest;