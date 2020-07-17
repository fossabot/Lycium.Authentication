﻿using Lycium.Authentication.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Transactions;

namespace Lycium.Configuration
{

    public static class ClientConfiguration
    {

        internal static bool HasSync;
        public static string ServerUrl;
        public static string SecretKey;
        public static string HostToken;
        internal static long HostTokenAliveTime;
        internal static long HostTokenCreateTime;
        internal static ImmutableHashSet<string> Whitelist;


        internal static IApplicationBuilder AppBuilder;
        private static ICollection<EndpointDataSource> _dataSource;

        static ClientConfiguration()
        {

            Whitelist = ImmutableHashSet.Create("test");

        }

        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <returns></returns>
        public static bool CheckHeartbeat()
        {

            if (ServerUrl == default)
            {
                throw new Exception("服务器地址为空，请在 AddLyciumClient 中配置选项 SetServerUrl!");
            }
            try
            {

                using (HttpClient client = new HttpClient())
                {

                    client.BaseAddress = new Uri(ServerUrl);
                    var result = client.GetAsync("api/Heartbeat").Result;
                    if (result.StatusCode != HttpStatusCode.OK)
                    {

                        throw new Exception("未与服务器建立良好链接, 请检查网络通路状态并于管理员联系!");

                    }
                    else
                    {

                        HasSync = true;

                    }
                    return HasSync;

                }

            }
            catch (Exception)
            {

                return HasSync;

            }
            
            
        }
        internal static void SetAppBuilderCache(IApplicationBuilder builder)
        {
            AppBuilder = builder;
        }
        internal static ICollection<EndpointDataSource> GetDataSources()
        {
            if (_dataSource == default)
            {

                var props = AppBuilder.Properties;
                if (props.ContainsKey("__EndpointRouteBuilder"))
                {
                    _dataSource = ((IEndpointRouteBuilder)props["__EndpointRouteBuilder"]).DataSources;
                }

            }
            return _dataSource;
        }

        #region 白名单和黑名单设置
        internal static void AddRouteWhiteList(IEnumerable<string> list)
        {

            Whitelist = Whitelist.Union(list);

        }
        internal static void AddRouteBackList(IEnumerable<string> list)
        {

            Whitelist = Whitelist.Except(list);

        }
        #endregion


        public static void SetTokenInfo(string token, long alivaTime ,long createTime)
        {
            HostToken = token;
            HostTokenAliveTime = alivaTime;
            HostTokenCreateTime = createTime;
        }

    }


}