using FinancialTracker.Common.Contracts.Authentication;
using FinancialTracker.Controllers.Common;
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
            Ok(await _userService.Login(request));

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterRequest request) =>
            Ok(await _userService.Register(request));
            
        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login() =>
            Ok(await _userService.Login());
    }
}
