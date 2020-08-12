using Lycium.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;

namespace Lycium.Authentication.Client.Services
{
    public class FreeSqlClientHostService : IClientHostService
    {

        private static readonly HttpClient _request;
        static FreeSqlClientHostService()
        {
            _request = new HttpClient();
            _request.BaseAddress = new Uri(ClientConfiguration.ServerUrl);
            _request.DefaultRequestHeaders.Add(LyciumConfiguration.SECRET_KEY, ClientConfiguration.SecretKey);
        }


        /// <summary>
        /// 刷新主机通信Token
        /// </summary>
        /// <returns></returns>
        public override bool RefreshToken()
        {

            try
            {

                var result = _request.GetStringAsync("api/Host/refreshToken").Result;
                var tempHost = JsonSerializer.Deserialize<TempHost>(result);
                if (tempHost != default)
                {
                    ClientConfiguration.SetTokenInfo(
                        tempHost.HostToken
                        , tempHost.TokenAliveTime
                        , tempHost.TokenCreateTime);
                    return true;
                }

            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public class TempHost 
        {
            public string HostToken { get; set; }

            public long TokenAliveTime { get; set; }
            public long TokenCreateTime { get; set; }

        }


    }
}
