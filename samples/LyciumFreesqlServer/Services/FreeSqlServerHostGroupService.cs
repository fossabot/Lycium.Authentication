using Lycium.Authentication;
using Lycium.Authentication.Common;
using Lycium.Authentication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyciumFreesqlServer.Services
{
    public class FreeSqlServerHostGroupService : IServerHostGroupService
    {

        private static Action<IFreeSql> InitAction;
        static FreeSqlServerHostGroupService()
        {
            InitAction = (freeSql) =>
            {
                freeSql.CodeFirst.ConfigEntity<LyciumHostGroup>
                (
                       item => item.Property(item => item.Id).IsIdentity(true)
                );
                InitAction = null;
            };
        }


        private readonly IFreeSql _freeSql;
        public FreeSqlServerHostGroupService(IFreeSql freesql)
        {
            _freeSql = freesql;
            InitAction?.Invoke(freesql);
        }


        /// <summary>
        /// 添加组
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public override bool AddGroup(LyciumHostGroup group)
        {
            return _freeSql.Insert(group).ExecuteAffrows() == 1;
        }


        /// <summary>
        /// 添加主机到组
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public override bool AddHostsToGroup(long gid, long cid)
        {
            LyciumHostRelation relation = new LyciumHostRelation();
            relation.Gid = gid;
            relation.Cid = cid;
            return _freeSql.Insert(relation).ExecuteAffrows() == 1;
        }


        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public override bool DeletedGroup(long gid)
        {
            return _freeSql.Delete<LyciumHostGroup>().Where(item => item.Id == gid).ExecuteAffrows() == 1;
        }


        /// <summary>
        /// 删除对应组的主机
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public override bool DeletedHostFromGroup(long gid, long cid)
        {
            return _freeSql.Delete<LyciumHostRelation>().Where(item => item.Gid == gid && item.Cid == cid).ExecuteAffrows() == 1;
        }


        /// <summary>
        /// 获取改组下所有主机URL
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public override IEnumerable<string> GetHostsUrlByGroupId(long gid)
        {
            return _freeSql
                .Select<LyciumHost,LyciumHostRelation>()
                .InnerJoin((host,relation) => host.Id == relation.Cid)
                .Where((host, relation) => relation.Gid == gid)
                .ToList((host,relation) =>host.HostUrl);
        }
    }
}
