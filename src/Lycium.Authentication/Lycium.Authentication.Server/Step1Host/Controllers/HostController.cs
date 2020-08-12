using Lycium.Authentication.Common;
using Lycium.Authentication.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Lycium.Authentication.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HostController : PassController
    {

        private readonly IServerHostService _hostService;
        private readonly IServerConfigurationService _configService;
        public HostController(IServerHostService hostService, IServerConfigurationService serverConfiguration)
        {
            _hostService = hostService;
            _configService = serverConfiguration;
        }


        //***********************************Step 1: 创建主机并自动分配 SecretKey****************************//
        /// <summary>
        /// 添加一个主机
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public string Create(LyciumHost host)
        {
            host.SecretKey = Guid.NewGuid().ToString();
            return _hostService.AddHost(host) ? JsonResult(host) : null;
        }


        /// <summary>
        /// 修改一个主机
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        [HttpPost("modify")]
        public HttpStatusCode Modify(LyciumHost host)
        {
            return _hostService.ModifyHost(host) ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        }


        //***********************************Step 2: 节点开机,认证 SecretKey 并分配Token****************************//
        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <returns></returns>
        [HttpGet("refreshToken")]
        public string RefreshToken()
        {

            var host = _hostService.RequestCheck(HttpContext);
            if (host!=null)
            {
                var config = _configService.GetConfigById(host.ConfigId);
                if (config != null)
                {
                    host.TokenAliveTime = config.TokenAlivaTime;
                    host.TokenCreateTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
                    host.HostToken = Guid.NewGuid().ToString();
                    if (_hostService.ModifyHost(host))
                    {
                        var obj = new { host.HostToken, host.TokenAliveTime,host.TokenCreateTime };
                        return JsonResult(obj);
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// 刷新ClientUrl
        /// </summary>
        /// <returns></returns>
        [HttpGet("clientUrl/{url}")]
        public HttpStatusCode ChangeClientUrl(string url)
        {

            var host = _hostService.RequestCheck(HttpContext);
            if (host != null)
            {
                var config = _configService.GetConfigById(host.ConfigId);
                if (config != null)
                {
                    host.HostUrl = url;
                    if (_hostService.ModifyHost(host))
                    {
                        return HttpStatusCode.OK;
                    }
                }
            }
            return HttpStatusCode.BadRequest;

        }


        /// <summary>
        /// 查询主机列表
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="size">页容量</param>
        /// <returns></returns>
        [HttpGet("query")]
        public string Query(int page, int size)
        {
            return JsonResult(_hostService.Query(page, size));
        }


        /// <summary>
        /// 模糊主机配置列表
        /// <param name="keyword">关键字</param>
        /// </summary>
        /// <returns></returns>
        [HttpGet("keywordquery/{keyword}")]
        public string QueryKeyword(int page, int size, string keyword)
        {
            return JsonResult(_hostService.KeywordQuery(page, size, keyword));
        }


        /// <summary>
        /// 一共有多少数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("totle")]
        public long Count()
        {
            return _hostService.Count();
        }

    }

}
