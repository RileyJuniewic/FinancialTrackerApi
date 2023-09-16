namespace FinancialTracker.Common.Contracts.Transaction;

public record GetTransactionRequest(
    Guid AccountId
    ) : AccountBase(AccountId);