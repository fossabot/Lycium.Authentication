using Lycium.Authentication.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Lycium.Authentication
{
    public abstract class IContextInfoService
    {
        /// <summary>
        /// 将Token推到HttpContext中
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="token"></param>
        public abstract void PutTokenToContext(HttpContext context, LyciumToken token);


        /// <summary>
        /// 移除Token
        /// </summary>
        /// <param name="context"></param>
        /// <param name="token"></param>
        public abstract void ClearTokenFromContext(HttpContext context, LyciumToken token);


        /// <summary>
        /// 从HttpContext上下文中获取Token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract string GetTokenFromContext(HttpContext context);


        /// <summary>
        /// 从HttpContext中获取Uid
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract long GetUidFromContext(HttpContext context);


        /// <summary>
        /// 从HttpContext中获取主机名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract string GetSecretKeyFromContext(HttpContext context);


        /// <summary>
        /// 从HttpContext中获取主机Key
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract string GetHostTokenFromContext(HttpContext context);
    }
}
