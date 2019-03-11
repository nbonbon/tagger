using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger;
using Moq;

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
                0x54, 0x50, 0x45, 0x31, // frame id: TPE1 [10]
                0x00, 0x00, 0x00, 0x0C, // frame size: 12 (big endian) [14]
                0x00, 0x00, // frame flags [18]
                0x00, // text frame encoding [20]
                0x52, 0x69, 0x63, 0x6B, 0x20, 0x41, 0x73, 0x74, 0x6C, 0x65, 0x79 // text frame data: "Rick Astley" [21]
            };
            Frame frame = new Frame(mockData, 10);
            int byteOffset = frame.Parse();
            Assert.AreEqual("TPE1", frame.FrameId);
            Assert.AreEqual(10 + 10 + 12, byteOffset, "ByteOffset should have been the initial value plus the size of the tag header plus the size of the tag data");
        }

        [TestMethod]
        public void ParseShouldParseFrameHeaderFrameSize()
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
            Assert.AreEqual((uint)12, frame.Size);
        }

        [TestMethod]
        public void ParseShouldParseFrameHeaderTags()
        {
            byte[] mockData = new byte[]
            {
                0x49, 0x44, 0x33, // "ID3"
                0x03, 0x01, // id3 version
                0x1F, // id3 header flags
                0x00, 0x00, 0x02, 0x01, // id3 tag size
                0x54, 0x50, 0x45, 0x31, // frame id: TPE1
                0x00, 0x00, 0x00, 0x0C, // frame size: 12 (big endian)
                0xA0, 0xA0, // frame flags
                0x00, // text frame encoding
                0x52, 0x69, 0x63, 0x6B, 0x20, 0x41, 0x73, 0x74, 0x6C, 0x65, 0x79 // text frame data: "Rick Astley"
            };
            Frame frame = new Frame(mockData, 10);
            frame.Parse();
            Assert.AreEqual(TagAlterPreservation.Discarded, frame.TagAlterPreservation);
            Assert.AreEqual(FileAlterPreservation.Preserved, frame.FileAlterPreservation);
            Assert.AreEqual(true, frame.ReadOnly);

            Assert.AreEqual(Compression.Compressed, frame.Compression);
            Assert.AreEqual(Encryption.Unencrypted, frame.Encryption);
            Assert.AreEqual(GroupIdentity.ContainsGroupIdentity, frame.GroupIdentity);
        }

        [TestMethod]
        public void WhenParseIsCalledWithTextFrameId_ParseShouldEncoding()
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
                0x03, // text frame encoding [20]
                0x52, 0x69, 0x63, 0x6B, 0x20, 0x41, 0x73, 0x74, 0x6C, 0x65, 0x79 // text frame data: "Rick Astley" [21]
            };

            Frame frame = new Frame(mockData, 10);
            frame.Parse();
            Assert.AreEqual(TextEncoding.UTF_8, frame.Encoding);
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

            Frame frame = new Frame(mockData, 10);
            frame.Parse();
            Assert.AreEqual("Rick Astley", frame.TextInfoData);
        }
    }
}
