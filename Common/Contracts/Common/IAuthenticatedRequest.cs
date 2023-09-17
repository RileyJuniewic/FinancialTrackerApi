namespace FinancialTracker.Common.Contracts.Common;

public interface IAuthenticatedRequest
{
    public Guid AccountId { get; }
}