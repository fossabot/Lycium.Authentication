using Lycium.Authentication;
using Lycium.Authentication.Common;
using Microsoft.AspNetCore.Http;
using System.Net;


public class LoginService
{
    private readonly IClientTokenService _tokenService;
    private readonly IContextInfoService _infoService;
    public LoginService(IClientTokenService tokenService, IContextInfoService infoService)
    {
        _tokenService = tokenService;
        _infoService = infoService;
    }
    public LyciumToken Login(long uid,long gid)
    {
        return _tokenService.Login(uid, gid);
    }
    public HttpStatusCode Logout(long uid, long gid)
    {
        return _tokenService.Logout(uid, gid);
    }
    public LyciumToken Login(HttpContext context)
    {
        var uid = _infoService.GetUidFromContext(context);
        var gid = _infoService.GetGidFromContext(context);
        return _tokenService.Login(uid, gid);
    }

    public HttpStatusCode Logout(HttpContext context)
    {
        var uid = _infoService.GetUidFromContext(context);
        var gid = _infoService.GetGidFromContext(context);
        return _tokenService.Logout(uid, gid);
    }

}

