using Lycium.Authentication.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Lycium.Authentication.Server
{

    [Route("api/[controller]")]
    [ApiController]
    public class HeartbeatController : PassController
    {

        [HttpGet]
        public HttpStatusCode Get()
        {
            return HttpStatusCode.OK;
        }

    }

}
