using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger;

namespace TaggerUnitTest
{
    [TestClass]
    public class ExtendedHeaderUnitTest
    {

        [TestMethod]
        public void ParseShouldParseExtendedHeaderSizeWhenNoCRC()
        {
            byte[] mockData = new byte[]
            {
                0x49, 0x44, 0x33, // "ID3"
                0x03, 0x01, // id3 version
                0x40, // id3 header flags (Extended header is present)
                0x00, 0x00, 0x02, 0x01, // id3 tag size
                0x00, 0x00, 0x00, 0x06, // Extended header size
                0x00, 0x00, // Extended header flags
                0x00, 0x00, 0x00, 0x00 // Size of padding
            };
            ExtendedHeader metadata = new ExtendedHeader(mockData, 10);
            int byteOffset = metadata.Parse();
            Assert.AreEqual((uint)06, metadata.ExtendedHeaderSize);
            Assert.AreEqual(10 + 10, byteOffset, "ByteOffset should have been the initial value plus the size of the extended header");
        }

        [TestMethod]
        public void ParseShouldParseExtendedHeaderSizeWhenCRC()
        {
            byte[] mockData = new byte[]
            {
                0x49, 0x44, 0x33, // "ID3"
                0x03, 0x01, // id3 version
                0x40, // id3 header flags (Extended header is present)
                0x00, 0x00, 0x02, 0x01, // id3 tag size
                0x00, 0x00, 0x00, 0x0A, // Extended header size
                0x80, 0x00, // Extended header flags
                0x00, 0x00, 0x00, 0x00, // Size of padding
                0x00, 0x00, 0x00, 0x00 // CRC
            };
            ExtendedHeader metadata = new ExtendedHeader(mockData, 10);
            int byteOffset = metadata.Parse();
            Assert.AreEqual((uint)10, metadata.ExtendedHeaderSize);
            Assert.AreEqual(10 + 14, byteOffset, "ByteOffset should have been the initial value plus the size of the extended header");
        }

        [TestMethod]
        public void ParseShouldParseExtendedHeaderCrcFlagAsFalseWhenBitFlagIsNotSet()
        {
            byte[] mockData = new byte[] 
            {
                0x49, 0x44, 0x33, // "ID3"
                0x03, 0x01, // id3 version
                0x40, // id3 header flags (Extended header is present)
                0x00, 0x00, 0x02, 0x01, // id3 tag size
                0x00, 0x00, 0x00, 0x06, // Extended header size
                0x7F, 0x00, // Extended header flags (All bits sits except of CRC flag)
                0x00, 0x00, 0x00, 0x00 // Size of padding
            };
            ExtendedHeader metadata = new ExtendedHeader(mockData, 10);;
            metadata.IsCrcDataPresent = true;
            metadata.Parse();
            Assert.AreEqual(false, metadata.IsCrcDataPresent);
        }

        [TestMethod]
        public void ParseShouldParseExtendedHeaderCrcFlagAsTrueWhenBitFlagIsSet()
        {
            byte[] mockData = new byte[] 
            {
                0x49, 0x44, 0x33, // "ID3"
                0x03, 0x01, // id3 version
                0x40, // id3 header flags (Extended header is present)
                0x00, 0x00, 0x02, 0x01, // id3 tag size
                0x00, 0x00, 0x00, 0x0A, // Extended header size
                0x80, 0x00, // Extended header flags (CRC flag bit set)
                0x00, 0x00, 0x00, 0x00, // Size of padding
                0x00, 0x00, 0x00, 0x00  // CRC
            };
            ExtendedHeader metadata = new ExtendedHeader(mockData, 10);;
            metadata.Parse();
            Assert.AreEqual(true, metadata.IsCrcDataPresent);
        }

        [TestMethod]
        public void ParseShouldParseSizeOfPadding()
        {
            byte[] mockData = new byte[] 
            {
                0x49, 0x44, 0x33, // "ID3"
                0x03, 0x01, // id3 version
                0x40, // id3 header flags (Extended header is present)
                0x00, 0x00, 0x02, 0x01, // id3 tag size
                0x00, 0x00, 0x00, 0x06, // Extended header size
                0x00, 0x00, // Extended header flags
                0x00, 0x00, 0x00, 0x20 // Size of padding
            };
            ExtendedHeader metadata = new ExtendedHeader(mockData, 10);;
            metadata.Parse();
            Assert.AreEqual((uint)32, metadata.SizeOfPadding);
        }
    }
}
