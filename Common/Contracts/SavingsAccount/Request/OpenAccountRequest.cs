using System.ComponentModel.DataAnnotations;
using FinancialTracker.Common.Validators;

namespace FinancialTracker.Common.Contracts.SavingsAccount.Request;

public record OpenAccountRequest(
    
    [Required(AllowEmptyStrings = false)]
    [StringLength(maximumLength: 25)]
    [Display(Name = "Account Name")]
    string AccountName,
    
    [Display(Name = "Initial Balance")]
    decimal InitialBalance);