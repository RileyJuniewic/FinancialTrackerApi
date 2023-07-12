namespace FinancialTracker.Common.Contracts.SavingsAccount.Response;

public record TransferResponse(
    string AccountId, 
    string AccountName, 
    string Balance, 
    string ReceiverId);