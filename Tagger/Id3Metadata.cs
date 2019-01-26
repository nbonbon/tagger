using System;

namespace Tagger
{
    public class Id3Metadata
    {
        private byte[] fileData;

        public Id3Metadata(byte[] fileData)
        {
            this.fileData = fileData;
        }

        public byte VersionMajor { get; set; }
        public byte VersionMinor { get; set; }
        public byte VersionRevision { get; set; }

        public string Version
        {
            get
            {
                return VersionMajor.ToString() + "." + VersionMinor + "." + VersionRevision;
            }

            private set{ }
        }

        public bool IsUnsynchronisationUsed { get; set; }
        public bool ContainsExtendedHeader { get; set; }
        public bool IsExperimentalStage { get; set; }
        public uint TagSize { get; set; }
        public ExtendedHeader ExtendedHeader { get; set; }

        public override string ToString()
        {
            string result = string.Empty;
            result += "Raw data: " + BitConverter.ToString(fileData) + Environment.NewLine;
            result += "Version: " + Version + Environment.NewLine;
            result += "ContainsExtendedHeader: " + ContainsExtendedHeader + Environment.NewLine;
            result += "IsExperimentalStage: " + IsExperimentalStage + Environment.NewLine;
            result += "TagSize: " + TagSize + Environment.NewLine;
            result += ExtendedHeader != null ? ExtendedHeader.ToString() : string.Empty;
            return result;
        }

        public static bool isID3v2(byte[] fileData)
        {
            return System.Text.Encoding.ASCII.GetString(fileData, 0, 3).ToUpper() == "ID3";
        }

        public void Parse()
        {
            ParseHeader();

            if (ContainsExtendedHeader)
            {
                ExtendedHeader = new ExtendedHeader();
                ParseExtendedHeaderSize();
            }
        }

        private void ParseHeader()
        {
            ParseVersion();
            ParseFlags();
            ParseTagSize();
        }

        public void ParseVersion()
        {
            VersionMajor = 2;
            VersionMinor = fileData[3];
            VersionRevision = fileData[4];
        }

        public void ParseFlags()
        {
            byte flagByte = fileData[5];
            IsUnsynchronisationUsed = IsBitSet(flagByte, 7);
            ContainsExtendedHeader = IsBitSet(flagByte, 6);
            IsExperimentalStage = IsBitSet(flagByte, 5);
        }

        private static bool IsBitSet(byte byteToSearch, int bitNumberToCheck)
        {
            return (byteToSearch & (1 << bitNumberToCheck)) != 0;
        }

        public  void ParseTagSize()
        {
            TagSize = (uint)(((fileData[6] & 0x7F) << 25) | ((fileData[7] & 0x7F) << 18) | ((fileData[8] & 0x7F) << 11) | ((fileData[9] & 0x7F) << 4)) >> 4;
        }

        public void ParseExtendedHeaderSize()
        {
            if (ExtendedHeader != null)
            {
                ExtendedHeader.ExtendedHeaderSize = (uint)((uint)fileData[10] << 24) | ((uint)fileData[11] << 16) | ((uint)fileData[12] << 8) | (uint)fileData[13];
            }
        }
    }
}
