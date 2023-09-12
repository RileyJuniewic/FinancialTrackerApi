using System.ComponentModel.DataAnnotations;
using FinancialTracker.Common.Validators;

namespace FinancialTracker.Common.Contracts.SavingsAccount.Request;

public record DeleteTransactionRequest(
    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Account Id")]
    [Guid]
    string SavingsAccountId, 
    
    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Transaction Id")]
    [Guid]
    string TransactionId);