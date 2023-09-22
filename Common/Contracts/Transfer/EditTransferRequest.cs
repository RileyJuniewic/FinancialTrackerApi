using FinancialTracker.Common.Contracts.Common;
using FinancialTracker.Common.Validators;

namespace FinancialTracker.Common.Contracts.Transfer;

public record EditTransferRequest(
    [Guid]
    Guid AccountId
    ) : IAuthenticatedRequest;