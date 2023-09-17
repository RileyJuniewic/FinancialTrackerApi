using FinancialTracker.Common.Contracts.Common;

namespace FinancialTracker.Common.Contracts.Transfer;

public record DeleteTransferRequest(
    Guid AccountId
    ) : IAuthenticatedRequest;