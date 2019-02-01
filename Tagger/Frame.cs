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

        private byte[] fileData;
        private static Encoding textEnconding = Encoding.GetEncoding("iso-8859-1");

        public string FrameId { get; set; }

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
            //TO DO: Implement Me
            byteOffset += 4;
        }

        private void ParseFrameId()
        {
            FrameId = textEnconding.GetString(fileData, byteOffset, FRAME_ID_SIZE).ToUpper();
            byteOffset += FRAME_ID_SIZE;
        }
    }
}
