using Lycium.Authentication.Common;
using System.Collections.Generic;
using System.Net;

namespace Lycium.Authentication.Server
{
    public abstract class IResourceNotify
    {
        public abstract HttpStatusCode NotifyAllowlist(LyciumHost host, IEnumerable<string> resources);
        public abstract HttpStatusCode NotifyBlocklist(LyciumHost host, IEnumerable<string> resources);

    }
}
