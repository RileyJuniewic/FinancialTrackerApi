namespace FinancialTracker.Common.Contracts.SavingsAccount;

public record CloseAccountResponse(DateTime CloseDate, string AccountName, string AccountId, string UserId);