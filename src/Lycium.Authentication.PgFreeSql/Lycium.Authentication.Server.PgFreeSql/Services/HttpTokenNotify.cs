using Lycium.Authentication.Common;
using System.Net;
using System.Threading.Tasks;

namespace Lycium.Authentication.Server
{
    public class HttpTokenNotify : ITokenNotify
    {

        private readonly IServerHostGroupService _host;
        private readonly LyciumRequest _request;
        public HttpTokenNotify(LyciumRequest lyciumRequest, IServerHostGroupService hostService)
        {
            _request = lyciumRequest;
            _host = hostService;
        }


        public override async Task NotifyTokensAdd(long gid, LyciumToken token)
        {

            var hosts = _host.GetHostsUrlFromGroupId(gid);
            foreach (var item in hosts)
            {
                var result = await _request.Post<LyciumToken, HttpStatusCode>(item, "api/LyciumToken/add", token);
                if (result == HttpStatusCode.InternalServerError)
                {
                    await _request.Post<LyciumToken, HttpStatusCode>(item, "api/LyciumToken/add", token);
                }
            }
            
        }


        public override async Task NotifyTokensClear(long gid, long uid)
        {

            var hosts = _host.GetHostsUrlFromGroupId(gid);
            foreach (var item in hosts)
            {
                var result = await _request.Get<HttpStatusCode>(item, $"api/LyciumToken/remove/{uid}/{gid}");
                if (result == HttpStatusCode.InternalServerError)
                {
                    await _request.Get<HttpStatusCode>(item, $"api/LyciumToken/remove/{uid}/{gid}");
                }
            }

        }


        public override async Task NotifyTokensModify(long gid, LyciumToken token)
        {
            var hosts = _host.GetHostsUrlFromGroupId(gid);
            foreach (var item in hosts)
            {
                var result = await _request.Post<LyciumToken, HttpStatusCode>(item, "api/LyciumToken/modify", token);
                if (result == HttpStatusCode.InternalServerError)
                {
                    await _request.Post<LyciumToken, HttpStatusCode>(item, "api/LyciumToken/modify", token);
                }
            }
        }

    }
}
