namespace FinancialTracker.Common.Contracts.SavingsAccount.Response;

public record TransactionTypesSumFromRangeResponse(
    
    decimal DepositSum,
    
    decimal WithdrawalSum,
    
    decimal TransferInSum,
    
    decimal TransferOutSum
    );