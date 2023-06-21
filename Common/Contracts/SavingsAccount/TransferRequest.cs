using FinancialTracker.Common.Contracts.Authentication;

namespace FinancialTracker.Common.Contracts.SavingsAccount;

public record TransferRequest(string AccountId, string TransferAmount, string ReceiverAccountId, string Description, LoginRequest LoginRequest);