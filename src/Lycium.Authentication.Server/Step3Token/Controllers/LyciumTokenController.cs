using Lycium.Authentication.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Lycium.Authentication.Server
{

    [Route("api/[controller]")]
    [ApiController]
    public class LyciumTokenController : BaseInfoController
    {

        private readonly IServerTokenService _tokenService;
        private readonly ITokenNotify _tokenNotify;
        public LyciumTokenController(
            IContextInfoService infoService,
            IServerHostService hostService,
            IServerTokenService tokenService,
            ITokenNotify tokenNotify
            ) :base(infoService, hostService)
        {
            _tokenService = tokenService;
            _tokenNotify = tokenNotify;
        }


        /// <summary>
        /// 申请新 Token
        /// </summary>
        /// <param name="uid"> 用户ID </param>
        /// <param name="gid"> APP组ID </param>
        /// <returns></returns>
        [HttpGet("LoginToken")]
        public async Task<string> LoginToken()
        {

            if (IsLegal)
            {

                bool needAdd = false;
                //已经存在token
                var token = _tokenService.GetToken(Uid, Gid);
                if (token == null)
                {
                    needAdd = true;
                    token = new LyciumToken();
                    token.Uid = Uid;
                    token.Gid = Gid;
                }

                //重置 Token 内容
                TokenOperator.Flush(token);
                if (needAdd)
                {
                    //如果数据库没有则新增
                    if (_tokenService.AddToken(token))
                    {
                        //如果新增成功则发送通知
                        await _tokenNotify.NotifyTokensAdd(Gid,token);
                        return JsonResult(token);
                    }
                    else
                    {

                        HttpContext.Response.StatusCode = 500;
                        LyciumConfiguration.ReturnMessage(HttpContext, 4017, "无法创建服务端Token，请联系管理员！");
                        
                    }
                }
                else
                {
                    //如果数据库有则更新
                    if (_tokenService.ModifyToken(token))
                    {
                        //如果修改成功则发送通知
                        await _tokenNotify.NotifyTokensModify(Gid, token);
                        return JsonResult(token);
                    }
                    else
                    {
                        HttpContext.Response.StatusCode = 401;
                        LyciumConfiguration.ReturnMessage(HttpContext, 4018, "无法更新服务端Token，请联系管理员！");
                    }
                }

            }
            else
            {
                HttpContext.Response.StatusCode = 401;
                LyciumConfiguration.ReturnMessage(HttpContext, 4016, "主机校验不合法！");

            }
            
            return null;
        }




        /// <summary>
        /// 注销 Token
        /// </summary>
        /// <param name="uid"> 用户ID </param>
        /// <param name="gid"> APP组ID </param>
        /// <returns></returns>
        [HttpGet("LogoutToken")]
        public async Task<HttpStatusCode> LogoutToken()
        {

            if (IsLegal)
            {
                //获取要被注销的token
                var token = _tokenService.GetToken(Uid, Gid);
                if (token != null)
                {
                    //异步通知节点注销token
                    await _tokenNotify.NotifyTokensClear(Gid, Uid);
                    return HttpStatusCode.OK;
                }
                else
                {
                   
                    return HttpStatusCode.BadRequest;
                }

            }
            else
            {

                return HttpStatusCode.Unauthorized;
            }
            

        }




        /// <summary>
        /// 获取单个Token
        /// </summary>
        /// <returns></returns>
        [HttpGet("single")]
        public async Task<string> GetToken()
        {
            if (IsLegal)
            {
                return JsonResult(_tokenService.GetToken(Uid,Gid));
            }
            else
            {

                HttpContext.Response.StatusCode = 401;
                LyciumConfiguration.ReturnMessage(HttpContext, 4016, "主机校验不合法！");
                return null;
            }
        }



        /// <summary>
        /// 检测并核对本地Token,如果本地校验通过则返回最新的Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("check")]
        public async Task<string> CheckToken(LyciumToken token)
        {
            if (IsLegal)
            {
                var localToken = _tokenService.GetToken(Uid, Gid);
                if (localToken == null)
                {
                    return null;
                }
                if (token.Content == localToken.Content)
                {
                    return JsonResult(localToken);
                }
                else
                {
                    return null;
                }
            }
            else
            {

                HttpContext.Response.StatusCode = 401;
                LyciumConfiguration.ReturnMessage(HttpContext, 4016, "主机校验不合法！");
                return null;
            }
        }



        /// <summary>
        /// 检测并刷新Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("refresh")]
        public async Task<string> RefreshToken(LyciumToken token)
        {
            if (IsLegal)
            {

                var localToken = _tokenService.GetToken(Uid, Gid);
                if (localToken==null)
                {
                    return null;
                }
                if (token.Content == localToken.Content)
                {

                    if (TokenOperator.CanFlush(token))
                    {

                        TokenOperator.Flush(token);
                        if (_tokenService.ModifyToken(token))
                        {
                            await _tokenNotify.NotifyTokensModify(Gid, token);
                            return JsonResult(token);
                        }
                        else
                        {

                            //二次重试
                            if (_tokenService.ModifyToken(token))
                            {
                                await _tokenNotify.NotifyTokensModify(Gid, token);
                                return JsonResult(token);
                            }
                            else
                            {

                                HttpContext.Response.StatusCode = 500;
                                LyciumConfiguration.ReturnMessage(HttpContext, 4211, "服务端Token更新失败！");
                                return null;
                            }
                        }

                    }
                }
                return null;
            }
            else
            {

                HttpContext.Response.StatusCode = 401;
                LyciumConfiguration.ReturnMessage(HttpContext, 4016, "主机校验不合法！");
                return null;
            }
        }


    }

}
