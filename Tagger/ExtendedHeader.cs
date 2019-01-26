using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagger
{
    public class ExtendedHeader
    {
        public uint ExtendedHeaderSize { get; set; }

        public override string ToString()
        {
            string result = string.Empty;
            result += "ExtendedHeaderSize: " + ExtendedHeaderSize;
            return result;
        }
    }
}
