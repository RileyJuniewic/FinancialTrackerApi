using FinancialTracker.Persistance;
using FinancialTracker.Services;
using FinancialTracker.Services.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FinancialTracker.Common.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddControllers();
            services.AddAuth(configuration);
            
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddServices();
            
            services.AddCors(options =>
            {
                options.AddPolicy("CorsLocalHost8080", policy =>
                {
                    policy./*WithOrigins("http://localhost:8080", "http://192.168.1.236:8080")*/SetIsOriginAllowed(origin => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            return services;
        }

        private static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    options.SlidingExpiration = true;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IHttpContextHelperService, HttpContextHelperService>();
            services.AddScoped<IAuthCookieService, AuthCookieService>();
            services.AddScoped<ISqlDataAccess, SqlDataAccess>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISavingsAccountService, SavingsAccountService>();

            return services;
        }
    }
}
