using Lycium.Authentication.Common;
using Lycium.Authentication.Server.Notify;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Lycium.Authentication.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {

        private readonly IServerHostService _hostService;
        private readonly IServerResourceService _resourceService;
        private readonly IResourceNotify _resourceNotify;
        public ResourceController(
            IServerHostService hostService, 
            IServerResourceService resourceService, 
            IResourceNotify resourceNotify
            )
        {
            _hostService = hostService;
            _resourceService = resourceService;
            _resourceNotify = resourceNotify;
        }


        //***********************************Step 4: 节点上传本地资源****************************//
        /// <summary>
        /// 客户端上传资源,并返回白名单
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<string> AddResrouce(params string[] resources)
        {

            var host = _hostService.OperationCheck(HttpContext);
            if (host != null)
            {
                if (_resourceService.AddResources(host, resources))
                {
                    return _resourceService.GetAllAllowlist(host.Id);
                }
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
        [HttpGet("get/allowlist/id/{cid}")]
        public IEnumerable<string> QueryAllAllowlist(long cid)
        {
            return _resourceService.GetAllAllowlist(cid);
        }



        //***********************************Step 5: 节点获取所有白名单资源****************************//
        /// <summary>
        /// 主机查询所有白名单资源
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/allowlist/secretKey")]
        public IEnumerable<string> QueryAllAllowlist()
        {

            var host = _hostService.OperationCheck(HttpContext);
            if (host != null)
            {
                return _resourceService.GetAllAllowlist(host.Id);
            }
            return null;

        }


        /// <summary>
        /// 将资源批量设置为白名单
        /// </summary>
        /// <param name="cid">主机ID</param>
        /// <param name="resources">资源</param>
        /// <returns></returns>
        [HttpPost("set/allowlist")]
        public object SetAllowList(long cid, params long[] resources)
        {

            if (_resourceService.ModifyResourceAsAllowlist(resources))
            {
                var host = _hostService.GetHostFromId(cid);
                var code = _resourceNotify.NotifyAllowlist(host, _resourceService.GetAllowlist(resources));
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
        [HttpPost("set/blocklist")]
        public object SetBlockList(long cid, params long[] resources)
        {

            if (_resourceService.ModifyResourceAsBlocklist(resources))
            {
                var host = _hostService.GetHostFromId(cid);
                var code = _resourceNotify.NotifyBlocklist(host, _resourceService.GetAllowlist(resources));
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
