using ErrorOr;
using FinancialTracker.Common.Errors;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace FinancialTracker.Services.Common
{
    public interface IHttpContextHelperService
    {
        ErrorOr<Claim> GetClaimUserId();
    }

    public class HttpContextHelperService : IHttpContextHelperService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextHelperService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ErrorOr<Claim> GetClaimUserId()
        {
            if (_httpContextAccessor.HttpContext is null)
            { return Errors.HttpContextError.HttpContextNull; }

            // var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims =>
            //     claims.Properties.FirstOrDefault(keys =>
            //         keys.Value == JwtRegisteredClaimNames.Sub).Value != null);

            var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "UserId");

            if (claim is null)
            { return Errors.HttpContextError.CannotFindClaimUserId; }

            return claim;
        }
    }
}
