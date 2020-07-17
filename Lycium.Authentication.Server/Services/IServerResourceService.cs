using Lycium.Authentication.Common;
using System.Collections.Generic;

namespace Lycium.Authentication
{
    public abstract class IServerResourceService
    {
        public abstract bool AddResources(LyciumHost host, params string[] resources);
        public abstract bool SetResourceAsWhitelist(params LyciumResource[] resources);
        public abstract bool SetResourceAsWhitelist(params long[] resources);
        public abstract bool SetResourceAsBlacklist(params LyciumResource[] resources);
        public abstract bool SetResourceAsBlacklist(params long[] resources);
        public abstract IEnumerable<string> GetAllWhitelist(long hostId);
        public abstract IEnumerable<LyciumResource> QueryAll(long hostId);
        public abstract IEnumerable<LyciumResource> Query(long hostId, int page,int size);
    }
}
