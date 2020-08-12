using Lycium.Authentication.Common;
using System.Collections.Generic;
using System.Net;

namespace Lycium.Authentication.Server
{

    public class HttpResourceNotify : IResourceNotify
    {

        private readonly LyciumRequest _request;
        public HttpResourceNotify(LyciumRequest lyciumRequest)
        {
            _request = lyciumRequest;
        }
        public override HttpStatusCode NotifyBlocklist(LyciumHost host, IEnumerable<string> resources)
        {
            return _request.Post<IEnumerable<string>, HttpStatusCode>(host, "api/Resource/set/blocklist", resources).Result;
        }

        public override HttpStatusCode NotifyAllowlist(LyciumHost host, IEnumerable<string> resources)
        {
            return _request.Post<IEnumerable<string>, HttpStatusCode>(host, "api/Resource/set/allowlist", resources).Result;
        }

    }

}
