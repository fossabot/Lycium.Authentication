using Lycium.Authentication;
using Lycium.Authentication.Common;
using Lycium.Authentication.Server;
using System;
using System.Collections.Generic;

namespace LyciumFreesqlServer.Services
{

    public class FreeSqlServerTokenService : IServerTokenService
    {


        private readonly IFreeSql _freeSql;
        public FreeSqlServerTokenService(IFreeSql freesql)
        {
            _freeSql = freesql;
        }

        public override bool AddToken(LyciumToken token)
        {
            return _freeSql.Insert(token).ExecuteAffrows() == 1;
        }

        public override bool CheckToken(LyciumToken clientToken)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<LyciumToken> GetRangeTokens(IEnumerable<TokenIncrement> increments)
        {
            throw new NotImplementedException();
        }

        public override LyciumToken GetToken(long uid, long gid)
        {
            return _freeSql.Select<LyciumToken>().Where(item => item.Uid == uid && item.Gid == gid).First();
        }

        public override bool ModifyToken(LyciumToken token)
        {
            return _freeSql.Update<LyciumToken>().SetDto(token).Where(item => item.Uid == token.Uid && item.Gid == token.Gid).ExecuteAffrows() == 1;
        }

        public override bool RemoveToken(long uid, long gid)
        {
            return _freeSql.Delete<LyciumToken>().Where(item => item.Uid == uid && item.Gid == gid).ExecuteAffrows() == 1;
        }
    }

}
