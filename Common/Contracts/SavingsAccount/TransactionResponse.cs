using FinancialTracker.Models;

namespace FinancialTracker.Common.Contracts.SavingsAccount
{
    public record TransactionResponse(
        Transaction Transaction,
        Models.SavingsAccount SavingsAccount);
}
