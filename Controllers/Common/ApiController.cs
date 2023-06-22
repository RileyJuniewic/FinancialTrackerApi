using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Controllers.Common
{
    [ApiController]
    [Authorize]
    public class ApiController : ControllerBase
    {
    }
}
