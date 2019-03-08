using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger;

namespace TaggerUnitTest
{
    [TestClass]
    public class FrameUnitTest
    {
        [TestMethod]
        public void ParseShouldParseFrameId()
        {
            byte[] mockData = new byte[]
            {
                0x49, 0x44, 0x33, // "ID3"
                0x03, 0x01, // id3 version
                0x1F, // id3 header flags
                0x00, 0x00, 0x02, 0x01, // id3 tag size
                0x54, 0x50, 0x45, 0x31, // frame id: TPE1
                0x00, 0x00, 0x00, 0x0C, // frame size
                0x00, 0x00, // frame flags
                0x00, // text frame encoding
                0x52, 0x69, 0x63, 0x6B, 0x20, 0x41, 0x73, 0x74, 0x6C, 0x65, 0x79 // text frame data: "Rick Astley"
            };
            Frame frame = new Frame(mockData, 10);
            frame.Parse();
            Assert.AreEqual("TPE1", frame.FrameId);
        }

        [TestMethod]
        public void ParseShouldParseFrameSize()
        {
            byte[] mockData = new byte[]
            {
                0x49, 0x44, 0x33, // "ID3"
                0x03, 0x01, // id3 version
                0x1F, // id3 header flags
                0x00, 0x00, 0x02, 0x01, // id3 tag size
                0x54, 0x50, 0x45, 0x31, // frame id: TPE1
                0x00, 0x00, 0x00, 0x0C, // frame size: 12 (big endian)
                0x00, 0x00, // frame flags
                0x00, // text frame encoding
                0x52, 0x69, 0x63, 0x6B, 0x20, 0x41, 0x73, 0x74, 0x6C, 0x65, 0x79 // text frame data: "Rick Astley"
            };
            Frame frame = new Frame(mockData, 10);
            frame.Parse();
            Assert.AreEqual(12, frame.Size);
        }
    }
}
