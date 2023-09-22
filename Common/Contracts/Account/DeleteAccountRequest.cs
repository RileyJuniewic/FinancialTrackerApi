using FinancialTracker.Common.Contracts.Common;
using FinancialTracker.Common.Validators;

namespace FinancialTracker.Common.Contracts.Account;

public record DeleteAccountRequest(
    [Guid]
    Guid AccountId
) : IAuthenticatedRequest;