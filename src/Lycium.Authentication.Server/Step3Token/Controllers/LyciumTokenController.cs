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
        public async Task<LyciumToken> LoginToken()
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
                        return token;
                    }
                }
                else
                {
                    //如果数据库有则更新
                    if (_tokenService.ModifyToken(token))
                    {
                        //如果修改成功则发送通知
                        await _tokenNotify.NotifyTokensModify(Gid, token);
                        return token;
                    }
                }

            }
            Response.StatusCode = 401;
            await Response.WriteAsync("检测到非法客户端请求，请联系管理员！");
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
                    Response.StatusCode = 200;
                    await Response.WriteAsync("注销成功！");
                }
                else
                {
                    Response.StatusCode = 400;
                    await Response.WriteAsync("未检测到需要注销的Token！");
                }

            }

            Response.StatusCode = 401;
            await Response.WriteAsync("检测到非法客户端请求，请联系管理员！");
            return HttpStatusCode.Unauthorized;

        }




        /// <summary>
        /// 获取单个Token
        /// </summary>
        /// <returns></returns>
        [HttpGet("single")]
        public async Task<LyciumToken> GetToken()
        {
            if (IsLegal)
            {
                return _tokenService.GetToken(Uid,Gid);
            }
            Response.StatusCode = 401;
            await Response.WriteAsync("检测到非法客户端请求，请联系管理员！");
            return null;
        }



        /// <summary>
        /// 检测并核对本地Token,如果本地校验通过则返回最新的Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("check")]
        public async Task<LyciumToken> CheckToken(LyciumToken token)
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
                    return localToken;
                }
                else
                {
                    return null;
                }
            }
            Response.StatusCode = 401;
            await Response.WriteAsync("检测到非法客户端请求，请联系管理员！");
            return null;
        }



        /// <summary>
        /// 检测并刷新Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("refresh")]
        public async Task<LyciumToken> RefreshToken(LyciumToken token)
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
                            return token;
                        }
                        else
                        {

                            //二次重试
                            if (_tokenService.ModifyToken(token))
                            {
                                await _tokenNotify.NotifyTokensModify(Gid, token);
                                return token;
                            }
                            else
                            {
                                return null;
                            }

                        }

                    }
                    else
                    {
                        return null;
                    }

                }

            }
            Response.StatusCode = 401;
            await Response.WriteAsync("检测到非法客户端请求，请联系管理员！");
            return null;
        }


    }

}
