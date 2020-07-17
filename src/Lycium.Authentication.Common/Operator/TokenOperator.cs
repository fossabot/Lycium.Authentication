using Lycium.Authentication.Common;
using System;

namespace Lycium.Authentication
{
    public static class TokenOperator
    {

        /// <summary>
        /// 判断单位时间内是否允许请求一个新的Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool ApplySucceed(LyciumToken token)
        {

            var nowTime = NowTimeStampSecond();
            var head = token._applyTimeSpan.First;
            while (head != null)
            {

                if (nowTime - head.Value > token.ApplyTimeLimit)
                {
                    token._applyTimeSpan.Remove(head);
                }
                else
                {
                    break;
                }
                head = head.Next;

            }

            if (!IsIllegal(token))
            {

                token._applyTimeSpan.AddLast(nowTime);
                return true;

            }
            else if (token.UseRollCheck)
            {

                token._applyTimeSpan.RemoveFirst();
                token._applyTimeSpan.AddLast(nowTime);

            }
            return false;

        }


        /// <summary>
        /// 是否是超过了请求次数
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsIllegal(LyciumToken token)
        {
            return token._applyTimeSpan.Count >= token.ApplyCountterLimit;
        }


        /// <summary>
        /// 该 Token 能否进行刷新操作
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool CanFlush(LyciumToken token)
        {
            return NowTimeStampSecond() <= token.FlushTimeSpan + token.CreateTime;
        }


        /// <summary>
        /// 该 Token 是否还存活
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsAlive(LyciumToken token)
        {
            return NowTimeStampSecond() <= token.AliveTimeSpan + token.CreateTime;
        }

        public static long NowTimeStampSecond()
        {
            return TimeStampSecond(DateTime.Now);
        }

        public static long TimeStampSecond(DateTime date)
        {
            return (date.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        public static long TimeStampMillisecond(DateTime date)
        {
            return (date.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        }

    }

}
