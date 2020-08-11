using Microsoft.AspNetCore.Mvc;

namespace Lycium.Authentication.Server.Controllers
{

    /// <summary>
    /// 主机通信Debug路由
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HostDebugController : ControllerBase
    {

        private readonly IServerHostService _hostService;
        private readonly IServerConfigurationService _configService;
        private readonly IContextInfoService _infoService;
        public HostDebugController(IServerHostService hostService, IServerConfigurationService serverConfiguration, IContextInfoService infoService)
        {
            _hostService = hostService;
            _configService = serverConfiguration;
            _infoService = infoService;
        }

        /// <summary>
        /// 检测主机
        /// </summary>
        /// <returns></returns>
        [HttpGet("connected")]
        public string CheckHost()
        {

            var secretKey = _infoService.GetSecretKeyFromContext(HttpContext);
            if (secretKey == null)
            {
                return "客户端 SecretKey 为空，请检查！";
            }


            var host = _hostService.GetHostFromSecretKey(secretKey);
            if (host == null)
            {
                return $"检测到 SecretKey 为 {secretKey.Substring(0, 6)}... 的主机未注册！";
            }

            return "检测通过！";

        }

        /// <summary>
        /// 检测主机
        /// </summary>
        /// <returns></returns>
        [HttpGet("available")]
        public string CheckHostWithToken()
        {

            var secretKey = _infoService.GetSecretKeyFromContext(HttpContext);
            if (secretKey == null)
            {
                return "客户端 SecretKey 为空，请检查！";
            }


            var host = _hostService.GetHostFromSecretKey(secretKey);
            if (host == null)
            {
                return $"检测到 SecretKey 为 {secretKey.Substring(0, 6)}... 的主机未注册！";
            }


            var token = _infoService.GetHostTokenFromContext(HttpContext);
            if (token != host.HostToken)
            {
                return $"检测到客户端主机 Token ：{token}与服务端的记录不匹配！";
            }
            return "检测通过！";

        }
    }
}
