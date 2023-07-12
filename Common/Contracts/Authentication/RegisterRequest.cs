using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Common.Contracts.Authentication
{
    public record RegisterRequest(
        
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "First Name")]
        [StringLength(maximumLength: 35)]
        string FirstName,
        
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Last Name")]
        [StringLength(maximumLength: 35)]
        string LastName,
        
        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: 62)]
        string Email,
        
        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: 128)]
        string Password);
}
