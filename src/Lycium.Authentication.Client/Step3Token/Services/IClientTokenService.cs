using Lycium.Authentication.Common;
using System.Collections.Generic;
using System.Net;

namespace Lycium.Authentication
{
    public abstract class IClientTokenService
    {
        public abstract LyciumToken Login(long uid, long gid);
        public abstract HttpStatusCode Logout(long uid, long gid);
        public abstract bool RemoveTokens(IEnumerable<LyciumToken> tokens);
        public abstract bool RemoveToken(long uid, long gid);
        public abstract bool AddTokens(IEnumerable<LyciumToken> tokens);
        public abstract bool AddToken(LyciumToken token);
        public abstract bool ModifyTokens(IEnumerable<LyciumToken> tokens);
        public abstract bool ModifyToken(LyciumToken token);
        public abstract bool CheckToken(long uid, long gid, LyciumToken target);
        public abstract LyciumToken GetToken(long uid,long gid);
        //public virtual void GetTokenIncrements(TokenCollection token)
        //{
        //    RemoveTokens(token.Remove);
        //    AddTokens(token.Add);
        //    ModifyTokens(token.Modify);
        //}
        public abstract LyciumToken GetServerToken(long uid,long gid);
        public abstract LyciumToken RefreshToken(LyciumToken tokens, long uid,long gid);

    }
}
