using FinancialTracker.Models;

namespace FinancialTracker.Common.Contracts.SavingsAccount.Response;

public record TransferResponse(
    Transaction TransferOut,
    Transaction TransferIn);