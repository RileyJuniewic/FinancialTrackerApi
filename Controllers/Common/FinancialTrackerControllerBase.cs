using FinancialTracker.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Controllers.Common;

[Authorize]
[ServiceFilter(typeof(AuthenticateAccountFilterService))]
[ApiController]
[Route("[controller]")]
public class FinancialTrackerControllerBase : ControllerBase
{
}