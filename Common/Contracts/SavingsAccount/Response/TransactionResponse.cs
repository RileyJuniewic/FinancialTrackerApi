using FinancialTracker.Models;

namespace FinancialTracker.Common.Contracts.SavingsAccount.Response
{
    public record TransactionResponse(
        Transaction Transaction,
        Models.SavingsAccount SavingsAccount);
}
