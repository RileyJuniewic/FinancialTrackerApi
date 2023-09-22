using FinancialTracker.Common.Contracts.Common;
using FinancialTracker.Common.Validators;

namespace FinancialTracker.Common.Contracts.Transaction;

public record GetAllTransactionsRequest(
    [Guid]
    Guid AccountId
    ) : IAuthenticatedRequest;