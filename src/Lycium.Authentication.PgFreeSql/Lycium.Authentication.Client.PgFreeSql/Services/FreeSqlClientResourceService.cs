using Lycium.Authentication.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Lycium.Authentication.Client.Services
{
    public class FreeSqlClientResourceService : IClientResourceService
    {

        private readonly LyciumRequest _request;
        public FreeSqlClientResourceService(LyciumRequest request)
        {
            _request = request;
        }
        public override bool GetAndWriteAllowlist()
        {
            var list = _request.Get<IEnumerable<string>>("api/Resource/get/allowlist/secretKey");
            AddAllowlist(list.ToArray());
            return true;
        }

        public override bool SyncResources()
        {
            try
            {
                var loaclResources = RouteScanHelper.RouteScan();
                var list = _request.Post<IEnumerable<string>, IEnumerable<string>>("api/Resource", loaclResources).Result;
                AddAllowlist(list);
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
            
        }
    }
}
