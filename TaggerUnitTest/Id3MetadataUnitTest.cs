using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger;

namespace TaggerUnitTest
{
    [TestClass]
    public class Id3MetadataUnitTest
    {
        [TestMethod]
        public void ParseVersionShouldParseVersion()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.Parse();
            Assert.AreEqual("2.3.1", metadata.Version);
        }

        [TestMethod]
        public void ParseFlagsShouldParseUnsynchronizationFlagAsTrueIfSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x80, 0x00, 0x00, 0x00, 0x00 };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.Parse();
            Assert.IsTrue(metadata.IsUnsynchronisationUsed);
        }

        [TestMethod]
        public void ParseFlagsShouldParseUnsynchronizationFlagAsFalseIfNotSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.IsUnsynchronisationUsed = true;
            metadata.Parse();
            Assert.IsFalse(metadata.IsUnsynchronisationUsed);
        }

        [TestMethod]
        public void ParseFlagsShouldParseExtendedHeaderFlagAsTrueIfSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.Parse();
            Assert.IsTrue(metadata.ContainsExtendedHeader);
        }

        [TestMethod]
        public void ParseFlagsShouldParseExtendedHeaderFlagAsFalseIfNotSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.IsUnsynchronisationUsed = true;
            metadata.Parse();
            Assert.IsFalse(metadata.ContainsExtendedHeader);
            Assert.AreEqual(null, metadata.ExtendedHeader);
        }

        [TestMethod]
        public void ParseFlagsShouldParseExperimentalIndicatorFlagAsTrueIfSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x20, 0x00, 0x00, 0x00, 0x00 };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.Parse();
            Assert.IsTrue(metadata.IsExperimentalStage);
        }

        [TestMethod]
        public void ParseFlagsShouldParseExperimentalIndicatorFlagAsFalseIfNotSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x1F, 0x00, 0x00, 0x00, 0x00 };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.IsUnsynchronisationUsed = true;
            metadata.Parse();
            Assert.IsFalse(metadata.IsExperimentalStage);
        }

        [TestMethod]
        public void ParseSizeShouldParseTagSize()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x1F, 0x00, 0x00, 0x02, 0x01 };
            Id3Metadata metadata = new Id3Metadata(mockData);
            metadata.Parse();
            Assert.AreEqual((uint)257, metadata.TagSize);
        }
    }
}
