using FinancialTracker.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Controllers.Common
{
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        [Route("/errors")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError()
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
            return Problem(title: exceptionHandlerFeature.Error.Message);
        }

        [Route("/errors-development")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment())
            {
                return NotFound();
            }

            var exceptionHandlerFeature =
                HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            if (exceptionHandlerFeature.Error is ProblemDetailsException problemDetailsException)
            {
                return Problem(detail: problemDetailsException.Details, title: problemDetailsException.Title,
                    statusCode: problemDetailsException.StatusCode, type: problemDetailsException.Type );
            }
            
            return Problem(
                detail: exceptionHandlerFeature.Error.StackTrace,
                title: exceptionHandlerFeature.Error.Message);
        }
    }
}
