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
        
        [StringLength(maximumLength: 100)]
        string Description,
        
        [Required]
        [Display(Name = "Amount")]
        decimal Amount);
}
