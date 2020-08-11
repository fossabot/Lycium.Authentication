using Lycium.Authentication.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycium.Authentication
{
    public abstract class IServerConfigurationService
    {

        /// <summary>
        /// 增加一个配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public abstract bool AddConfig(LyciumConfig config);

        /// <summary>
        /// 根据名称获取配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract LyciumConfig GetConfigByName(string name);

        /// <summary>
        /// 根据ID获取配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract LyciumConfig GetConfigById(long id);

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public abstract bool UpdateConfig(LyciumConfig config);

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public abstract IEnumerable<LyciumConfig> Query(int page,int size);

        /// <summary>
        /// 根据关键字查询配置列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public abstract IEnumerable<LyciumConfig> KeywordsQuery(int page, int size, string keyword);

        /// <summary>
        /// 获取所有实体数量
        /// </summary>
        /// <returns></returns>
        public abstract long Count();
    }
}
