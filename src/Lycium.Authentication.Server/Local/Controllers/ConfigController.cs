using Lycium.Authentication.Controllers;
using Lycium.Authentication.Server.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace Lycium.Authentication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : PassController
    {

        private readonly IServerConfigurationService _configService;
        public ConfigController(IServerConfigurationService configService)
        {
            _configService = configService;
        }

        //***********************************Step 1.5: 创建Token认证配置 给节点用****************************//
        /// <summary>
        /// 创建一个新的 config 
        /// </summary>
        /// <param name="config"></param>
        [HttpPost("create")]
        public HttpStatusCode Create(LyciumConfig config)
        {
            return _configService.AddConfig(config) ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        }


        /// <summary>
        /// 更新一个 config 
        /// </summary>
        /// <param name="config"></param>
        [HttpPost("modify")]
        public HttpStatusCode Modify(LyciumConfig config)
        {
            return _configService.UpdateConfig(config) ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        }


        /// <summary>
        /// 查询配置列表
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="size">页容量</param>
        /// <returns></returns>
        [HttpGet("query")]
        public string Query(int page,int size)
        {
           return JsonResult(_configService.Query(page,size));
        }


        /// <summary>
        /// 模糊查询配置列表
        /// <param name="keyword">关键字</param>
        /// </summary>
        /// <returns></returns>
        [HttpGet("keywordquery/{keyword}")]
        public string QueryKeyword(int page, int size, string keyword)
        {
            return JsonResult(_configService.KeywordsQuery(page, size, keyword));
        }


        /// <summary>
        /// 一共有多少数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("totle")]
        public long Count()
        {
            return _configService.Count();
        }
    }

}
