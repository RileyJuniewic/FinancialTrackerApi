using FinancialTracker.Common.Contracts.Common;

namespace FinancialTracker.Common.Contracts.Account;

public record ValidateAccountRequest(
    Guid AccountId, 
    Guid UserId);