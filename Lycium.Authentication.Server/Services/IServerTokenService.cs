using Lycium.Authentication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycium.Authentication
{
    public abstract class IServerTokenService
    {
        public abstract bool CheckToken(LyciumToken clientToken, LyciumToken serverToken);
        public abstract bool WriteToken(LyciumToken token);
        public abstract bool UpdateToken(LyciumToken token);
    }
}
