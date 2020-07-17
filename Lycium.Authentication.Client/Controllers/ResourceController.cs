using Lycium.Authentication.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace Lycium.Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : PassController
    {

        private readonly IClientResourceService _resourceService;
        public ResourceController(IClientResourceService resourceService)
        {
            _resourceService = resourceService;
        }


        [HttpPost("set/whitelist")]
        public HttpStatusCode SetWhitelist(IEnumerable<string> whitelist)
        {
            _resourceService.AddWritelist(whitelist);
            return HttpStatusCode.OK;
        }


        [HttpPost("set/backlist")]
        public HttpStatusCode SetBacklist(IEnumerable<string> backlist)
        {
            _resourceService.AddBacklist(backlist);
            return HttpStatusCode.OK;
        }

    }

}
