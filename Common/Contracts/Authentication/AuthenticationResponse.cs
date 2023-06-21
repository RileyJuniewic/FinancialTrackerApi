using FinancialTracker.Models;

namespace FinancialTracker.Common.Contracts.Authentication
{
    public record AuthenticationResponse(
        User user,
        string Token);
}
