using System.Collections.Generic;

namespace Lycium.Authentication.Common
{
    public class TokenCollection
    {
        public TokenCollection()
        {
            Add = new List<LyciumToken>();
            Modify = new List<LyciumToken>();
            Remove = new List<LyciumToken>();
        }
        public List<LyciumToken> Add { get; set; }
        public List<LyciumToken> Modify { get; set; }
        public List<LyciumToken> Remove { get; set; }
    }
}
