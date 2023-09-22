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
            var guidValue = (Guid)(value ?? throw new ArgumentNullException(nameof(value)));

            if (guidValue == Guid.Empty)
            {
                ErrorMessage = "Value cannot be empty";
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