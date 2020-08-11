using Lycium.Authentication;
using Lycium.Authentication.Common;
using Lycium.Authentication.Server;
using LyciumFreesqlServer.Request;
using System.Net;
using System.Threading.Tasks;

namespace LyciumFreesqlServer.Services
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

            var hosts = _host.GetHostsUrlByGroupId(gid);
            foreach (var item in hosts)
            {
                var result = await _request.Post<LyciumToken, HttpStatusCode>(item, "LyciumToken/add", token);
                if (result == HttpStatusCode.InternalServerError)
                {
                    await _request.Post<LyciumToken, HttpStatusCode>(item, "LyciumToken/add", token);
                }
            }
            
        }


        public override async Task NotifyTokensClear(long gid, long uid)
        {

            var hosts = _host.GetHostsUrlByGroupId(gid);
            foreach (var item in hosts)
            {
                var result = await _request.Get<HttpStatusCode>(item, $"LyciumToken/remove/{uid}/{gid}");
                if (result == HttpStatusCode.InternalServerError)
                {
                    await _request.Get<HttpStatusCode>(item, $"LyciumToken/remove/{uid}/{gid}");
                }
            }

        }


        public override async Task NotifyTokensModify(long gid, LyciumToken token)
        {
            var hosts = _host.GetHostsUrlByGroupId(gid);
            foreach (var item in hosts)
            {
                var result = await _request.Post<LyciumToken, HttpStatusCode>(item, "LyciumToken/modify", token);
                if (result == HttpStatusCode.InternalServerError)
                {
                    await _request.Post<LyciumToken, HttpStatusCode>(item, "LyciumToken/modify", token);
                }
            }
        }

    }
}
