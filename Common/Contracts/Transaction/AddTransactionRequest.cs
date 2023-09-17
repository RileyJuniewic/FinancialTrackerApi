namespace FinancialTracker.Common.Contracts.Transaction;

public record AddTransactionRequest(
    Guid AccountId
    ) : IAccountRequest;