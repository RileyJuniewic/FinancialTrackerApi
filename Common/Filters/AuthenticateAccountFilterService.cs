using System.Security.Principal;
using Azure.Identity;
using FinancialTracker.Common.Contracts;
using FinancialTracker.Common.Contracts.Account;
using FinancialTracker.Common.Contracts.Common;
using FinancialTracker.Common.Exceptions;
using FinancialTracker.Common.Exceptions.Common;
using FinancialTracker.Models;
using FinancialTracker.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using NuGet.Protocol;

namespace FinancialTracker.Common.Filters;

public class AuthenticateAccountFilterService : IAsyncActionFilter
{
    private readonly IAccountService _accountService;

    public AuthenticateAccountFilterService(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var argumentValues = context.ActionArguments.Values.FirstOrDefault();
        if (argumentValues is IAuthenticatedRequest authenticatedRequest)
        {
            var userIdClaim = context.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == HttpClaims.UserId);
            if (userIdClaim == null)
                throw new ClaimNotFoundException(ExceptionMessages.InternalError);
            
            var validUserId = Guid.TryParse(userIdClaim.Value, out var userId);
            if (!validUserId)
                throw new UserIdParsingException(ExceptionMessages.InternalError);
            
            var request = new ValidateAccountRequest(authenticatedRequest.AccountId, userId);
            var accountAuthenticated = await _accountService.ValidateAccountAsync(request);

            if (!accountAuthenticated)
                throw new AuthenticationFailedException("Please provide a valid 'AccountId'");
        }
        
        await next();
    }
}