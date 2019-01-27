using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger;

namespace TaggerUnitTest
{
    [TestClass]
    public class ExtendedHeaderUnitTest
    {
        [TestMethod]
        public void ParseExtendedHeaderSizeShouldParseExtendedHeaderSize()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x40, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x0A };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.Parse();
            Assert.AreEqual((uint)10, metadata.ExtendedHeader.ExtendedHeaderSize);
        }

    }
}
