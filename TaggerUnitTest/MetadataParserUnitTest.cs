using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Abstractions;
using Tagger;

namespace TaggerUnitTest
{
    [TestClass]
    public class MetadataParserUnitTest
    {
        [TestMethod]
        public void ParseShouldParseAllMetadata()
        {
            const string mockFilepath = @"c:\test.mp3";
            byte[] mockFileData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x00, 0xE0, 0x00, 0x00, 0x02, 0x01 };
            Moq.Mock<IFileSystem> filesystemMock = new Moq.Mock<IFileSystem>();
            filesystemMock.Setup(x => x.File.ReadAllBytes(mockFilepath)).Returns(mockFileData);
            filesystemMock.Setup(x => x.File.Exists(mockFilepath)).Returns(true);

            MetadataParser parser = new MetadataParser(filesystemMock.Object, mockFilepath);
            Id3Metadata metadata = parser.Parse();
            Assert.AreEqual("2.3.0", metadata.Version);
            Assert.IsTrue(metadata.IsUnsynchronisationUsed);
            Assert.IsTrue(metadata.ContainsExtendedHeader);
            Assert.IsTrue(metadata.IsExperimentalStage);
            Assert.AreEqual(257, metadata.TagSize);
        }

        [TestMethod]
        public void ParseVersionShouldParseVersion()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01};
            Id3Metadata metadata = new Id3Metadata();
            MetadataParser.ParseVersion(metadata, mockData);
            Assert.AreEqual("2.3.1", metadata.Version);
        }

        [TestMethod]
        public void ParseFlagsShouldParseUnsynchronizationFlagAsTrueIfSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x80 };
            Id3Metadata metadata = new Id3Metadata();
            MetadataParser.ParseFlags(metadata, mockData);
            Assert.IsTrue(metadata.IsUnsynchronisationUsed);
        }

        [TestMethod]
        public void ParseFlagsShouldParseUnsynchronizationFlagAsFalseIfNotSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x70 };
            Id3Metadata metadata = new Id3Metadata();
            metadata.IsUnsynchronisationUsed = true;
            MetadataParser.ParseFlags(metadata, mockData);
            Assert.IsFalse(metadata.IsUnsynchronisationUsed);
        }

        [TestMethod]
        public void ParseFlagsShouldParseExtendedHeaderFlagAsTrueIfSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x40 };
            Id3Metadata metadata = new Id3Metadata();
            MetadataParser.ParseFlags(metadata, mockData);
            Assert.IsTrue(metadata.ContainsExtendedHeader);
        }

        [TestMethod]
        public void ParseFlagsShouldParseExtendedHeaderFlagAsFalseIfNotSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x30 };
            Id3Metadata metadata = new Id3Metadata();
            metadata.IsUnsynchronisationUsed = true;
            MetadataParser.ParseFlags(metadata, mockData);
            Assert.IsFalse(metadata.ContainsExtendedHeader);
        }

        [TestMethod]
        public void ParseFlagsShouldParseExperimentalIndicatorFlagAsTrueIfSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x20 };
            Id3Metadata metadata = new Id3Metadata();
            MetadataParser.ParseFlags(metadata, mockData);
            Assert.IsTrue(metadata.IsExperimentalStage);
        }

        [TestMethod]
        public void ParseFlagsShouldParseExperimentalIndicatorFlagAsFalseIfNotSet()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x1F };
            Id3Metadata metadata = new Id3Metadata();
            metadata.IsUnsynchronisationUsed = true;
            MetadataParser.ParseFlags(metadata, mockData);
            Assert.IsFalse(metadata.IsExperimentalStage);
        }

        [TestMethod]
        public void ParseSizeShouldParseTagSize()
        {
            byte[] mockData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x1F, 0x00, 0x00, 0x02, 0x01 };
            Id3Metadata metadata = new Id3Metadata();
            MetadataParser.ParseTagSize(metadata, mockData);
            Assert.AreEqual(257, metadata.TagSize);
        }
    }
}
