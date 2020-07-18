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
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Test()
        {
            return "test";
        }

        [HttpGet("{id}")]
        public string Test2(long id)
        {
            return "test";
        }


        [HttpGet("test2/{id}")]
        public string Test3(long id)
        {
            return "test";
        }
    }
}