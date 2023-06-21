namespace FinancialTracker.Common.Contracts.Authentication
{
    public record LoginRequest (
        string Email,
        string Password);
}
