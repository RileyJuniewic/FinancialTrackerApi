using FinancialTracker.Common.Contracts.Common;

namespace FinancialTracker.Common.Contracts.Transaction;

public record AddTransactionRequest(
    Guid AccountId
    ) : IAuthenticatedRequest;