using Lycium.Authentication.Common;

namespace Lycium.Authentication
{
    public abstract class IServerTokenService
    {
        public abstract bool CheckToken(LyciumToken clientToken, LyciumToken serverToken);
        public abstract bool WriteToken(LyciumToken token);
        public abstract bool UpdateToken(LyciumToken token);
    }
}
