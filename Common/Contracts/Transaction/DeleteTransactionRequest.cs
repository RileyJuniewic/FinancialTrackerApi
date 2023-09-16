namespace FinancialTracker.Common.Contracts.Transaction;

public record DeleteTransactionRequest
    (
        Guid AccountId
        ) : AccountBase(AccountId);