using Lycium.Authentication;
using Lycium.Authentication.Common;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LyciumFreesqlServer.Request
{
    public class LyciumRequest
    {

        private readonly IHttpClientFactory _factory;
        public LyciumRequest(IHttpClientFactory factory)
        {
            _factory = factory;
        }


        public HttpClient GetClient(LyciumHost host)
        {

            var request = _factory.CreateClient(host.SecretKey);
            request.BaseAddress = new Uri(host.HostUrl);
            return request;
        }


        internal async Task<S> Post<T, S>(LyciumHost host, string route, T obj)
        {

            try
            {
                var request = GetClient(host);
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
        internal T Get<T>(LyciumHost host, string route)
        {

            try
            {

                var request = GetClient(host);
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
