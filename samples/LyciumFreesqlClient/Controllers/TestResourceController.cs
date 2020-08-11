using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LyciumFreesqlClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestResourceController : ControllerBase
    {
        [HttpGet]
        public string GetResource()
        {
            return "AAAAAAA";
        }
    }
}
