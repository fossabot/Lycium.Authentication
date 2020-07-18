using Lycium.Authentication;
using Lycium.Authentication.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LyciumFreesqlServer.Services
{
    public class FreeSqlServerResourceService : IServerResourceService
    {


        private static Action<IFreeSql> InitAction;
        static FreeSqlServerResourceService()
        {
            InitAction = (freeSql) =>
            {
                freeSql.CodeFirst.ConfigEntity<LyciumResource>
                (
                     item => item.Property(item => item.Id).IsIdentity(true)
                );
                InitAction = null;
            };
        }
        private readonly IFreeSql _freesql;
        public FreeSqlServerResourceService(IFreeSql freesql)
        {
            _freesql = freesql;
            InitAction?.Invoke(freesql);
        }


        /// <summary>
        /// 添加本地没有的资源
        /// </summary>
        /// <param name="host"></param>
        /// <param name="resources"></param>
        /// <returns></returns>
        public override bool AddResources(LyciumHost host, params string[] resources)
        {
            var insertList = new List<LyciumResource>();
            var res = QueryAll(host.Id);
            if (res != null)
            {
                var hashSets = res.Select(item => item.Resource).ToImmutableHashSet();
                
                for (int i = 0; i < resources.Length; i++)
                {
                    if (!hashSets.Contains(resources[i]))
                    {
                        insertList.Add(new LyciumResource() { Cid = host.Id, Resource = resources[i] });
                    }
                }
                return _freesql.Insert(insertList).ExecuteAffrows() == insertList.Count;
            }
            else
            {

                for (int i = 0; i < resources.Length; i++)
                {
                    insertList.Add(new LyciumResource() {  Cid = host.Id, Resource = resources[i] });
                }
                return _freesql.Insert(resources).ExecuteAffrows() == resources.Length;
            }

        }


        /// <summary>
        /// 获取该主机所有的白名单
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public override IEnumerable<string> GetAllWhitelist(long hostId)
        {
            return _freesql.Select<LyciumResource>().Where(item => item.Cid == hostId && item.InWhiteList == true).ToList(item => item.Resource);
        }



        public override IEnumerable<LyciumResource> Query(long hostId, int page, int size)
        {
            return _freesql.Select<LyciumResource>().Where(item=>item.Cid == hostId).Page(page, size).ToList();
        }

        public override IEnumerable<LyciumResource> QueryAll(long hostId)
        {
            return _freesql.Select<LyciumResource>().Where(item => item.Cid == hostId).ToList();
        }


        public override bool SetResourceAsBlacklist(params long[] resources)
        {
            return SetResourceStatus(false, resources);
        }


        public override bool SetResourceAsWhitelist(params long[] resources)
        {
            return SetResourceStatus(true, resources);
        }





        /// <summary>
        /// 批量设置资源状态
        /// </summary>
        /// <param name="host"></param>
        /// <param name="status"></param>
        /// <param name="resources"></param>
        /// <returns></returns>
        private bool SetResourceStatus(bool status, params long[] resources)
        {
           return _freesql
                .Update<LyciumResource>()
                .Set(item => item.InWhiteList == status)
                .Where(item => resources.Contains(item.Id))
                .ExecuteAffrows() == resources.Length;
        }


        public override IEnumerable<string> GetWhitelist(params long[] resources)
        {
            return _freesql.Select<LyciumResource>().Where(item => resources.Contains(item.Id)).ToList(item => item.Resource);
        }
    }
}
