namespace FinancialTracker.Common.Contracts.SavingsAccount.Response;

public record TransactionTypesSumFromRangeResponse(
    
    string DepositSum,
    
    string WithdrawalSum,
    
    string TransferInSum,
    
    string TransferOutSum
    );