using Lycium.Auth.DataAccess;
using Lycium.Auth.Service;
using Lycium.Authentication.Common;
using Lycium.Authentication.Controllers;
using Lycium.Authentication.Services;
using Lycium.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Lycium.Authentication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : PassController
    {
        private readonly LyciumNotifier _notifier;
        private readonly ITokenService _token;
        private readonly IHostService _host;
        private readonly IContextInfoService _info;
        public TokenController(
            LyciumNotifier notifier,
            IHostService host,
            ITokenService token,
            IContextInfoService info,
            IEntitySetter<LyciumToken> setter,
            IEntityGetter<LyciumToken> getter
            ) : base(setter, getter)
        {

            _info = info;
            _host = host;
            _notifier = notifier;
            _token = token;

        }

        [HttpGet("Refresh/{uid}/{token}")]
        public async Task<LyciumToken> RefreshToken(long uid, string token)
        {

            //校验客户端
            var host = _host.GetHost(HttpContext);
            if (_host.CheckHost(HttpContext, host))
            {

                var serverToken = _token.GetToken(uid);
                if (serverToken != default && _token.CheckToken(token, serverToken))
                {

                    //判断是否存活
                    if (token == serverToken.PreContent && serverToken.IsAlive)
                    {

                        //存活就返回
                        _notifier.SendToken(host, serverToken);
                        return serverToken;

                    }
                    else if (serverToken.CanFlush)
                    {

                        //允许刷新，刷新内容
                        _token.RefreshAndNotifyToken(serverToken);
                        return serverToken;

                    }
                    else
                    {
                        _token.LogoutBroadcast(serverToken);
                    }

                }

            }
            else
            {

                Response.StatusCode = 403;
                await Response.WriteAsync("检测到非法请求，请联系管理员！");

            }
            return null;
        }



        [HttpGet("Login/{uid}")]
        public async Task<LyciumToken> Login(long uid)
        {

            //客户端检测，如果通过则放行
            var host = _host.GetHost(HttpContext);
            if (host == null)
            {
                throw new System.Exception($"主机校验失败!主机名为{_info.GetHostName(HttpContext)}!");
            }
            if (_host.CheckHost(HttpContext, host))
            {

                //获取服务端当前Token
                var serverToken = _token.GetToken(uid);
                if (serverToken == default)
                {
                    //没有Token
                    var token = _token.CreateAndNotifyToken(uid);
                    return token;

                }
                else
                {

                    if (serverToken.IsAlive)
                    {

                        //存活就返回
                        _notifier.SendToken(host, serverToken);
                        return serverToken;

                    }
                    else if (serverToken.CanFlush)
                    {

                        //刷新内容并发送通知
                        _token.RefreshAndNotifyToken(serverToken);
                        return serverToken;

                    }
                    else
                    {

                        var token = _token.LogoutAndCreateToken(uid);
                        return token;
                    }


                }

            }
            else
            {
                Response.StatusCode = 403;
                await Response.WriteAsync("检测到非法客户端请求，请联系管理员！");
            }
            return null;

        }



        [HttpGet("Logout/{uid}/{token}")]
        public HttpStatusCode LogoutToken(long uid, string token)
        {

            var host = _host.GetHost(HttpContext);
            if (_host.CheckHost(HttpContext, host))
            {
                var serverToken = _token.GetToken(uid);
                if (serverToken == default)
                {
                    //没有Token
                    return HttpStatusCode.Redirect;

                }
                else if (_token.CheckToken(token, serverToken))
                {

                    _token.LogoutBroadcast(serverToken);
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.Unauthorized;
                }

            }
            else
            {

                return HttpStatusCode.NonAuthoritativeInformation;

            }
        }

    }
}
