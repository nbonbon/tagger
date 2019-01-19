using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Abstractions;
using Tagger;

namespace TaggerUnitTest
{
    [TestClass]
    public class MetadataParserUnitTest
    {
        [TestMethod]
        public void ParseShouldParseVersion()
        {
            // Arrange
            const string mockFilepath = @"c:\test.mp3";
            byte[] mockFileData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x00 };
            Moq.Mock<IFileSystem> filesystemMock = new Moq.Mock<IFileSystem>();
            filesystemMock.Setup(x => x.File.ReadAllBytes(mockFilepath)).Returns(mockFileData);
            filesystemMock.Setup(x => x.File.Exists(mockFilepath)).Returns(true);

            MetadataParser parser = new MetadataParser(filesystemMock.Object, mockFilepath);
            Id3Metadata metadata = parser.Parse();
            Assert.AreEqual("2.3.1", metadata.Version);
        }

        [TestMethod]
        public void ParseShouldParseUnsynchronizationFlagAsTrueIfSet()
        {
            // Arrange
            const string mockFilepath = @"c:\test.mp3";
            byte[] mockFileData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x80 };
            Moq.Mock<IFileSystem> filesystemMock = new Moq.Mock<IFileSystem>();
            filesystemMock.Setup(x => x.File.ReadAllBytes(mockFilepath)).Returns(mockFileData);
            filesystemMock.Setup(x => x.File.Exists(mockFilepath)).Returns(true);

            MetadataParser parser = new MetadataParser(filesystemMock.Object, mockFilepath);
            Id3Metadata metadata = parser.Parse();
            Assert.IsTrue(metadata.isUnsynchronisationUsed);
        }

        [TestMethod]
        public void ParseShouldParseUnsynchronizationFlagAsFalseIfNotSet()
        {
            // Arrange
            const string mockFilepath = @"c:\test.mp3";
            byte[] mockFileData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x70 };
            Moq.Mock<IFileSystem> filesystemMock = new Moq.Mock<IFileSystem>();
            filesystemMock.Setup(x => x.File.ReadAllBytes(mockFilepath)).Returns(mockFileData);
            filesystemMock.Setup(x => x.File.Exists(mockFilepath)).Returns(true);

            MetadataParser parser = new MetadataParser(filesystemMock.Object, mockFilepath);
            Id3Metadata metadata = parser.Parse();
            Assert.IsFalse(metadata.isUnsynchronisationUsed);
        }

        [TestMethod]
        public void ParseShouldParseExtendedHeaderFlagAsTrueIfSet()
        {
            // Arrange
            const string mockFilepath = @"c:\test.mp3";
            byte[] mockFileData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x40 };
            Moq.Mock<IFileSystem> filesystemMock = new Moq.Mock<IFileSystem>();
            filesystemMock.Setup(x => x.File.ReadAllBytes(mockFilepath)).Returns(mockFileData);
            filesystemMock.Setup(x => x.File.Exists(mockFilepath)).Returns(true);

            MetadataParser parser = new MetadataParser(filesystemMock.Object, mockFilepath);
            Id3Metadata metadata = parser.Parse();
            Assert.IsTrue(metadata.containsExtendedHeader);
        }

        [TestMethod]
        public void ParseShouldParseExtendedHeaderFlagAsFalseIfNotSet()
        {
            // Arrange
            const string mockFilepath = @"c:\test.mp3";
            byte[] mockFileData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x30 };
            Moq.Mock<IFileSystem> filesystemMock = new Moq.Mock<IFileSystem>();
            filesystemMock.Setup(x => x.File.ReadAllBytes(mockFilepath)).Returns(mockFileData);
            filesystemMock.Setup(x => x.File.Exists(mockFilepath)).Returns(true);

            MetadataParser parser = new MetadataParser(filesystemMock.Object, mockFilepath);
            Id3Metadata metadata = parser.Parse();
            Assert.IsFalse(metadata.containsExtendedHeader);
        }

        [TestMethod]
        public void ParseShouldParseExperimentalIndicatorFlagAsTrueIfSet()
        {
            // Arrange
            const string mockFilepath = @"c:\test.mp3";
            byte[] mockFileData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x20 };
            Moq.Mock<IFileSystem> filesystemMock = new Moq.Mock<IFileSystem>();
            filesystemMock.Setup(x => x.File.ReadAllBytes(mockFilepath)).Returns(mockFileData);
            filesystemMock.Setup(x => x.File.Exists(mockFilepath)).Returns(true);

            MetadataParser parser = new MetadataParser(filesystemMock.Object, mockFilepath);
            Id3Metadata metadata = parser.Parse();
            Assert.IsTrue(metadata.isExperimentalStage);
        }

        [TestMethod]
        public void ParseShouldParseExperimentalIndicatorFlagAsFalseIfNotSet()
        {
            // Arrange
            const string mockFilepath = @"c:\test.mp3";
            byte[] mockFileData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x01, 0x1F };
            Moq.Mock<IFileSystem> filesystemMock = new Moq.Mock<IFileSystem>();
            filesystemMock.Setup(x => x.File.ReadAllBytes(mockFilepath)).Returns(mockFileData);
            filesystemMock.Setup(x => x.File.Exists(mockFilepath)).Returns(true);

            MetadataParser parser = new MetadataParser(filesystemMock.Object, mockFilepath);
            Id3Metadata metadata = parser.Parse();
            Assert.IsFalse(metadata.isExperimentalStage);
        }
    }
}
