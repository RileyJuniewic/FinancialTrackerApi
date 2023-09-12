using FinancialTracker.Models;

namespace FinancialTracker.Common.Contracts.SavingsAccount.Response
{
    public record TransactionResponse(
        Models.Transaction Transaction,
        Models.Account Account);
}
