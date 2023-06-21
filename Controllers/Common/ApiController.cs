using ErrorOr;
using GroceryStore.Api.Common.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Controllers.Common
{
    [ApiController]
    [Authorize]
    public class ApiController : ControllerBase
    {
        private IActionResult ValidationProblem(List<Error> errors)
        {
            return ValidationProblem(errors);
        }

        private IActionResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(statusCode: statusCode, title: error.Description);
        }

        protected IActionResult Problem(List<Error> errors)
        {
            if (errors.Count == 0) return Problem();

            if (errors.All(x => x.Type == ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            HttpContext.Items[HttpContextItemKeys.Errors] = errors;
            var firstError = errors.First();
            return Problem(firstError);
        }
    }
}
