namespace FinancialTracker.Common.Contracts.Transaction;

public record GetAllTransactionsRequest(
    Guid AccountId
    ) : AccountBase(AccountId);