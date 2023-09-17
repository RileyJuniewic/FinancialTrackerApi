using FinancialTracker.Common.Contracts.Common;

namespace FinancialTracker.Common.Contracts.Account;

public record EditAccountRequest(
    Guid AccountId
    ) : IAuthenticatedRequest;