using Lycium.Authentication.Common;
using Lycium.Authentication.Server;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Lycium.Authentication
{

    public abstract class IServerTokenService
    {

        public abstract bool CheckToken(LyciumToken clientToken);
        public abstract LyciumToken GetToken(long uid,long gid);
        public abstract bool AddToken(LyciumToken token);
        public abstract bool ModifyToken(LyciumToken token);
        public abstract bool RemoveToken(long uid, long gid);
        public abstract IEnumerable<LyciumToken> GetRangeTokens(IEnumerable<TokenIncrement> increments);

    }

}
