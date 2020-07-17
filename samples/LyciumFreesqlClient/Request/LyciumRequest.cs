using Lycium.Authentication;
using Lycium.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LyciumFreesqlClient.Request
{
    public class LyciumRequest
    {

        private readonly static Uri ServerAddress;
        static LyciumRequest()
        {
            ServerAddress = new Uri(ClientConfiguration.ServerUrl);
        }


        private readonly IHttpClientFactory _factory;
        private readonly IClientHostService _hostServer;
        public LyciumRequest(IHttpClientFactory factory, IClientHostService hostService)
        {
            _factory = factory;
            _hostServer = hostService;
        }


        public HttpClient GetClient()
        {

            if (_hostServer.NeedRefresh())
            {
                _hostServer.RefreshToken();
            }
            var request = _factory.CreateClient(ClientConfiguration.SecretKey);
            request.BaseAddress = ServerAddress;
            request.DefaultRequestHeaders.Add(LyciumConfiguration.SECRET_KEY, ClientConfiguration.SecretKey);
            request.DefaultRequestHeaders.Add(LyciumConfiguration.HOST_TOKEN, ClientConfiguration.HostToken);
            return request;
        }


        internal async Task<S> Post<T, S>(string route, T obj)
        {

            try
            {
                var request = GetClient();
                StringContent content = new StringContent(JsonSerializer.Serialize(obj));
                content.Headers.ContentType.MediaType = "application/json";
                var result = await request.PostAsync(route, content);
                return JsonSerializer.Deserialize<S>(result.Content.ReadAsStringAsync().Result);

            }
            catch (Exception ex)
            {

                return default;

            }

        }
        internal T Get<T>(string route)
        {

            try
            {

                var request = GetClient();
                var result = request.GetStringAsync(route).Result;
                var instance = JsonSerializer.Deserialize<T>(result);
                return instance;

            }
            catch (Exception ex)
            {

                return default;

            }

        }

    }
}
