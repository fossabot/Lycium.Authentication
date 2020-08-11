using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lycium.Authentication.Server
{
    public class TokenIncrement
    {
        public long Cid { get; set; }
        public long Gid { get; set; }
        public long Uid { get; set; }
        public char OperatorType { get; set; }
        public long CreateTime { get; set; }
    }
}
