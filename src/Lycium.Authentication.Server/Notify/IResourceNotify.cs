using Lycium.Authentication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lycium.Authentication.Server.Notify
{
    public abstract class IResourceNotify
    {
        public abstract HttpStatusCode WhitelistNotify(LyciumHost host, IEnumerable<string> resources);
        public abstract HttpStatusCode BacklistNotify(LyciumHost host, IEnumerable<string> resources);

    }
}
