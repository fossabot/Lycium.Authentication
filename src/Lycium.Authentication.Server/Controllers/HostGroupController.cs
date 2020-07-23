using Lycium.Authentication.Common;
using Lycium.Authentication.Controllers;
using Lycium.Authentication.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace Lycium.Authentication.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HostGroupController : PassController
    {

        private readonly IServerHostGroupService _groupService;
        public HostGroupController(IServerHostGroupService groupService)
        {
            _groupService = groupService;
        }


        /// <summary>
        /// 将主机添加到组
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        [HttpGet("group/host/add/{cid}/{gid}")]
        public bool AddToGroup(long cid, long gid)
        {
            return _groupService.AddHostsToGroup(gid, cid);
        }

        /// <summary>
        /// 将主机从组删除
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        [HttpGet("group/host/delete/{cid}/{gid}")]
        public bool DeletedFromGroup(long cid, long gid)
        {
            return _groupService.DeletedHostFromGroup(gid, cid);
        }

        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        [HttpGet("group/delete/{gid}")]
        public bool DeleteGroup(long gid)
        {
           return _groupService.DeletedGroup(gid);
        }


        /// <summary>
        /// 获取该组下所有主机
        /// </summary>
        /// <param name="gid">组ID</param>
        /// <returns></returns>
        [HttpGet("group/host/query/{gid}")]
        public IEnumerable<LyciumHost> GetHosts(long gid)
        {
            return _groupService.GetHostsByGroupId(gid);
        }


        /// <summary>
        /// 新增一个组
        /// </summary>
        /// <param name="group">新增一个组</param>
        /// <returns></returns>
        [HttpPost("group/add")]
        public bool AddGroup(LyciumHostGroup group)
        {
            return _groupService.AddGroup(group);
        }


    }

}
