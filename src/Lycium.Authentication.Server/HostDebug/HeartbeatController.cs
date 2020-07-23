using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Lycium.Authentication.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HeartbeatController : ControllerBase
    {

        [HttpGet]
        public HttpStatusCode Get()
        {
            return HttpStatusCode.OK;
        }

    }

}
