using Lycium.Auth;
using Lycium.Auth.Controllers;
using Lycium.Auth.DataAccess;
using Lycium.Auth.Service;
using Lycium.Request;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lyicum.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : LyciumHostController<LyciumResource>
    {
        private readonly LyciumNotifier _notifier;

        public ResourceController(
            LyciumNotifier notifier,
    IEntitySetter<LyciumResource> setter,
    IEntityGetter<LyciumResource> getter,
    IContextInfoService info,
    IHostService host) : base(setter, getter, host, info)
        {
            _notifier = notifier;
        }



        public override async Task<bool> Post([FromBody] params LyciumResource[] values)
        {

            if (values != null)
            {

                var host = _host.GetHost(HttpContext);
                var list = _getter.GetCollectionFromLong(host.Id);
                
                if (list != null)
                {

                    var checkList = new HashSet<string>(list.Select(item => item.Resource));
                    var insertList = new List<LyciumResource>();

                    foreach (var item in values)
                    {

                        if (!checkList.Contains(item.Resource))
                        {
                            item.Cid = host.Id;
                            insertList.Add(item);
                        }

                    }

                    var writeList = list.Where(item => item.InWhiteList).Select(item => item.Resource);
                    await _notifier.SendAllWhiteList(host, writeList.ToArray());
                    return base.Post(insertList.ToArray()).Result;
                }
                

            }

            return true;
        }



        [HttpGet("WhiteList/{cid}")]
        public IEnumerable<string> GetWhiteList(long cid)
        {

            return _getter.GetCollectionFromLong(cid)
                .Where(item => item.InWhiteList)
                .Select(item => item.Resource);

        }


        [HttpPut("WhiteList")]
        public async Task<HttpStatusCode> PutWhiteList([FromBody] LyciumResource value)
        {

            if (Put(value).Result)
            {

                if (value.InWhiteList)
                {

                    var host = _host.GetHost(value.Cid);
                    var result = await _notifier.SendWhiteListIncrement(host, value.Resource);
                    return result;

                }

            }
            return HttpStatusCode.OK;
        }

    }

}
