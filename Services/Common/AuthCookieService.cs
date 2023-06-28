using System.Security.Claims;
using FinancialTracker.Common.Errors;
using FinancialTracker.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FinancialTracker.Services.Common;

public interface IAuthCookieService
{
    Task SignInAsync(User user);
    Task SignOutAsync();
}

public class AuthCookieService : IAuthCookieService
{
    private readonly IHttpContextAccessor _httpContext;

    public AuthCookieService(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public async Task SignInAsync(User user)
    {
        if (_httpContext.HttpContext == null)
            throw Errors.HttpContextError.HttpContextNull;
        
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

    public async Task SignOutAsync()
    {
        if (_httpContext.HttpContext == null)
            throw Errors.HttpContextError.HttpContextNull;

        await _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}