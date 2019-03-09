using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagger
{
    public class Frame
    {
        private int byteOffset = -1;
        private const int FRAME_ID_SIZE = 4;
        private const int FRAME_SIZE_SIZE = 4;
        private const int FRAME_HEADER_SIZE = 2;
        private byte[] fileData;
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
            return byteOffset;
        }

        private void ParseFrameHeader()
        {
            ParseFrameId();
            ParseFrameSize();
            ParseFrameFlags();
        }

        private void ParseFrameFlags()
        {
            TagAlterPreservation = BitUtil.IsBitSet(fileData[byteOffset], 7);
            FileAlterPreservation = BitUtil.IsBitSet(fileData[byteOffset], 6);
            ReadOnly = BitUtil.IsBitSet(fileData[byteOffset], 5);

            Compression = BitUtil.IsBitSet(fileData[byteOffset + 1], 7);
            Encryption = BitUtil.IsBitSet(fileData[byteOffset + 1], 6);
            GroupIdentity = BitUtil.IsBitSet(fileData[byteOffset + 1], 5);

            byteOffset += FRAME_HEADER_SIZE;
        }

        private void ParseFrameSize()
        {
            Size = BitUtil.ParseBigEndianUint32(fileData, byteOffset);
            byteOffset += FRAME_SIZE_SIZE;
        }

        private void ParseFrameId()
        {
            FrameId = textEnconding.GetString(fileData, byteOffset, FRAME_ID_SIZE).ToUpper();
            byteOffset += FRAME_ID_SIZE;
        }
    }
}
