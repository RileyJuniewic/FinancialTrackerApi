using System.ComponentModel.DataAnnotations;
using FinancialTracker.Common.Validators;

namespace FinancialTracker.Common.Contracts.SavingsAccount.Request;

public record OpenAccountRequest(
    
    [Required(AllowEmptyStrings = false)]
    [StringLength(maximumLength: 25)]
    [Display(Name = "Account Name")]
    string AccountName,
    
    [StringCurrency(MaxLength = 12)]
    [Display(Name = "Initial Balance")]
    string InitialBalance);