using Lycium.Authentication.Common;
using Lycium.Authentication.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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


        [HttpGet("group/add/{cid}/{gid}")]
        public LyciumHost AddToGroup(long cid, long gid)
        {

        }
        [HttpGet("group/modify/{cid}/{gid}")]
        public LyciumHost ModifyGroup(long cid, long gid)
        {

        }
        [HttpGet("group/query/{page}/{size}/{cid}/{gid}")]
        public IEnumerable<LyciumHost> QueryPageGroup(long cid, long gid)
        {

        }
        [HttpGet("group/query/{page}/{size}/{cid}/{gid}")]
        public LyciumHost QuerySingleGroup(long cid, long gid)
        {

        }
        [HttpGet("group/delete/{cid}/{gid}")]
        public LyciumHost DeleteFromGroup(long cid, long gid)
        {

        }

       

    }

}
