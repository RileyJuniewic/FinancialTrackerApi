using FinancialTracker.Common.Contracts.Common;

namespace FinancialTracker.Common.Contracts.Account;

public record GetAccountRequest(
    Guid AccountId
    ) : IAuthenticatedRequest;