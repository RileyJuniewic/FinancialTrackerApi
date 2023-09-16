using FinancialTracker.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Controllers;

[Authorize]
[AuthenticateAccount]
[ApiController]
[Route("[controller]")]
public class BaseController : ControllerBase
{
}