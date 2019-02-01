using System;

namespace Tagger
{
    public class ExtendedHeader
    {
        private byte[] fileData;
        public uint ExtendedHeaderSize { get; set; }
        public bool IsCrcDataPresent { get; set; }
        public uint SizeOfPadding { get; set; }

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
            ParseCrcFlag();
            ParseSizeOfPadding();

            if (IsCrcDataPresent)
            {
                ParseCrc();
            }
        }

        private void ParseCrc()
        {
            // Placeholder
        }

        private void ParseExtendedHeaderSize()
        {
            this.ExtendedHeaderSize = (uint)((uint)fileData[10] << 24) | ((uint)fileData[11] << 16) | ((uint)fileData[12] << 8) | (uint)fileData[13];
        }

        private void ParseCrcFlag()
        {
            this.IsCrcDataPresent = BitUtil.IsBitSet(fileData[14], 7);
        }

        private void ParseSizeOfPadding()
        {
            this.SizeOfPadding = (uint)((uint)fileData[16] << 24) | ((uint)fileData[17] << 16) | ((uint)fileData[18] << 8) | (uint)fileData[19];
        }
    }
}
