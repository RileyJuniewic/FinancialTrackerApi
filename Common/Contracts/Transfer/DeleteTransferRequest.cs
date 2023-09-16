namespace FinancialTracker.Common.Contracts.Transfer;

public record DeleteTransferRequest(
    Guid AccountId
    ) : AccountBase(AccountId);