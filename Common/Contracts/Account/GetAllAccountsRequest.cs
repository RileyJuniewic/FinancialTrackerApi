using FinancialTracker.Common.Contracts.Common;

namespace FinancialTracker.Common.Contracts.Account;

public record GetAllAccountsRequest(
    Guid AccountId
    ) : IAuthenticatedRequest;