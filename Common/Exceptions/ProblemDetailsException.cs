using FinancialTracker.Models.Enums;

namespace FinancialTracker.Common.Exceptions;

public class ProblemDetailsException : Exception
{

    public ProblemDetailsException(ErrorType type, string title, string details, int statusCode = -1)
        :base(details)
    {
        if (statusCode == -1)
        {
            StatusCode = type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
        }
        else
        {
            StatusCode = statusCode;
        }

        Details = details;
        Type = type.ToString();
        Title = title;
    }

    public int StatusCode { get; }
    public string Type { get; }
    public string Title { get; }
    public string Details { get; }
}