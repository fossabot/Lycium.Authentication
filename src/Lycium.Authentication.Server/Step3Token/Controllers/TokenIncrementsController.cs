using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lycium.Authentication.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lycium.Authentication.Server.Step3Token.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenIncrementsController : BaseInfoController
    {

        private readonly IServerTokenService _tokenService;
        private readonly ITokenIncrementService _tokenIncrementService;
        public TokenIncrementsController(
            IContextInfoService infoService,
            IServerHostService hostService,
            IServerTokenService tokenService,
            ITokenIncrementService tokenIncrementService
            ) : base(infoService, hostService)
        {
            _tokenService = tokenService;
            _tokenIncrementService = tokenIncrementService;
        }

        /// <summary>
        /// 获取主机的 Token 增量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<TokenCollection> GetCollection()
        {
            if (IsLegal)
            {
                var collection = new TokenCollection();
                var result = _tokenIncrementService.GetCompressionIncrements(Cid);
                collection.Add.AddRange(_tokenService.GetRangeTokens(result.Where(item => item.OperatorType == 'c')));
                collection.Remove.AddRange(_tokenService.GetRangeTokens(result.Where(item => item.OperatorType == 'd')));
                collection.Modify.AddRange(_tokenService.GetRangeTokens(result.Where(item => item.OperatorType == 'u')));
                return collection;
            }
            Response.StatusCode = 401;
            await Response.WriteAsync("检测到非法客户端请求，请联系管理员！");
            return null;
        }
    }
}
