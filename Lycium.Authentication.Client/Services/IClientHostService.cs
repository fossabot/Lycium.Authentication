using Lycium.Authentication.Common;
using Lycium.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycium.Authentication
{
    public abstract class IClientHostService
    {


        /// <summary>
        /// 检测主机是否应该刷新Token
        /// </summary>
        /// <returns></returns>
        public virtual bool NeedRefresh()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000 - ClientConfiguration.HostTokenCreateTime >= ClientConfiguration.HostTokenAliveTime;
        }


        /// <summary>
        /// 向服务器申请新的Token
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public abstract bool RefreshToken();

    }
}
