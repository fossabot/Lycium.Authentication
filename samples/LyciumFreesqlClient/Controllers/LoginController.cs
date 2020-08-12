using Lycium.Authentication.Common;
using Lycium.Authentication.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LyciumFreesqlClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : PassController
    {
        private readonly LoginService _loginService;
        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet("login")]
        public LyciumToken Login(long uid,string name)
        {

            return _loginService.Login(uid, name);
        }
        [HttpGet("logout")]
        public HttpStatusCode Logout(long uid, string name)
        {
            return _loginService.Logout(uid, name);
        }


    }
}
