using Lycium.Authentication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycium.Authentication.Server.Notify
{
    public abstract class ITokenNotify
    {
        public abstract void BroadcastLogout(IEnumerable<LyciumHost> hosts, LyciumToken token);
        public abstract void BroadcastLogin(IEnumerable<LyciumHost> hosts, LyciumToken token);
        public abstract void BroadcastRefreshToken(IEnumerable<LyciumHost> hosts, LyciumToken token);
    }
}
