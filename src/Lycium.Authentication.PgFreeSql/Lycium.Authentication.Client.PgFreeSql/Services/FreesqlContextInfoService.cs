using Lycium.Authentication.Common;
using Microsoft.AspNetCore.Http;
using System;

namespace Lycium.Authentication.Client.Services
{
    public class FreesqlContextInfoService : IContextInfoService
    {
        public override void ClearTokenFromContext(HttpContext context, LyciumToken token)
        {
            context.Response.Headers[LyciumConfiguration.USER_TOKEN] = "";
        }

        public override long GetGidFromContext(HttpContext context)
        {
            return Convert.ToInt64(context.Request.Headers[LyciumConfiguration.USER_GID]);
        }

        public override string GetHostTokenFromContext(HttpContext context)
        {
            return context.Request.Headers[LyciumConfiguration.HOST_TOKEN];
        }

        public override string GetSecretKeyFromContext(HttpContext context)
        {
            return context.Request.Headers[LyciumConfiguration.SECRET_KEY];
        }

        public override string GetTokenFromContext(HttpContext context)
        {
            return context.Request.Headers[LyciumConfiguration.USER_TOKEN];
        }

        public override long GetUidFromContext(HttpContext context)
        {
            return Convert.ToInt64(context.Request.Headers[LyciumConfiguration.USER_UID]);
        }

        public override void PutTokenToContext(HttpContext context, LyciumToken token)
        {
            context.Response.Headers[LyciumConfiguration.USER_TOKEN] = token.Content;
        }
    }
}
