using FinancialTracker.Common.Contracts.Common;

namespace FinancialTracker.Common.Contracts.Transfer;

public record EditTransferRequest(
    Guid AccountId
    ) : IAuthenticatedRequest;