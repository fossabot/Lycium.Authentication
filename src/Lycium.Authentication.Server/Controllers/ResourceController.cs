using Lycium.Authentication.Common;
using Lycium.Authentication.Server.Notify;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Lycium.Authentication.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {

        private readonly IServerHostService _hostService;
        private readonly IServerResourceService _resourceService;
        private readonly IResourceNotify _resourceNotify;
        private readonly IContextInfoService _infoService;
        public ResourceController(
            IServerHostService hostService, 
            IServerResourceService resourceService, 
            IResourceNotify resourceNotify,
            IContextInfoService infoService
            )
        {
            _hostService = hostService;
            _resourceService = resourceService;
            _resourceNotify = resourceNotify;
            _infoService = infoService;
        }


        /// <summary>
        /// 客户端上传资源,并返回白名单
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<string> AddResrouce(params string[] resources)
        {
            var secretKey = _infoService.GetSecretKeyFromContext(HttpContext);
            var host = _hostService.GetHostBySecretKey(secretKey);
            if (_resourceService.AddResources(host, resources))
            {
                return _resourceService.GetAllWhitelist(host.Id);
            }
            return null;
        }


        /// <summary>
        /// 根据主机ID分页查询主机资源
        /// </summary>
        /// <param name="cid">主机ID</param>
        /// <param name="page">当前页</param>
        /// <param name="size">页容量</param>
        /// <returns></returns>
        [HttpGet("query/{cid}/{page}/{size}")]
        public IEnumerable<LyciumResource> Query(long cid, int page, int size)
        {
            return _resourceService.Query(cid, page, size);
        }


        /// <summary>
        /// 根据主机ID查询所有主机资源
        /// </summary>
        /// <param name="cid">主机ID</param>
        /// <returns></returns>
        [HttpGet("queryall/{cid}/")]
        public IEnumerable<LyciumResource> QueryAll(long cid)
        {
            return _resourceService.QueryAll(cid);
        }


        /// <summary>
        /// 根据主机ID查询所有主机资源
        /// </summary>
        /// <param name="cid">主机ID</param>
        /// <returns></returns>
        [HttpGet("get/whitelist/id/{cid}")]
        public IEnumerable<string> QueryAllWhitelist(long cid)
        {
            return _resourceService.GetAllWhitelist(cid);
        }
        [HttpGet("get/whitelist/secretKey")]
        public IEnumerable<string> QueryAllWhitelist()
        {
            var secretKey = _infoService.GetSecretKeyFromContext(HttpContext);
            var host = _hostService.GetHostBySecretKey(secretKey);
            return _resourceService.GetAllWhitelist(host.Id);
        }


        /// <summary>
        /// 将资源批量设置为白名单
        /// </summary>
        /// <param name="cid">主机ID</param>
        /// <param name="resources">资源</param>
        /// <returns></returns>
        [HttpPost("set/whitelist")]
        public object SetWhitelist(long cid, params long[] resources)
        {

            if (_resourceService.SetResourceAsWhitelist(resources))
            {
                var host = _hostService.GetHostById(cid);
                var code = _resourceNotify.WhitelistNotify(host, _resourceService.GetWhitelist(resources));
                if (code == System.Net.HttpStatusCode.OK)
                {
                    return new { code = 0, msg = "更新成功，通知成功！" };
                }
                else
                {
                    return new { code = 1, msg = "更新成功，通知失败！" };
                }
            }
            return new { code = 2, msg = "更新失败，通知失败！" };

        }



        /// <summary>
        /// 将资源批量设置为黑名单
        /// </summary>
        /// <param name="cid">主机ID</param>
        /// <param name="resources">资源</param>
        /// <returns></returns>
        [HttpPost("set/backlist")]
        public object SetBacklist(long cid, params long[] resources)
        {

            if (_resourceService.SetResourceAsBlacklist(resources))
            {
                var host = _hostService.GetHostById(cid);
                var code = _resourceNotify.BacklistNotify(host, _resourceService.GetWhitelist(resources));
                if (code == System.Net.HttpStatusCode.OK)
                {
                    return new { code = 0, msg = "更新成功，通知成功！" };
                }
                else
                {
                    return new { code = 1, msg = "更新成功，通知失败！" };
                }
            }
            return new { code = 2, msg = "更新失败，通知失败！" };

        }

    }

}
