using Lycium.Authentication;
using Lycium.Authentication.Common;
using LyciumFreesqlClient.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LyciumFreesqlClient.Services
{
    public class FreeSqlClientTokenService : IClientTokenService
    {
        private readonly LyciumRequest _request;
        private readonly IFreeSql _freeSql;
        public FreeSqlClientTokenService(LyciumRequest request,IFreeSql freesql)
        {
            _freeSql = freesql;
            _request = request;
        }

        public override bool AddToken(LyciumToken token)
        {
            RemoveToken(token.Uid, token.Gid);
            return _freeSql.Insert(token).ExecuteAffrows() == 1;
        }

        public override bool AddTokens(IEnumerable<LyciumToken> tokens)
        {
            return _freeSql.Insert(tokens).ExecuteAffrows() == tokens.Count();
        }

        public override bool CheckToken(long uid, long gid, LyciumToken target)
        {
            throw new NotImplementedException();
        }
        
        public override LyciumToken GetServerToken(long uid, long gid)
        {
            return _request.Get<LyciumToken>("api/LyciumToken/single", uid, gid);
        }

        public override LyciumToken GetToken(long uid, long gid)
        {
            return _freeSql.Select<LyciumToken>().Where(item => item.Uid == uid && item.Gid == gid).First();
        }

        public override LyciumToken Login(long uid, long gid)
        {
            return _request.Get<LyciumToken>("api/LyciumToken/LoginToken", uid, gid);
        }

        public override HttpStatusCode Logout(long uid, long gid)
        {
            return _request.Get<HttpStatusCode>("api/LyciumToken/LogoutToken", uid, gid);
        }

        public override bool ModifyToken(LyciumToken token)
        {

            var result = _freeSql.Update<LyciumToken>().SetDto(token).Where(item => item.Uid == token.Uid && item.Gid == token.Gid).ExecuteAffrows() == 1;
            if (!result)
            {
                var tempToken = GetToken(token.Uid, token.Gid);
                if (tempToken == null)
                {
                    result = AddToken(token);
                }
            }
            return result;

        }

        public override bool ModifyTokens(IEnumerable<LyciumToken> tokens)
        {
            throw new NotImplementedException();
        }

        public override LyciumToken RefreshToken(LyciumToken token, long uid, long gid)
        {
            return _request.Post<LyciumToken, LyciumToken>("api/LyciumToken/refresh", token, uid, gid).Result;
        }

        public override bool RemoveToken(long uid, long gid)
        {
            return _freeSql.Delete<LyciumToken>().Where(item => item.Uid == uid && item.Gid == gid).ExecuteAffrows() == 1;
        }

        public override bool RemoveTokens(IEnumerable<LyciumToken> tokens)
        {
            throw new NotImplementedException();
        }
    }
}
