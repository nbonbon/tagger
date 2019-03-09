using System;
using System.Text;

namespace Tagger
{
    public class TextInfoFrameParser : ITextInfoFrameParser
    {
        private const int ENCODING_SIZE = 1;

        private int byteOffset = 0;
        private byte[] fileData = null;
        private uint frameSize = 0;
        private string frameId = "";
        private bool initialized = false;
        private static Encoding textEnconding = Encoding.GetEncoding("iso-8859-1");

        public void Initialize(byte[] fileData, int byteOffset, uint frameSize, string frameId)
        {
            this.fileData = fileData;
            this.byteOffset = byteOffset;
            this.frameSize = frameSize;
            this.frameId = frameId;
            initialized = true;
        }

        public int Parse(out TextInfoFrame frame)
        {
            if (!initialized)
            {
                throw new InvalidOperationException("TextInfoFrameParser must be initialized before Parse() can be called.");
            }

            frame = new TextInfoFrame();
            frame.FrameId = frameId;

            if (frameId != null && 
                frameId.Length > 0 &&
                frameId.ToUpper()[0] == 'T')
            {
                switch (frameId)
                {
                    case "TPE1":
                        ParseLeadArtist(ref frame);
                        byteOffset += (int)frameSize;
                        break;
                    default:
                        byteOffset += (int)frameSize;
                        break;
                }
            }
            else
            {
                byteOffset += (int)frameSize;
            }

            return byteOffset;
        }

        private void ParseLeadArtist(ref TextInfoFrame frame)
        {
            frame.Info = textEnconding.GetString(fileData, byteOffset, (int)frameSize - ENCODING_SIZE);
        }
    }
}