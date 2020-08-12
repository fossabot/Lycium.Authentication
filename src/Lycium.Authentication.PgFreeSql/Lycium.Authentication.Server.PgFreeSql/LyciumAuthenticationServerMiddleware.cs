using Lycium.Authentication;
using Lycium.Authentication.Server;
using Lycium.Authentication.Server.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LyciumAuthenticationServerMiddleware
    {
        public static void AddLyciumAuthenticationPgSql(this IServiceCollection services)
        {
           
            services.AddScoped<IServerConfigurationService, FreeSqlServerConfigurationService>();
            services.AddScoped<IServerHostService, FreeSqlServerHostService>();
            services.AddScoped<IContextInfoService, FreeSqlContextInfoService>();
            services.AddScoped<IServerResourceService, FreeSqlServerResourceService>();
            services.AddScoped<IServerHostGroupService, FreeSqlServerHostGroupService>();
            services.AddScoped<IServerTokenService, FreeSqlServerTokenService>();
            services.AddScoped<IResourceNotify, HttpResourceNotify>();
            services.AddScoped<ITokenNotify, HttpTokenNotify>();
            services.AddScoped<LyciumRequest>();

        }
    }
}
