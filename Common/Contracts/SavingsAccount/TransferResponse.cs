namespace FinancialTracker.Common.Contracts.SavingsAccount;

public record TransferResponse(string AccountId, string AccountName, string Balance, string ReceiverId);