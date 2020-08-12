using Lycium.Authentication.Common;
using System.Collections.Generic;

namespace Lycium.Authentication
{
    public abstract class IServerResourceService
    {
        public abstract bool AddResources(LyciumHost host, params string[] resources);
        public abstract bool ModifyResourceAsAllowlist(params long[] resources);
        public abstract bool ModifyResourceAsBlocklist(params long[] resources);
        public abstract IEnumerable<string> GetAllowlist(params long[] resources);
        public abstract IEnumerable<string> GetAllAllowlist(long hostId);
        public abstract IEnumerable<LyciumResource> QueryAll(long hostId);
        public abstract IEnumerable<LyciumResource> Query(long hostId, int page,int size);
    }
}
