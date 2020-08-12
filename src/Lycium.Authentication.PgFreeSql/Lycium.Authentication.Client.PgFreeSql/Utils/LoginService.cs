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
    public LyciumToken Login(long uid,string groupName)
    {
        var gid =_tokenService.GetGidFromServer(groupName);
        if (gid != 0)
        {
            return _tokenService.Login(uid, gid);
        }
        return null;
    }
    public HttpStatusCode Logout(long uid, string groupName)
    {
        var gid = _tokenService.GetGidFromServer(groupName);
        if (gid != 0)
        {
            return _tokenService.Logout(uid, gid);
        }
        return HttpStatusCode.BadRequest;
    }
    public LyciumToken Login(HttpContext context, string groupName)
    {
        var uid = _infoService.GetUidFromContext(context);
        var gid = _tokenService.GetGidFromServer(groupName);
        if (gid != 0)
        {
            return _tokenService.Login(uid, gid);
        }
        return null;
    }

    public HttpStatusCode Logout(HttpContext context, string groupName)
    {
        var uid = _infoService.GetUidFromContext(context);
        var gid = _tokenService.GetGidFromServer(groupName);
        if (gid != 0)
        {
            return _tokenService.Logout(uid, gid);
        }
        return HttpStatusCode.BadRequest;
    }

}

