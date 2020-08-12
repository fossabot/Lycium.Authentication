using Lycium.Authentication.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Lycium.Authentication.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostDebugController : PassController
    {

        [HttpGet("remoteinfo")]
        public string IsConnectedWithServer()
        {
            return ClientHostDebugService.InfoFromServer("connected");
        }

        [HttpGet("remotefullinfo")]
        public string AvailableWithServer()
        {
            return ClientHostDebugService.InfoFromServer("available");
        }


        [HttpGet("heartbeat")]
        public bool Get()
        {
            return ClientHostDebugService.Heartbeat();
        }
    }
}
