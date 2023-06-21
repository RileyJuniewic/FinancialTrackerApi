using FinancialTracker.Models.Enums;

namespace FinancialTracker.Common.Contracts.SavingsAccount
{
    public record TransactionRequest(
        string SavingsAccountId,
        TransactionType Type,
        string Description,
        string Amount);
}
