using API.Classes;
using API.Utils.Authentication;
using API.Utils.Interfaces;
using API.Utils.Services;
using DB.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Utils;

namespace API
{
    public static class DependenciesRegistrar
    {
        public static IServiceCollection Inject(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IApiControllerWrapper, ApiControllerWrapper>();
            services.AddScoped<ISessionContextService, SessionContextService>();

            services.AddScoped<IRepositoryProvider, RepositoryProvider>();
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();

            services.AddTransient<IUsersService, UsersService>(); 

            services.AddTransient<IUserCredentialsAuthService, UserCredentialsAuthService>();

            services.AddTransient<IApiResponseFormatter, ApiResponseFormatter>();

            return services;
        }
    }
}
