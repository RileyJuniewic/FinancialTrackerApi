using FinancialTracker.Common.Contracts;
using FinancialTracker.Persistance;
using Newtonsoft.Json;

namespace FinancialTracker.Common.Exceptions;

public class GuidValidationMiddleware
{
    private readonly RequestDelegate _next;

    public GuidValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context, ISqlDataAccess sqlDataAccess)
    {
        if (context.Request.ContentLength is null or < 1)
        {
            await _next(context);
            return;
        }
        
        context.Request.EnableBuffering();
            
        using var reader = new StreamReader(context.Request.Body);
        var bodyJson = await reader.ReadToEndAsync();
        context.Request.Body.Seek(0, SeekOrigin.Begin);
            
        var request = JsonConvert.DeserializeObject<TestRequest>(bodyJson);
        if (request != null)
        {
            var result = await sqlDataAccess.LoadData<string, dynamic>("TestProcedure", new {request.Id});
            if (result.FirstOrDefault() != request.Id) throw new Exception("Trouble In Terrorist Town");
        }
            
        await _next(context);
    }
}