using Lycium.Authentication.Common;
using Lycium.Authentication.Server.Notify;
using LyciumFreesqlServer.Request;
using System.Collections.Generic;
using System.Net;

namespace LyciumFreesqlServer.Services
{

    public class HttpResourceNotify : IResourceNotify
    {

        private readonly LyciumRequest _request;
        public HttpResourceNotify(LyciumRequest lyciumRequest)
        {
            _request = lyciumRequest;
        }
        public override HttpStatusCode BacklistNotify(LyciumHost host, IEnumerable<string> resources)
        {
            return _request.Post<IEnumerable<string>, HttpStatusCode>(host, "Resource/set/backlist", resources).Result;
        }

        public override HttpStatusCode WhitelistNotify(LyciumHost host, IEnumerable<string> resources)
        {
            return _request.Post<IEnumerable<string>, HttpStatusCode>(host, "Resource/set/whitelist", resources).Result;
        }

    }

}
