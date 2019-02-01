using System;
using System.Text;

namespace Tagger
{
    public class Id3Metadata
    {
        private byte[] fileData;
        private static Encoding textEnconding = Encoding.GetEncoding("iso-8859-1");

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
            return textEnconding.GetString(fileData, 0, 3).ToUpper() == "ID3";
        }

        public void Parse()
        {
            ParseHeader();

            if (ContainsExtendedHeader)
            {
                ExtendedHeader = new ExtendedHeader(this.fileData);
                ExtendedHeader.Parse();
            }
        }

        private void ParseHeader()
        {
            ParseVersion();
            ParseFlags();
            ParseTagSize();
        }

        private void ParseVersion()
        {
            VersionMajor = 2;
            VersionMinor = fileData[3];
            VersionRevision = fileData[4];
        }

        private void ParseFlags()
        {
            byte flagByte = fileData[5];
            IsUnsynchronisationUsed = BitUtil.IsBitSet(flagByte, 7);
            ContainsExtendedHeader = BitUtil.IsBitSet(flagByte, 6);
            IsExperimentalStage = BitUtil.IsBitSet(flagByte, 5);
        }

        private void ParseTagSize()
        {
            TagSize = (uint)(((fileData[6] & 0x7F) << 25) | ((fileData[7] & 0x7F) << 18) | ((fileData[8] & 0x7F) << 11) | ((fileData[9] & 0x7F) << 4)) >> 4;
        }
    }
}
