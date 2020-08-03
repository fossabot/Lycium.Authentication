using Lycium.Authentication.Common;
using Lycium.Authentication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycium.Authentication
{
    public abstract class IServerHostGroupService
    {

        /// <summary>
        /// 根据组ID获取主机
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<string> GetHostsUrlByGroupId(long gid);

        /// <summary>
        /// 增加主机到群组
        /// </summary>
        /// <returns></returns>
        public abstract bool AddHostsToGroup(long gid, long cid);

        /// <summary>
        /// 将主机从组内删除
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public abstract bool DeletedHostFromGroup(long gid,long cid);


        /// <summary>
        /// 创建一个组
        /// </summary>
        /// <returns></returns>
        public abstract bool AddGroup(LyciumHostGroup group);


        /// <summary>
        /// 删除一个组
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public abstract bool DeletedGroup(long gid);


        /// <summary>
        /// 查询主机是否在组中
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        public abstract bool IsExistHostInGroup(long cid, long gid);

    }
}
