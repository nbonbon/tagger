using System;
using System.Text;

namespace Tagger
{
    public class Frame
    {
        private const int FRAME_HEADER_ID_SIZE = 4;
        private const int FRAME_HEADER_SIZE_SIZE = 4;
        private const int FRAME_HEADER_FLAGS_SIZE = 2;
        private const int ENCODING_SIZE = 1;
        private const string USER_DEFINED_TEXT_INFO_FRAME_ID = "TXXX";

        private int byteOffset = -1;
        private byte[] fileData;
        private static Encoding textEnconding = Encoding.GetEncoding("iso-8859-1");

        public string FrameId { get; private set; }
        public uint Size { get; private set; }
        public bool TagAlterPreservation { get; private set; }
        public bool FileAlterPreservation { get; private set; }
        public bool Compression { get; private set; }
        public bool Encryption { get; private set; }
        public bool GroupIdentity { get; private set; }
        public bool ReadOnly { get; private set; }
        public string LeadArtist { get; private set; }

        public Frame(byte[] fileData, int byteOffset)
        {
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
                ParseEncoding();

                switch (FrameId)
                {
                    case "TPE1":
                        ParseLeadArtist();
                        break;
                    default:
                        byteOffset += (int)Size;
                        break;
                }
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

        private void ParseEncoding()
        {
            byteOffset += ENCODING_SIZE;
        }

        private void ParseLeadArtist()
        {
            int textSize = (int)Size - ENCODING_SIZE;
            LeadArtist = textEnconding.GetString(fileData, byteOffset, textSize);
            byteOffset += textSize;
        }
    }
}
