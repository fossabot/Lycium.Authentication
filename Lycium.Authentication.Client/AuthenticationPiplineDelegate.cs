using Lycium.Authentication;
using Lycium.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{

    public class AuthenticationPiplineDelegate : IAuthenticationDelegate
    {
        //private readonly IContextInfoService _info;
        private readonly IClientResourceService _resourceService;
        private readonly IClientHostService _hostService;

        public AuthenticationPiplineDelegate(
            //IContextInfoService info,
            IClientResourceService resource,
            IClientHostService host
            )
        {
            //_info = info;
            _resourceService = resource;
            _hostService = host;
        }




        public async Task PipelineDelegate(HttpContext context, Func<Task> next)
        {

            //获取路由资源
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                await next.Invoke();
            }


            var routeEndpoint = (RouteEndpoint)endpoint;
            if (routeEndpoint != null)
            {

                //拿到资源
                var resource = $"{context.Request.Method}:{routeEndpoint.RoutePattern.RawText}";


                //检测路由是否在白名单中，在的话放行
                if (!_resourceService.IsInWhtelist(resource))  
                {

                    //是否与服务器已经通步过
                    if (!ClientConfiguration.HasSync)
                    {

                        //心跳检测
                        if (ClientConfiguration.CheckHeartbeat())
                        {

                            //刷新HostToken
                            if (!_hostService.RefreshToken())
                            {

                                //Token刷新不通过
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                await context.Response.WriteAsync(
                                    JsonSerializer.Serialize(
                                        new { code = 5001, msg = $"服务端刷新Token失败，请核对主机与服务端信息！" })
                                    );

                            }
                            else
                            {

                                //同步服务器白名单
                                if (!_resourceService.SyncResources())
                                {

                                    //同步白名单不通过
                                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    await context.Response.WriteAsync(
                                        JsonSerializer.Serialize(
                                            new { code = 5002, msg = $"服务端获取白名单资源失败，请核对主机与服务端信息！" })
                                        );

                                }

                            }

                        }
                        else
                        {

                            //心跳检测不通过
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            await context.Response.WriteAsync(
                                JsonSerializer.Serialize(
                                    new { code = 5000, msg = "心跳检测失败， 无法联通服务端，请检测网络和服务的！" })
                                );

                        }
                        

                    }

                    #region Token认证
                    //从上下文中获取Token
                    //var tokenString = _info.GetToken(context);
                    //if (tokenString == null || tokenString == "")
                    //{

                    //    //没有Token返回登录
                    //    context.Response.StatusCode = 401;
                    //    await context.Response.WriteAsync(
                    //        JsonSerializer.Serialize(
                    //            new { code = 4011, msg = "未从客户端获取到Token，Token认证失败！" })
                    //        );


                    //}

                    ////获取服务端Token
                    //var serverToken = _token.GetToken(context);
                    //if (serverToken != null)
                    //{

                    //    //Token本地认证
                    //    if (tokenString == serverToken.PreContent || tokenString == serverToken.Content)
                    //    {

                    //        //本地Token如果没有存活
                    //        if (!serverToken.IsAlive)
                    //        {

                    //            //Token在刷新期
                    //            if (serverToken.CanFlush)
                    //            {

                    //                //向服务器请求并刷新Token
                    //                var newToken = _notifier.RefreshToken(serverToken.Uid, serverToken.Content);
                    //                if (newToken != null)
                    //                {

                    //                    //将Token设置到上下文中
                    //                    _info.PutTokenToContext(context, newToken);

                    //                }
                    //                else
                    //                {
                    //                    //没有请求到Token返回登录
                    //                    context.Response.StatusCode = 401;
                    //                    await context.Response.WriteAsync("Token 刷新失败，认证失败！");
                    //                }

                    //            }
                    //            else
                    //            {

                    //                //没申请到
                    //                context.Response.StatusCode = 401;
                    //                await context.Response.WriteAsync("Token已经过期，请重新登陆！");

                    //            }

                    //        }
                    //        else if (tokenString == serverToken.PreContent)
                    //        {

                    //            //本地存活，且版本很新
                    //            _info.PutTokenToContext(context, serverToken);

                    //        }


                    //        if (ClientConfiguration.NeedAuth)
                    //        {

                    //            var code = _notifier.AuthAllow(serverToken.Uid, resource);
                    //            if (code == System.Net.HttpStatusCode.OK)
                    //            {

                    //                await next.Invoke();

                    //            }
                    //            else if (code == System.Net.HttpStatusCode.Forbidden)
                    //            {

                    //                context.Response.StatusCode = 403;
                    //                await context.Response.WriteAsync("您没有权限访问此资源，请联系管理员！");

                    //            }
                    //            else
                    //            {

                    //                context.Response.StatusCode = (int)code;
                    //                await context.Response.WriteAsync("程序异常，请联系管理员！");

                    //            }

                    //        }
                    //        else
                    //        {
                    //            await next.Invoke();
                    //        }


                    //    }
                    //    else
                    //    {

                    //        //请求远程刷新
                    //        var newToken = _notifier.RefreshToken(serverToken.Uid, tokenString);
                    //        if (newToken != null)
                    //        {

                    //            _info.PutTokenToContext(context, newToken);

                    //        }
                    //        else
                    //        {
                    //            context.Response.StatusCode = 401;
                    //            await context.Response.WriteAsync(
                    //                JsonSerializer.Serialize(
                    //                    new { code = 4012, msg = "Token远程认证失败！" })
                    //                );
                    //        }


                    //        if (ClientConfiguration.NeedAuth)
                    //        {

                    //            var code = _notifier.AuthAllow(serverToken.Uid, resource);
                    //            if (code == System.Net.HttpStatusCode.OK)
                    //            {

                    //                await next.Invoke();

                    //            }
                    //            else if (code == System.Net.HttpStatusCode.Forbidden)
                    //            {

                    //                context.Response.StatusCode = 403;
                    //                await context.Response.WriteAsync("您没有权限访问此资源，请联系管理员！");

                    //            }
                    //            else
                    //            {

                    //                context.Response.StatusCode = (int)code;
                    //                await context.Response.WriteAsync(
                    //                    JsonSerializer.Serialize(
                    //                        new { code = 5000, msg = "程序异常，请联系管理员！" }
                    //                        )
                    //                    );

                    //            }

                    //        }
                    //        else
                    //        {
                    //            await next.Invoke();
                    //        }

                    //    }

                    //}
                    //else
                    //{

                    //    context.Response.StatusCode = 401;
                    //    await context.Response.WriteAsync(
                    //        JsonSerializer.Serialize(
                    //            new { code = 4013, msg = "未从服务端获取到Token, 请检查UID和数据库，Token认证失败！" })
                    //        );

                    //}
                    #endregion


                }
                else
                {
                    //白名单放行
                    await next.Invoke();
                }

            }
            else
            {
                //空资源/静态资源放行
                await next.Invoke();
            }


        }


    }
}
