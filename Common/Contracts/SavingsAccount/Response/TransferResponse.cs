using FinancialTracker.Models;

namespace FinancialTracker.Common.Contracts.SavingsAccount.Response;

public record TransferResponse(
    Models.Transaction TransferOut,
    Models.Transaction TransferIn);