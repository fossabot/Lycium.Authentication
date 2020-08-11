using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeSql.DatabaseModel;
using Lycium.Authentication.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LyciumFreesqlServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : PassController
    {
        private readonly IFreeSql _freesql;
        public TestController(IFreeSql freeSql)
        {
            _freesql = freeSql;

        }

        [HttpGet("{name}")]
        public string Get(string name)
        {
            var table = _freesql.DbFirst.GetTablesByDatabase("qecms");
            foreach (var item in table)
            {
                if (item.Name == name)
                {
                    var tempTable = item;
                    foreach (var primarys in tempTable.Primarys)
                    {
                        if (primarys.IsPrimary)
                        {
                            return primarys.Name;
                        }
                        Console.WriteLine(primarys.Name);
                    }
                    
                    break;
                }
            }
            return null;
        }

        [HttpGet("fields/{name}")]
        public string GetFields(string name)
        {
            var table = _freesql.DbFirst.GetTablesByDatabase("qecms");
            foreach (var item in table)
            {
                if (item.Name == name)
                {
                    var tempTable = item;
                    foreach (var column in tempTable.Columns)
                    {
                        if (column.DbTypeText == "text")
                        {
                            return column.Name;
                        }
                    }

                    break;
                }
            }
            return null;
        }

        [HttpGet("getsql/{name}")]
        public object GetSqls(string name)
        {

            return _freesql
                .Select<Test>()
                .Where(item=>item.Id==1)
                .ToList<Test2>("a.\"Id\",\'Const\' AS \"Name\"").FirstOrDefault();

        }
    }

    public class Test
    {
        public long Id { get; set; }
    }

    public class Test2 :Test
    {
        public string Name { get; set; }
    }

}
