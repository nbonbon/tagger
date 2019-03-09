using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger;
using System;

namespace TaggerUnitTest
{
    [TestClass]
    public class TextInfoFrameParserUnitTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Call to Parse was inappropriately allowed.")]
        public void WhenParseIsCalledBeforeInitialize_ParseShouldThrowAnInvalidOperationException()
        {
            TextInfoFrameParser textInfoParser = new TextInfoFrameParser();
            TextInfoFrame frame;
            textInfoParser.Parse(out frame);
        }

        [TestMethod]
        public void WhenParseIsCalledWithTPE1FrameId_ParseShouldParseTheLeadArtist()
        {
            byte[] mockData = new byte[]
            {
                0x49, 0x44, 0x33, // "ID3"
                0x03, 0x01, // id3 version
                0x1F, // id3 header flags
                0x00, 0x00, 0x02, 0x01, // id3 tag size
                0x54, 0x50, 0x45, 0x31, // frame id: TPE1 [10]
                0x00, 0x00, 0x00, 0x0C, // frame size: 12 (big endian) [14]
                0x00, 0x00, // frame flags [18]
                0x00, // text frame encoding [20]
                0x52, 0x69, 0x63, 0x6B, 0x20, 0x41, 0x73, 0x74, 0x6C, 0x65, 0x79 // text frame data: "Rick Astley" [21]
            };

            TextInfoFrameParser textInfoParser = new TextInfoFrameParser();
            textInfoParser.Initialize(mockData, 21, 12, "TPE1");
            TextInfoFrame frame;
            textInfoParser.Parse(out frame);
            Assert.AreEqual("Rick Astley", frame.Info);
            Assert.AreEqual("TPE1", frame.FrameId);
        }
    }
}
