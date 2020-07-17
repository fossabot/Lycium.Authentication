using System.Collections.Generic;

namespace Lycium.Authentication.Common
{
    public class LyciumToken
    {
        public static long GlobalFlushTimeSpan;
        public static long GlobalAliveTimeSpan;

        static LyciumToken()
        {
            GlobalFlushTimeSpan = 60 * 60 * 24 * 14;
            GlobalAliveTimeSpan = 60 * 60 * 24 * 5;
        }


        internal LinkedList<long> _applyTimeSpan;
        public LyciumToken()
        {

            _applyTimeSpan = new LinkedList<long>();
            FlushTimeSpan = GlobalFlushTimeSpan;
            AliveTimeSpan = GlobalAliveTimeSpan;

        }


        public long Uid { get; set; }
        public string PreContent { get; set; }
        public string Content { get; set; }
        public long CreateTime { get; set; }
        public long AliveTimeSpan { get; set; }
        public long FlushTimeSpan { get; set; }
        public long ApplyTimeLimit { get; set; }
        public long ApplyCountterLimit { get; set; }
        public bool UseRollCheck { get; set; }
    }
}
