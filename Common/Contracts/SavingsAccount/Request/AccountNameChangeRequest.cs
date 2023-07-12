using System.ComponentModel.DataAnnotations;
using FinancialTracker.Common.Validators;

namespace FinancialTracker.Common.Contracts.SavingsAccount.Request;

public record AccountNameChangeRequest(
    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Account Id")]
    [Guid]
    string AccountId,
    
    [Required(AllowEmptyStrings = false)]
    [StringLength(maximumLength: 25)]
    string Name);