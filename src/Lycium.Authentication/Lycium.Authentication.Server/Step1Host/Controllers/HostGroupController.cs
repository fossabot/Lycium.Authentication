﻿using Lycium.Authentication.Controllers;
using Lycium.Authentication.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        /// 获取该组下所有主机地址
        /// </summary>
        /// <param name="gid">组ID</param>
        /// <returns></returns>
        [HttpGet("group/host/query/{gid}")]
        public string GetHosts(long gid)
        {
            return JsonResult(_groupService.GetHostsUrlFromGroupId(gid));
        }


        /// <summary>
        /// 获取该组下所有主机地址
        /// </summary>
        /// <param name="gid">组ID</param>
        /// <returns></returns>
        [HttpGet("group/gid/query/{groupName}")]
        public long GetGidFromGroupName(string groupName)
        {
            return _groupService.GetGidFromGroupName(groupName);
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


        /// <summary>
        /// 查询主机是否在组中
        /// </summary>
        /// <param name="group">新增一个组</param>
        /// <returns></returns>
        [HttpPost("group/exist")]
        public bool QueryExistHost(long cid,long gid)
        {
            return _groupService.IsExistHostInGroup(cid,gid);
        }
    }

}
