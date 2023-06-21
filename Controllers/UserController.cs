using ErrorOr;
using FinancialTracker.Common.Contracts.Authentication;
using FinancialTracker.Controllers.Common;
using FinancialTracker.Models;
using FinancialTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequest request) =>
            (await _userService.Login(request)).Match(Ok, Problem);

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterRequest request) =>
            (await _userService.Register(request)).Match(Ok, Problem);
    }
}
