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
        public abstract bool GetAndWriteAllowlist();


        /// <summary>
        /// 向本地追加白名单
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        public virtual void AddAllowlist(IEnumerable<string> resources)
        {
            ClientConfiguration.AddRouteAllowList(resources);
        }


        /// <summary>
        /// 向本地添加黑名单
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        public virtual void AddBlocklist(IEnumerable<string> resources)
        {
            ClientConfiguration.AddRouteAllowList(resources);
        }


        /// <summary>
        /// 检测资源是否在白名单中
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public virtual bool IsInAllowlist(string resource)
        {
            return ClientConfiguration.Allowlist.Contains(resource);
        }

       
        /// <summary>
        /// 获取当前系统白名单
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetAllowlist()
        {
            return ClientConfiguration.Allowlist;
        }
    }
}
