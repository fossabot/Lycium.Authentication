using Lycium.Configuration;
using System;
using System.Net.Http;

namespace Lycium.Authentication
{
    public static class ClientHostDebugService
    {

        /// <summary>
        /// 心跳检测实现
        /// </summary>
        /// <returns></returns>
        public static bool Heartbeat()
        {
            return ClientConfiguration.CheckHeartbeat();
        }


        /// <summary>
        /// 合法性检测实现
        /// </summary>
        /// <returns></returns>
        public static string InfoFromServer(string action)
        {
            try
            {

                using var client = new HttpClient();
                client.BaseAddress = new Uri(ClientConfiguration.ServerUrl);
                client.DefaultRequestHeaders.Add(LyciumConfiguration.SECRET_KEY, ClientConfiguration.SecretKey);
                client.DefaultRequestHeaders.Add(LyciumConfiguration.HOST_TOKEN, ClientConfiguration.HostToken);
                var result = client.GetStringAsync("api/HostDebug/"+action).Result;
                return result;

            }
            catch (Exception ex)
            {

                return ex.Message;

            }
        }

    }
}
