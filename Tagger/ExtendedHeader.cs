﻿using System;

namespace Tagger
{
    public class ExtendedHeader
    {
        private byte[] fileData;
        private int byteOffset = -1;

        public uint ExtendedHeaderSize { get; set; }
        public bool IsCrcDataPresent { get; set; }
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
            byteOffset += 4;
            return byteOffset; 
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
