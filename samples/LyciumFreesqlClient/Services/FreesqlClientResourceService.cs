using Lycium.Authentication;
using Lycium.Authentication.Common;
using Lycium.Utils;
using LyciumFreesqlClient.Request;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LyciumFreesqlServer.Services
{
    public class FreesqlClientResourceService : IClientResourceService
    {

        private readonly LyciumRequest _request;
        public FreesqlClientResourceService(LyciumRequest request)
        {
            _request = request;
        }
        public override bool GetAndWriteWhitelist()
        {
            var list = _request.Get<IEnumerable<string>>("api/Resource/get/whitelist/secretKey");
            AddWritelist(list.ToArray());
            return true;
        }

        public override bool SyncResources()
        {
            var loaclResources = RouteScanHelper.RouteScan();
            var list = _request.Post<IEnumerable<string>, IEnumerable<string>>("api/Resource", loaclResources).Result;
            AddWritelist(list);
            return true;
        }
    }
}
