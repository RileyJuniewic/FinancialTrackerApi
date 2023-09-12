using System.ComponentModel.DataAnnotations;
using FinancialTracker.Common.Validators;
using FinancialTracker.Models.Enums;

namespace FinancialTracker.Common.Contracts.SavingsAccount.Request;

public record EditTransactionRequest(
        
    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Account Id")]
    [Guid]
    string AccountId,
    
    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Transaction Id")]
    [Guid]
    string TransactionId,
    
    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Date")]
    [DataType(DataType.Date)]
    DateTime Date,
    
    [Required]
    [Display(Name = "Transaction Type")]
    [TransactionEnum]
    TransactionType Type,
    
    [Required]
    [Display(Name = "Transfer Amount")]
    decimal Amount, 

    [StringLength(maximumLength: 100)]
    string Description);