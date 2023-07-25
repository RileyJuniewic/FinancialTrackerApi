using System.ComponentModel.DataAnnotations;
using FinancialTracker.Common.Validators;

namespace FinancialTracker.Common.Contracts.SavingsAccount.Request;

public record EditTransactionRequest(
        
    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Account Id")]
    [Guid]
    string AccountId,
    
    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Account Id")]
    [Guid]
    string TransactionId,
    
    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Date")]
    [DataType(DataType.Date)]
    DateTime Date,

    [StringLength(maximumLength: 100)]
    string Description);