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
    public class HeartbeatController : PassController
    {

        /// <summary>
        /// 心跳检测接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpStatusCode Get()
        {
            return HttpStatusCode.OK;
        }


    }

}
