using FinancialTracker.Common.Contracts.Authentication;

namespace FinancialTracker.Common.Contracts.SavingsAccount;

public record CloseAccountRequest(string AccountId, LoginRequest LoginRequest);