using System.ComponentModel.DataAnnotations;
using FinancialTracker.Persistance;

namespace FinancialTracker.Common.Validators;

public class GuidValidIdentifierAttribute : ValidationAttribute
{
    private readonly ISqlDataAccess _sqlDataAccess;
    
    public GuidValidIdentifierAttribute(ISqlDataAccess sqlDataAccess)
    {
        _sqlDataAccess = sqlDataAccess;
    }
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var id = Guid.Empty;
        var isGuidValid = Guid.TryParse(value?.ToString(), out id);
        return ValidationResult.Success;
    }
}