using System.ComponentModel.DataAnnotations;
using FinancialTracker.Common.Validators;
using FinancialTracker.Models.Enums;

namespace FinancialTracker.Common.Contracts.SavingsAccount.Request
{
    public record TransactionRequest(
        
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Account Id")]
        [Guid]
        string AccountId,
        
        [Required]
        [TransactionEnum]
        TransactionType Type,
        
        [StringLength(maximumLength: 200)]
        string Description,
        
        [Required]
        [StringCurrency(MaxLength = 12)]
        [Display(Name = "Amount")]
        string Amount);
}
