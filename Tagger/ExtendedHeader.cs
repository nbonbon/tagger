using System;

namespace Tagger
{
    public class ExtendedHeader
    {
        private byte[] fileData;
        public uint ExtendedHeaderSize { get; set; }

        public ExtendedHeader(byte[] fileData)
        {
            this.fileData = fileData;
        }

        public override string ToString()
        {
            string result = string.Empty;
            result += "ExtendedHeaderSize: " + ExtendedHeaderSize + Environment.NewLine;
            return result;
        }

        public void Parse()
        {
            ParseExtendedHeaderSize();
        }

        public void ParseExtendedHeaderSize()
        {
            this.ExtendedHeaderSize = (uint)((uint)fileData[10] << 24) | ((uint)fileData[11] << 16) | ((uint)fileData[12] << 8) | (uint)fileData[13];
        }
    }
}
