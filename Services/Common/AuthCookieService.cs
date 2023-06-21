using System.Security.Claims;
using FinancialTracker.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FinancialTracker.Services.Common;

public interface IAuthCookieService
{
    Task CreateSignInCookie(User user);
}

public class AuthCookieService : IAuthCookieService
{
    private readonly IHttpContextAccessor _httpContext;

    public AuthCookieService(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public async Task CreateSignInCookie(User user)
    {
        if (_httpContext.HttpContext == null)
            throw new Exception();
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim("FullName", $"{user.FirstName} {user.LastName}"),
            new Claim("UserId", user.Id)
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            IsPersistent = true,
            IssuedUtc = DateTime.UtcNow,
        };

        await _httpContext.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity), 
            authProperties);

    }
}