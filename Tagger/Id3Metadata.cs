using System;
using System.Collections.Generic;
using System.Text;

namespace Tagger
{
    public class Id3Metadata
    {
        private byte[] fileData;
        private int byteOffset = -1;
        private static Encoding textEnconding = Encoding.GetEncoding("iso-8859-1");

        public byte VersionMajor { get; private set; }
        public byte VersionMinor { get; private set; }
        public byte VersionRevision { get; private set; }
        public string TagId { get; private set; }
        public Dictionary<string, Frame> Frames { get; private set; }

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

        /// <summary>
        /// The ID3v2 tag size is the size of the complete tag after unsychronisation, including padding, excluding the
        /// header but not excluding the extended header(total tag size - 10).
        /// </summary>
        public uint TagSize { get; set; }
        public ExtendedHeader ExtendedHeader { get; set; }

        public Id3Metadata(byte[] fileData)
        {
            this.fileData = fileData;
            byteOffset = 0;
            Frames = new Dictionary<string, Frame>();
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
            return result;
        }

        public bool isID3v2(byte[] fileData)
        {
            byteOffset += 3;
            TagId = textEnconding.GetString(fileData, 0, 3).ToUpper();
            return (TagId == "ID3");
        }

        public bool ValidateTag()
        {
            return (fileData[6] < 0x80 && fileData[7] < 0x80 && fileData[8] < 0x80 && fileData[9] < 0x80) &&
                (fileData[3] < 0xFF && fileData[4] < 0xFF);
        }

        public void Parse()
        {
            if (isID3v2(this.fileData))
            {
                ParseHeader();

                if (ValidateTag())
                {
                    if (ContainsExtendedHeader)
                    {
                        ExtendedHeader = new ExtendedHeader(this.fileData, byteOffset);
                        byteOffset = ExtendedHeader.Parse();
                    }

                    ParseFrames();
                }
            }
        }

        private void ParseFrames()
        {
            if (fileData.Length < 18 || ContainsExtendedHeader)
            {
                return;
            }

            for (int i = byteOffset; i < TagSize;)
            {
                Frame frame = new Frame(fileData, byteOffset);
                i = frame.Parse();
                byteOffset = i;
                //Console.WriteLine(frame.ToString());

                if (frame.FrameId != "\0\0\0\0")
                {
                    Frames.Add(frame.FrameId, frame);

                    Console.WriteLine(frame.FrameId);

                    if (frame.IsTextInfoFrame)
                    {
                        Console.WriteLine(frame.TextInfoData);
                    }

                }
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
