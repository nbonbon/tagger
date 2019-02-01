using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger;

namespace TaggerUnitTest
{
    [TestClass]
    public class ExtendedHeaderUnitTest
    {
        [TestMethod]
        public void ParseShouldParseExtendedHeaderSize()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x40, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            ExtendedHeader metadata = new ExtendedHeader(mockData, 10);
            metadata.Parse();
            Assert.AreEqual((uint)10, metadata.ExtendedHeaderSize);
        }

        [TestMethod]
        public void ParseShouldParseExtendedHeaderCrcFlagAsFalseWhenBitFlagIsNotSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x40, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x0A, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00 };
            ExtendedHeader metadata = new ExtendedHeader(mockData, 10);;
            metadata.IsCrcDataPresent = true;
            metadata.Parse();
            Assert.AreEqual(false, metadata.IsCrcDataPresent);
        }

        [TestMethod]
        public void ParseShouldParseExtendedHeaderCrcFlagAsTrueWhenBitFlagIsSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x40, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x0A, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00 };
            ExtendedHeader metadata = new ExtendedHeader(mockData, 10);;
            metadata.Parse();
            Assert.AreEqual(true, metadata.IsCrcDataPresent);
        }

        [TestMethod]
        public void ParseShouldParseSizeOfPadding()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x40, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x0A, 0x80, 0x00, 0x00, 0x00, 0x00, 0x20 };
            ExtendedHeader metadata = new ExtendedHeader(mockData, 10);;
            metadata.Parse();
            Assert.AreEqual((uint)32, metadata.SizeOfPadding);
        }
    }
}
