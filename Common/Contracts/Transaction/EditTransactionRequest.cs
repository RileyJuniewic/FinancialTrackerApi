using FinancialTracker.Common.Contracts.Common;

namespace FinancialTracker.Common.Contracts.Transaction;

public record EditTransactionRequest(
    Guid AccountId
    ) : IAuthenticatedRequest;