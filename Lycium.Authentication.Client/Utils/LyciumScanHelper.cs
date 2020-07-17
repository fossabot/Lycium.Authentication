using Lycium.Configuration;
using Lycium.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace Lycium.Authentication.Utils
{
    public static class LyciumScanHelper
    {

        private static int _index;

        /// <summary>
        /// 是否为Lycium路由节点
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsLyciumApi(Endpoint point)
        {

            if (point.Metadata.Count > _index + 1 && point.Metadata[_index].GetType().BaseType == typeof(LyciumApiAttribute))
            {
                return true;
            }
            else
            {
                for (int i = 0; i < point.Metadata.Count; i += 1)
                {

                    if (point.Metadata[i].GetType() == typeof(LyciumApiAttribute))
                    {
                        _index = i;
                        return true;
                    }
                }
            }

            return false;

        }

        /// <summary>
        /// 扫描 Lycium 本身的路由
        /// </summary>
        public static void AddLyciumRoute()
        {

            var dataSource = ClientConfiguration.GetDataSources();
            if (dataSource != default)
            {

                var list = new List<string>();
                foreach (var item in dataSource)
                {

                    foreach (RouteEndpoint endpoint in item.Endpoints)
                    {

                        if (IsLyciumApi(endpoint))
                        {

                            list.Add($"{RouteScanHelper.GetMethod(endpoint)}:{endpoint.RoutePattern.RawText}");

                        }

                    }

                }
                ClientConfiguration.AddRouteWhiteList(list.ToArray());

            }
        }
    }
}
