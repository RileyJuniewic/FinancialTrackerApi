using FinancialTracker.Common.Contracts.Common;

namespace FinancialTracker.Common.Contracts.Transfer;

public record AddTransferRequest(
    Guid AccountId
    ) : IAuthenticatedRequest;