using Lycium.Authentication.Common;
using Lycium.Authentication.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Lycium.Authentication.Server
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseInfoController : PassController
    {

        private readonly IContextInfoService _info;
        private readonly IServerHostService _host;
        public BaseInfoController(
            IContextInfoService contextInfoService,
            IServerHostService serverHostService)
        {
            _info = contextInfoService;
            _host = serverHostService;
        }

        public long Cid
        {
            get { return _host.GetHostFromSecretKey(_info.GetSecretKeyFromContext(HttpContext)).Id; }
        }

        public long Uid
        {
            get { return _info.GetUidFromContext(HttpContext); }
        }


        public long Gid
        {
            get { return _info.GetGidFromContext(HttpContext); }
        }


        public string Token 
        {
            get { return _info.GetTokenFromContext(HttpContext); }
        }


        public bool IsLegal
        {
            get { return _host.OperationCheck(HttpContext) != null; }
        }

    }
}
