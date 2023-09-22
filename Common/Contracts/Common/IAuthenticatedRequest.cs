using FinancialTracker.Common.Validators;
using Microsoft.Build.Framework;

namespace FinancialTracker.Common.Contracts.Common;

public interface IAuthenticatedRequest
{
    public Guid AccountId { get; }
}