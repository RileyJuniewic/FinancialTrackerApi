namespace FinancialTracker.Common.Contracts.Transaction;

public record EditTransactionRequest(
    Guid AccountId
    ) : AccountBase(AccountId);