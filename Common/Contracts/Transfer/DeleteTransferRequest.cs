using FinancialTracker.Common.Contracts.Common;
using FinancialTracker.Common.Validators;

namespace FinancialTracker.Common.Contracts.Transfer;

public record DeleteTransferRequest(
    [Guid]
    Guid AccountId
    ) : IAuthenticatedRequest;