using FinancialTracker.Common.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddDependencies(builder.Configuration);
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
        app.UseExceptionHandler("/errors-development");
    }
    else
    {
        app.UseExceptionHandler("/error");
    }

    app.UseHttpsRedirection();
    app.UseCors("CorsLocalHost8080");
    app.UseCookiePolicy();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
