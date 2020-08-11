using Lycium.Authentication.Common;
using Lycium.Authentication.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Lycium.Authentication.Client
{
    [Route("api/[controller]")]
    [ApiController]
    public class LyciumTokenController : PassController
    {
        private readonly IClientTokenService _tokenService;
        public LyciumTokenController(IClientTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("add")]
        public HttpStatusCode Add(LyciumToken token)
        {
            if (_tokenService.AddToken(token))
            {
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.InternalServerError;
        }

        [HttpPost("modify")]
        public HttpStatusCode Modify(LyciumToken token)
        {
            if (_tokenService.ModifyToken(token))
            {
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.InternalServerError;
        }

        [HttpGet("remove/{uid}/{gid}")]
        public HttpStatusCode Remove(long uid,long gid)
        {
            if (_tokenService.RemoveToken(uid,gid))
            {
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.InternalServerError;
        }
    }
}
