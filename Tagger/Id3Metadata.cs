using System;
using System.Text;

namespace Tagger
{
    public class Id3Metadata
    {
        private byte[] fileData;
        private int byteOffset = -1;
        private static Encoding textEnconding = Encoding.GetEncoding("iso-8859-1");

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
        public string LeadArtist { get; set; }

        public Id3Metadata(byte[] fileData)
        {
            this.fileData = fileData;
            byteOffset = 0;
        }

        public override string ToString()
        {
            string result = string.Empty;
            result += "Raw data: " + BitConverter.ToString(fileData) + Environment.NewLine;
            result += "Version: " + Version + Environment.NewLine;
            result += "ContainsExtendedHeader: " + ContainsExtendedHeader + Environment.NewLine;
            result += "IsExperimentalStage: " + IsExperimentalStage + Environment.NewLine;
            result += "TagSize: " + TagSize + Environment.NewLine;
            result += ExtendedHeader != null ? ExtendedHeader.ToString() : string.Empty;
            result += "LeadArtist: " + LeadArtist + Environment.NewLine;
            return result;
        }

        public bool isID3v2(byte[] fileData)
        {
            byteOffset += 3;
            return textEnconding.GetString(fileData, 0, 3).ToUpper() == "ID3";
        }

        public void Parse()
        {
            if (isID3v2(this.fileData))
            {
                ParseHeader();

                if (ContainsExtendedHeader)
                {
                    ExtendedHeader = new ExtendedHeader(this.fileData, byteOffset);
                    byteOffset = ExtendedHeader.Parse();
                }

                ParseFrames();
            }
        }

        private void ParseFrames()
        {
            if (fileData.Length < 18 || ContainsExtendedHeader)
            {
                return;
            }

            Frame frame = new Frame(fileData, byteOffset);
            byteOffset = frame.Parse();
            switch (frame.FrameId)
            {
                case "TPE1":
                    ParseLeadArtist();
                    break;
            } 
        }

        private void ParseLeadArtist()
        {
            int textDataSize = 12 - 1;
            LeadArtist = textEnconding.GetString(fileData, 21, textDataSize);
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
            byteOffset += 2;
        }

        private void ParseFlags()
        {
            byte flagByte = fileData[5];
            IsUnsynchronisationUsed = BitUtil.IsBitSet(flagByte, 7);
            ContainsExtendedHeader = BitUtil.IsBitSet(flagByte, 6);
            IsExperimentalStage = BitUtil.IsBitSet(flagByte, 5);
            byteOffset += 1;
        }

        private void ParseTagSize()
        {
            TagSize = (uint)(((fileData[6] & 0x7F) << 25) | ((fileData[7] & 0x7F) << 18) | ((fileData[8] & 0x7F) << 11) | ((fileData[9] & 0x7F) << 4)) >> 4;
            byteOffset += 4;
        }
    }
}
