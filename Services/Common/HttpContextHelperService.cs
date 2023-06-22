using FinancialTracker.Common.Errors;
using System.Security.Claims;

namespace FinancialTracker.Services.Common
{
    public interface IHttpContextHelperService
    {
        Claim GetClaimUserId();
    }

    public class HttpContextHelperService : IHttpContextHelperService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextHelperService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Claim GetClaimUserId()
        {
            if (_httpContextAccessor.HttpContext is null) 
                throw Errors.HttpContextError.HttpContextNull;

            var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "UserId");

            if (claim is null) 
                throw Errors.HttpContextError.CannotFindClaimUserId;

            return claim;
        }
    }
}
