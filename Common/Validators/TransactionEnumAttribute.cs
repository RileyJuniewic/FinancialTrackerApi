using System.ComponentModel.DataAnnotations;
using FinancialTracker.Models.Enums;

namespace FinancialTracker.Common.Validators;

public class TransactionEnumAttribute : ValidationAttribute
{
    public TransactionEnumAttribute()
    {
        const string defaultErrorMessage = "Value must be a Transaction Type.";
        ErrorMessage ??= defaultErrorMessage;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        try
        {
            var result = (TransactionType)(value ?? -1);
            if (!Enum.IsDefined(typeof(TransactionType), result))
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
        catch (Exception)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
        
        return ValidationResult.Success;
    }
}