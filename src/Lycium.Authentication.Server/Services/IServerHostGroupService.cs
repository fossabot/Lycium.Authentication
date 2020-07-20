using Lycium.Authentication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycium.Authentication.Server.Services
{
    public abstract class IServerHostGroupService
    {
        /// <summary>
        /// 根据组ID获取主机
        /// </summary>
        /// <returns></returns>
        public abstract LyciumHost GetHostsByGroupId();
        /// <summary>
        /// 增加主机到群组
        /// </summary>
        /// <returns></returns>
        public abstract LyciumHost AddHostsToGroup();

        public abstract bool DeletedHostFromGroup(long gid,long cid);


        public abstract LyciumHost AddGroup();

        public abstract bool DeletedGroup();

    }
}
