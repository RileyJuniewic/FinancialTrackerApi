using FinancialTracker.Common.Contracts.Common;

namespace FinancialTracker.Common.Contracts.Transaction;

public record GetTransactionRequest(
    Guid AccountId
    ) : IAuthenticatedRequest;