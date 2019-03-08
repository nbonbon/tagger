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

        private byte[] fileData;
        private static Encoding textEnconding = Encoding.GetEncoding("iso-8859-1");

        public string FrameId { get; set; }
        public int Size { get; set; }

        public Frame(byte[] fileData, int byteOffset)
        {
            this.fileData = fileData;
            this.byteOffset = byteOffset;
        }

        public override string ToString()
        {
            string result = string.Empty;
            result += "Frame Id: " + FrameId + Environment.NewLine;
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
            //TO DO: Implement Me
            byteOffset += 2;
        }

        private void ParseFrameSize()
        {
            byte[] sizeBytes = new byte[4];
            Buffer.BlockCopy(fileData, byteOffset, sizeBytes, 0, 4);
            Array.Reverse(sizeBytes);
            Size = BitConverter.ToInt32(sizeBytes, 0);
            byteOffset += FRAME_SIZE_SIZE;
        }

        private void ParseFrameId()
        {
            FrameId = textEnconding.GetString(fileData, byteOffset, FRAME_ID_SIZE).ToUpper();
            byteOffset += FRAME_ID_SIZE;
        }
    }
}
