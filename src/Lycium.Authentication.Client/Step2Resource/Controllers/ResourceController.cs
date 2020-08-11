using Lycium.Configuration;
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

        [HttpPost("get/allowlist")]
        public IEnumerable<string> GetAllowlist()
        {
            return ClientConfiguration.Allowlist;
        }


        [HttpPost("set/allowlist")]
        public HttpStatusCode SetAllowlist(IEnumerable<string> allowlist)
        {
            _resourceService.AddAllowlist(allowlist);
            return HttpStatusCode.OK;
        }


        [HttpPost("set/backlist")]
        public HttpStatusCode SetBacklist(IEnumerable<string> backlist)
        {
            _resourceService.AddBlocklist(backlist);
            return HttpStatusCode.OK;
        }

    }

}
