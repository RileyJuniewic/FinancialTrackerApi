using FinancialTracker.Common.Contracts.Authentication;
using FinancialTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequest request) =>
            Ok(await _userService.LoginAsync(request));

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterRequest request) =>
            Ok(await _userService.RegisterAsync(request));
            
        [HttpGet]
        [Route("RefreshJwtToken")]
        public async Task<IActionResult> Login() =>
            Ok(await _userService.LoginAsync());

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return Ok();
        }
    }
}
