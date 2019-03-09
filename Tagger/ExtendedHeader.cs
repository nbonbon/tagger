using System;

namespace Tagger
{
    public class ExtendedHeader
    {
        private const int EXTENDED_HEADER_SIZE_SIZE = 4;
        private byte[] fileData;
        private int byteOffset = -1;

        /// <summary>
        /// Size of the header excluding itself (currently 6 or 10 bytes (10 if CRC is present))
        /// </summary>
        public uint ExtendedHeaderSize { get; set; }

        /// <summary>
        /// If this is set then 4 bytes are appended to the extended header for the CRC
        /// </summary>
        public bool IsCrcDataPresent { get; set; }

        /// <summary>
        /// Size of the total tag minus the frames and headers (padding or non data)
        /// </summary>
        public uint SizeOfPadding { get; set; }

        public ExtendedHeader(byte[] fileData, int byteOffset)
        {
            this.fileData = fileData;
            this.byteOffset = byteOffset;
        }

        public override string ToString()
        {
            string result = string.Empty;
            result += "ExtendedHeader Size: " + ExtendedHeaderSize + Environment.NewLine;
            result += "ExtendedHeader IsCrcDataPresent: " + IsCrcDataPresent + Environment.NewLine;
            result += "ExtendedHeader SizeOfPadding: " + SizeOfPadding + Environment.NewLine;
            return result;
        }

        public int Parse()
        {
            ParseExtendedHeaderSize();
            ParseCrcFlag();
            ParseSizeOfPadding();

            if (IsCrcDataPresent)
            {
                ParseCrc();
            }

            // Extended size does not include the header size bytes so add 4 to it to account for those bytes
            byteOffset += (int)this.ExtendedHeaderSize;
            byteOffset += EXTENDED_HEADER_SIZE_SIZE;
            return byteOffset; 
        }

        private void ParseCrc()
        {
            // Placeholder
        }

        private void ParseExtendedHeaderSize()
        {
            this.ExtendedHeaderSize = BitUtil.ParseBigEndianUint32(fileData, 10);
        }

        private void ParseCrcFlag()
        {
            this.IsCrcDataPresent = BitUtil.IsBitSet(fileData[14], 7);
        }

        private void ParseSizeOfPadding()
        {
            this.SizeOfPadding = BitUtil.ParseBigEndianUint32(fileData, 16);
        }
    }
}
