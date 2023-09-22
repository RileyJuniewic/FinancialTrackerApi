using FinancialTracker.Common.Contracts.Common;
using FinancialTracker.Common.Validators;

namespace FinancialTracker.Common.Contracts.Account;

public record GetAllAccountsRequest(
    [Guid]
    Guid AccountId
    ) : IAuthenticatedRequest;