using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger;

namespace TaggerUnitTest
{
    [TestClass]
    public class Id3MetadataUnitTest
    {
        [TestMethod]
        public void ParseShouldParseVersion()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.Parse();
            Assert.AreEqual("2.3.1", metadata.Version);
        }

        [TestMethod]
        public void ParseShouldParseUnsynchronizationFlagAsTrueIfSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x80, 0x00, 0x00, 0x00, 0x00 };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.Parse();
            Assert.IsTrue(metadata.IsUnsynchronisationUsed);
        }

        [TestMethod]
        public void ParseShouldParseUnsynchronizationFlagAsFalseIfNotSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.IsUnsynchronisationUsed = true;
            metadata.Parse();
            Assert.IsFalse(metadata.IsUnsynchronisationUsed);
        }

        [TestMethod]
        public void ParseShouldParseExtendedHeaderFlagAsTrueIfSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.Parse();
            Assert.IsTrue(metadata.ContainsExtendedHeader);
        }

        [TestMethod]
        public void ParseShouldParseExtendedHeaderFlagAsFalseIfNotSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.IsUnsynchronisationUsed = true;
            metadata.Parse();
            Assert.IsFalse(metadata.ContainsExtendedHeader);
            Assert.AreEqual(null, metadata.ExtendedHeader);
        }

        [TestMethod]
        public void ParseShouldParseExperimentalIndicatorFlagAsTrueIfSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x20, 0x00, 0x00, 0x00, 0x00 };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.Parse();
            Assert.IsTrue(metadata.IsExperimentalStage);
        }

        [TestMethod]
        public void ParseShouldParseExperimentalIndicatorFlagAsFalseIfNotSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x1F, 0x00, 0x00, 0x00, 0x00 };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.IsUnsynchronisationUsed = true;
            metadata.Parse();
            Assert.IsFalse(metadata.IsExperimentalStage);
        }

        [TestMethod]
        public void ParseShouldParseTagSize()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x1F, 0x00, 0x00, 0x02, 0x01 };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.Parse();
            Assert.AreEqual((uint)257, metadata.TagSize);
        }

        [TestMethod]
        public void ParseShouldParseArtistWhenArtistTagIsFirstTagAfterHeader()
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
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.Parse();
            Assert.AreEqual("Rick Astley", metadata.LeadArtist);
        }
    }
}
