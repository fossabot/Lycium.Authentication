using Lycium.Authentication.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace Lycium.Authentication
{
    public abstract class IServerHostService
    {

        private readonly IContextInfoService _infoService;
        public IServerHostService(IContextInfoService infoService)
        {
            _infoService = infoService;
        }


        /// <summary>
        /// 根据密钥获取主机实体
        /// </summary>
        /// <param name="hostName">主机名（唯一）</param>
        /// <returns></returns>
        public abstract LyciumHost GetHostBySecretKey(string secretKey);
        public abstract LyciumHost GetHostById(long id);


        /// <summary>
        /// 从上下文获取SecretKey并判断主机是否合法
        /// </summary>
        /// <param name="context">http上下文</param>
        /// <returns></returns>
        public virtual LyciumHost RequestCheck(HttpContext context)
        {

            var secretKey = _infoService.GetSecretKeyFromContext(context);
            if (secretKey != null)
            {
                var host = GetHostBySecretKey(secretKey);
                if (host != null)
                {
                    return host;
                }
            }
            return null;

        }



        /// <summary>
        /// 检查主机是否合法，并匹配Token, 匹配返回主机信息，不匹配返回空
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual LyciumHost OperationCheck(HttpContext context)
        {
            var host = RequestCheck(context);
            if (host != null)
            {
                var token = _infoService.GetHostTokenFromContext(context);
                if (token == host.HostToken)
                {
                    return host;
                }
            }
            return null;
        }



        /// <summary>
        /// 获取所有主机
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<LyciumHost> Query(int page, int size);


        /// <summary>
        /// 模糊查询获取所有主机
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<LyciumHost> KeywordQuery(int page, int size, string keyword);


        /// <summary>
        /// 获取所有实体数量
        /// </summary>
        /// <returns></returns>
        public abstract long Count();


        /// <summary>
        /// 增加一个主机
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public abstract bool AddHost(LyciumHost host);


        /// <summary>
        /// 根据主机名模糊查询主机列表
        /// </summary>
        /// <returns></returns>
        public abstract LyciumHost NameQuery(string hostName);


        /// <summary>
        /// 更新一个主机
        /// </summary>
        /// <param name="lyciumHost"></param>
        /// <returns></returns>
        public abstract bool UpdateHost(LyciumHost lyciumHost);
    }
}
