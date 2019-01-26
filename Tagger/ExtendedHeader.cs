using System;

namespace Tagger
{
    public class ExtendedHeader
    {
        public uint ExtendedHeaderSize { get; set; }

        public override string ToString()
        {
            string result = string.Empty;
            result += "ExtendedHeaderSize: " + ExtendedHeaderSize + Environment.NewLine;
            return result;
        }
    }
}
