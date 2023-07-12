using System.ComponentModel.DataAnnotations;
using FinancialTracker.Common.Contracts.Authentication;
using FinancialTracker.Common.Validators;

namespace FinancialTracker.Common.Contracts.SavingsAccount.Request;

public record CloseAccountRequest(
    
    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Account Id")]
    [Guid]
    string AccountId,
    
    [Required]
    LoginRequest LoginRequest);