using FinancialTracker.Common.Contracts.Common;

namespace FinancialTracker.Common.Contracts.Transaction;

public record GetAllTransactionsRequest(
    Guid AccountId
    ) : IAuthenticatedRequest;