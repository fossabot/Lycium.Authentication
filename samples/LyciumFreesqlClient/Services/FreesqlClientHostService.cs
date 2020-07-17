using Lycium.Authentication;
using Lycium.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;

namespace LyciumFreesqlClient.Services
{
    public class FreesqlClientHostService : IClientHostService
    {

        private static readonly HttpClient _request;
        static FreesqlClientHostService()
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
                var result = _request.GetStringAsync("host/refreshToken").Result;
                var instance = JsonSerializer.Deserialize<(string Token, long alivaTime, long createTime)>(result);
                if (result != default)
                {
                    ClientConfiguration.SetTokenInfo(
                        instance.Token
                        , instance.alivaTime
                        , instance.createTime);
                    return true;
                }
                else
                {
                    throw new Exception("无法从服务端获取信息，请检查网络与服务端！");
                }
                

            }
            catch (Exception ex)
            {

                throw new Exception("无法刷新 Token 请检查网络，错误信息：" + ex.Message);

            }
            
        }

    }
}
