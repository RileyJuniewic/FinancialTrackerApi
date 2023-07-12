using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Common.Contracts.Authentication
{
    public record LoginRequest (
        
        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: 62)]
        string Email,
        
        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: 128)]
        string Password);
}
