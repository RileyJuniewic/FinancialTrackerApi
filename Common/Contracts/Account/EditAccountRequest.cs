namespace FinancialTracker.Common.Contracts.Account;

public record EditAccountRequest(
    Guid AccountId
    ) : AccountBase(AccountId);