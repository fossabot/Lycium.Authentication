using Lycium.Authentication.Common;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lycium.Authentication.Server
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

        public HttpClient GetClient(string host)
        {

            var request = _factory.CreateClient(host);
            request.BaseAddress = new Uri(host);
            return request;
        }

        internal async Task<S> Post<T, S>(string host, string route, T obj)
        {

            try
            {
                var request = GetClient(host);
                StringContent content = new StringContent(JsonSerializer.Serialize(obj, LyciumConfiguration.JsonOption));
                content.Headers.ContentType.MediaType = "application/json";
                var result = await request.PostAsync(route, content);
                return JsonSerializer.Deserialize<S>(result.Content.ReadAsStringAsync().Result);

            }
            catch (Exception ex)
            {

                return default;

            }

        }


        internal async Task<S> Post<T, S>(LyciumHost host, string route, T obj)
        {

            try
            {
                var request = GetClient(host);
                StringContent content = new StringContent(JsonSerializer.Serialize(obj, LyciumConfiguration.JsonOption));
                content.Headers.ContentType.MediaType = "application/json";
                var result = await request.PostAsync(route, content);
                return JsonSerializer.Deserialize<S>(result.Content.ReadAsStringAsync().Result);

            }
            catch (Exception ex)
            {

                return default;

            }

        }

        internal async Task<T> Get<T>(string host, string route)
        {

            try
            {

                var request = GetClient(host);
                var result = request.GetStringAsync(route).Result;
                var instance = JsonSerializer.Deserialize<T>(result, LyciumConfiguration.JsonOption);
                return instance;

            }
            catch (Exception ex)
            {

                return default;

            }

        }
        internal async Task<T> Get<T>(LyciumHost host, string route)
        {

            try
            {

                var request = GetClient(host);
                var result = request.GetStringAsync(route).Result;
                var instance = JsonSerializer.Deserialize<T>(result, LyciumConfiguration.JsonOption);
                return instance;

            }
            catch (Exception ex)
            {

                return default;

            }

        }

    }
}
