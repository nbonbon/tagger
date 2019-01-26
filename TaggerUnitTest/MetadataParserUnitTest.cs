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
            byte[] mockFileData = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x00, 0xE0, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x0A };
            Moq.Mock<IFileSystem> filesystemMock = new Moq.Mock<IFileSystem>();
            filesystemMock.Setup(x => x.File.ReadAllBytes(mockFilepath)).Returns(mockFileData);
            filesystemMock.Setup(x => x.File.Exists(mockFilepath)).Returns(true);

            MetadataParser parser = new MetadataParser(filesystemMock.Object, mockFilepath);
            Id3Metadata metadata = parser.Parse();
            Assert.AreEqual("2.3.0", metadata.Version);
            Assert.IsTrue(metadata.IsUnsynchronisationUsed);
            Assert.IsTrue(metadata.ContainsExtendedHeader);
            Assert.IsTrue(metadata.IsExperimentalStage);
            Assert.AreEqual((uint)257, metadata.TagSize);
            Assert.AreEqual((uint)10, metadata.ExtendedHeader.ExtendedHeaderSize);
        }
    }
}
