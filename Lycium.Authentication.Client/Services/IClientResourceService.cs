using Lycium.Configuration;
using System.Collections.Generic;

namespace Lycium.Authentication
{
    public abstract class IClientResourceService
    {

        /// <summary>
        /// 同步资源
        /// </summary>
        /// <returns></returns>
        public abstract bool SyncResources();


        /// <summary>
        /// 从服务端获取并写入白名单
        /// </summary>
        /// <returns></returns>
        public abstract bool GetAndWriteWhitelist();


        /// <summary>
        /// 向本地追加白名单
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        public virtual void AddWritelist(IEnumerable<string> resources)
        {
            ClientConfiguration.AddRouteWhiteList(resources);
        }


        /// <summary>
        /// 向本地添加黑名单
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        public virtual void AddBacklist(IEnumerable<string> resources)
        {
            ClientConfiguration.AddRouteWhiteList(resources);
        }


        /// <summary>
        /// 检测资源是否在白名单中
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public virtual bool IsInWhtelist(string resource)
        {
            return ClientConfiguration.Whitelist.Contains(resource);
        }

       
        /// <summary>
        /// 获取当前系统白名单
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetWhitelist()
        {
            return ClientConfiguration.Whitelist;
        }
    }
}
