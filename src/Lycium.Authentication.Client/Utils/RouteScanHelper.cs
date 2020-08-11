using Lycium.Authentication.Common;
using Lycium.Authentication.Utils;
using Lycium.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;

namespace Lycium.Authentication.Utils
{
    public static class RouteScanHelper
    {
        private static int _index;
        public static string GetMethod(Endpoint point)
        {

            string method = default;
            if (point.Metadata.Count > _index + 1 && point.Metadata[_index].GetType().BaseType == typeof(HttpMethodAttribute))
            {
                method = ((HttpMethodAttribute)point.Metadata[_index]).HttpMethods.FirstOrDefault();
            }
            else
            {
                for (int i = 0; i < point.Metadata.Count; i += 1)
                {

                    if (point.Metadata[i].GetType().BaseType == typeof(HttpMethodAttribute))
                    {
                        _index = i;
                        method = ((HttpMethodAttribute)point.Metadata[i]).HttpMethods.FirstOrDefault();
                        break;
                    }
                }
            }

            return method;

        }


        public static List<string> RouteScan()
        {

            List<string> RouteCache = new List<string>();
            var dataSource = ClientConfiguration.GetDataSources();
            if (dataSource != default)
            {

                foreach (var item in dataSource)
                {

                    foreach (RouteEndpoint endpoint in item.Endpoints)
                    {

                        if (!LyciumScanHelper.IsLyciumApi(endpoint))
                        {
                            RouteCache.Add($"{GetMethod(endpoint)}:{endpoint.RoutePattern.RawText}");
                        }
                       
                    }

                }
            }
            return RouteCache;

        }
    }

}
