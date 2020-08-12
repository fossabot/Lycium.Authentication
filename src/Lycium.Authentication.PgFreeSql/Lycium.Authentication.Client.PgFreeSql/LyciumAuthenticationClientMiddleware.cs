using Lycium.Authentication;
using Lycium.Authentication.Client;
using Lycium.Authentication.Client.Services;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LyciumAuthenticationClientMiddleware
    {
        public static void AddLyciumAuthenticationPgSql(this IServiceCollection services, Func<LyciumAuthenticationClientOptionas, LyciumAuthenticationClientOptionas> func = null)
        {
            services.AddLyciumAuthentication(func);
            services.AddScoped<LyciumRequest>();
            services.AddScoped<LoginService>();
            services.AddScoped<IContextInfoService, FreesqlContextInfoService>();
            services.AddScoped<IClientHostService, FreeSqlClientHostService>();
            services.AddScoped<IClientResourceService, FreeSqlClientResourceService>();
            services.AddScoped<IClientTokenService, FreeSqlClientTokenService>();
        }
    }
}
