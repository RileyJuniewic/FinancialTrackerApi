using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Common.Validators;

public class StringCurrencyAttribute : ValidationAttribute
{
    public StringCurrencyAttribute()
    {
        const string defaultErrorMessage = "must be in currency format.";
        ErrorMessage ??= defaultErrorMessage;
    }

    public int MaxLength { get; set; } = -1;
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        try
        {
            var amount = value?.ToString();
            if (amount == null)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            
            if (MaxLength > 0 && amount.Length > MaxLength)
                return new ValidationResult(validationContext.DisplayName + " must be a string the length of " + MaxLength);
                
            
            var floatAmount = float.Parse(amount);
            if (floatAmount < 0)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }
        catch (Exception)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
        
        return ValidationResult.Success;
    }
}