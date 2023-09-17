namespace FinancialTracker.Common.Contracts.Account;

public record GetAllAccountsRequest(
    Guid AccountId
    ) : IAccountRequest;