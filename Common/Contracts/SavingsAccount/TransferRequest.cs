using FinancialTracker.Common.Contracts.Authentication;

namespace FinancialTracker.Common.Contracts.SavingsAccount;

public record TransferRequest(string AccountId, string ReceiverAccountId, string TransferAmount, string Description, LoginRequest LoginRequest);