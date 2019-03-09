using System;
using System.Text;

namespace Tagger
{
    public class Frame
    {
        private const int FRAME_HEADER_ID_SIZE = 4;
        private const int FRAME_HEADER_SIZE_SIZE = 4;
        private const int FRAME_HEADER_FLAGS_SIZE = 2;
        private const string USER_DEFINED_TEXT_INFO_FRAME_ID = "TXXX";

        private int byteOffset = -1;
        private byte[] fileData;
        private ITextInfoFrameParser textInfoFrameParser;
        private static Encoding textEnconding = Encoding.GetEncoding("iso-8859-1");

        public string FrameId { get; set; }
        public uint Size { get; set; }
        public bool TagAlterPreservation { get; set; }
        public bool FileAlterPreservation { get; set; }
        public bool Compression { get; set; }
        public bool Encryption { get; set; }
        public bool GroupIdentity { get; set; }
        public bool ReadOnly { get; set; }

        public Frame(byte[] fileData, int byteOffset)
        {
            this.textInfoFrameParser = new TextInfoFrameParser();
            Initialize(fileData, byteOffset);
        }

        public Frame(byte[] fileData, int byteOffset, ITextInfoFrameParser textInfoFrameParser) 
        {
            this.textInfoFrameParser = textInfoFrameParser;
            Initialize(fileData, byteOffset);
        }

        private void Initialize(byte[] fileData, int byteOffset)
        {
            this.fileData = fileData;
            this.byteOffset = byteOffset;

            TagAlterPreservation = Tagger.TagAlterPreservation.Preserved;
            FileAlterPreservation = Tagger.FileAlterPreservation.Preserved;
            Compression = Tagger.Compression.Uncompressed;
            Encryption = Tagger.Encryption.Unencrypted;
            GroupIdentity = Tagger.GroupIdentity.DoesNotContainGroupIdentity;
            ReadOnly = false;
        }

        public override string ToString()
        {
            string result = string.Empty;
            result += "Frame Id: " + FrameId + Environment.NewLine;
            result += "Frame Size: " + Size + Environment.NewLine;
            result += "Frame TagAlterPreservation: " + TagAlterPreservation + Environment.NewLine;
            result += "Frame FileAlterPreservation: " + FileAlterPreservation + Environment.NewLine;
            result += "Frame Compression: " + Compression + Environment.NewLine;
            result += "Frame Encryption: " + Encryption + Environment.NewLine;
            result += "Frame GroupIdentity: " + GroupIdentity + Environment.NewLine;
            result += "Frame ReadOnly: " + ReadOnly + Environment.NewLine;
            return result;
        }

        public int Parse()
        {
            ParseFrameHeader();
            ParseFrameBody();
            return byteOffset;
        }

        private void ParseFrameHeader()
        {
            ParseFrameId();
            ParseFrameSize();
            ParseFrameFlags();
        }

        private void ParseFrameBody()
        {
            if (FrameId.ToUpper()[0] == 'T')
            {
                TextInfoFrame frame;
                textInfoFrameParser.Initialize(fileData, byteOffset, Size, FrameId);
                byteOffset = textInfoFrameParser.Parse(out frame);
            }
            else
            {
                byteOffset += (int)Size;
            }
        }

        private void ParseFrameFlags()
        {
            TagAlterPreservation = BitUtil.IsBitSet(fileData[byteOffset], 7);
            FileAlterPreservation = BitUtil.IsBitSet(fileData[byteOffset], 6);
            ReadOnly = BitUtil.IsBitSet(fileData[byteOffset], 5);

            Compression = BitUtil.IsBitSet(fileData[byteOffset + 1], 7);
            Encryption = BitUtil.IsBitSet(fileData[byteOffset + 1], 6);
            GroupIdentity = BitUtil.IsBitSet(fileData[byteOffset + 1], 5);

            byteOffset += FRAME_HEADER_FLAGS_SIZE;
        }

        private void ParseFrameSize()
        {
            Size = BitUtil.ParseBigEndianUint32(fileData, byteOffset);
            byteOffset += FRAME_HEADER_SIZE_SIZE;
        }

        private void ParseFrameId()
        {
            FrameId = textEnconding.GetString(fileData, byteOffset, FRAME_HEADER_ID_SIZE).ToUpper();
            byteOffset += FRAME_HEADER_ID_SIZE;
        }
    }
}
