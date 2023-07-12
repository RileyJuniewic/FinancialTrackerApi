using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Common.Validators;

public class GuidAttribute : ValidationAttribute
{
    public GuidAttribute()
    {
        const string defaultErrorMessage = "Value must be in format of a Guid";
        ErrorMessage ??= defaultErrorMessage;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        try
        {
            var valueString = value?.ToString();
            if (valueString == null)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            if (valueString.Length is > 36 or < 36)
                return new ValidationResult(validationContext.DisplayName + " must be a Guid formatted string with the length of 36");

            var isValid = Guid.TryParse(valueString, out var _);
            
            if (!isValid)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
        catch (Exception)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }
}