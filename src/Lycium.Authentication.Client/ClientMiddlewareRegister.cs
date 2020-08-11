using Lycium.Authentication;
using Lycium.Authentication.Utils;
using Lycium.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{

    public static class LyciumServiceInjection
    {

        public static void AddLyciumAuthentication(this IServiceCollection services, Func<LyciumAuthenticationClientOptionas, LyciumAuthenticationClientOptionas> func = null)
        {
            services.AddScoped<IAuthenticationDelegate, AuthenticationPiplineDelegate>();
            LyciumAuthenticationClientOptionas optionas = new LyciumAuthenticationClientOptionas();
            func?.Invoke(optionas);

        }

    }

    public class LyciumAuthenticationClientOptionas 
    {

        public LyciumAuthenticationClientOptionas SetServerUrl(string url)
        {
            ClientConfiguration.ServerUrl = url;
            return this;
        }

        public LyciumAuthenticationClientOptionas SetSecretKey(string secretKey)
        {
            ClientConfiguration.SecretKey = secretKey;
            return this;
        }

    }


}

namespace Microsoft.AspNetCore.Builder
{

    public static class ClientMiddlewareRegister
    {

        public static async void UseLyciumAuthentication(this IApplicationBuilder app, IAuthenticationDelegate @delegate)
        {

            ClientConfiguration.SetAppBuilderCache(app);
            app.Use(@delegate.PipelineDelegate);
            while (ClientConfiguration.GetDataSources() == null || ClientConfiguration.GetDataSources().Count == 0)
            {
                await Task.Delay(2000);
            }
            //添加本地白名单
            LyciumScanHelper.AddLyciumRoute();
            @delegate.StartSync();
        }

    }



}
