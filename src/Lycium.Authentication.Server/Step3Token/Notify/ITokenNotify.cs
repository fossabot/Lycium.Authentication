using Lycium.Authentication.Common;
using System.Threading.Tasks;

namespace Lycium.Authentication.Server
{
    public abstract class ITokenNotify
    {
        public abstract Task NotifyTokensAdd(long gid, LyciumToken token);
        public abstract Task NotifyTokensModify(long gid, LyciumToken token);
        public abstract Task NotifyTokensClear(long gid, long uid);


        /// <summary>
        /// 获取操作增量
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public TokenIncrement GetAddIncrement(long cid,LyciumToken token)
        {
            var increment = new TokenIncrement();
            increment.OperatorType = 'c';
            increment.Uid = token.Uid;
            increment.Gid = token.Gid;
            increment.Cid = cid;
            increment.CreateTime = TokenOperator.NowTimeStampSecond();
            return increment;
        }

        /// <summary>
        /// 获取操作增量
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public TokenIncrement GetModifyIncrement(long cid, LyciumToken token)
        {
            var increment = new TokenIncrement();
            increment.OperatorType = 'u';
            increment.Uid = token.Uid;
            increment.Gid = token.Gid;
            increment.Cid = cid;
            increment.CreateTime = TokenOperator.NowTimeStampSecond();
            return increment;
        }


        /// <summary>
        /// 获取操作增量
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public TokenIncrement GetRemoveIncrement(long cid, long uid, long gid)
        {
            var increment = new TokenIncrement();
            increment.OperatorType = 'd';
            increment.Uid = uid;
            increment.Gid = gid;
            increment.Cid = cid;
            increment.CreateTime = TokenOperator.NowTimeStampSecond();
            return increment;
        }

    }
}
