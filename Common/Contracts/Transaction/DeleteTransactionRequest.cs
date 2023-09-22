using FinancialTracker.Common.Contracts.Common;
using FinancialTracker.Common.Validators;

namespace FinancialTracker.Common.Contracts.Transaction;

public record DeleteTransactionRequest(
    [Guid]
    Guid AccountId
    ) : IAuthenticatedRequest;