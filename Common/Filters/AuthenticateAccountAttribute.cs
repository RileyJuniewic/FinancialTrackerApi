using FinancialTracker.Common.Contracts;
using FinancialTracker.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using NuGet.Protocol;

namespace FinancialTracker.Common.Filters;

public class AuthenticateAccountAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var dict = context.ActionArguments.Values.FirstOrDefault();
        if (dict != null)
        {
            try
            {
                var test = (TestBaseRequest)dict;
            }
            catch (SqlException e) { throw; }
            catch (Exception) { await base.OnActionExecutionAsync(context, next); }
        }
        await base.OnActionExecutionAsync(context, next);
    }
}