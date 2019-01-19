using System;
using System.IO;
using System.IO.Abstractions;

namespace Tagger
{
    public class MetadataParser
    {
        readonly IFileSystem fileSystem;
        readonly string filepath;

        public MetadataParser(IFileSystem fileSystem, string filepath)
        {
            this.fileSystem = fileSystem;
            this.filepath = filepath;
        }

        public MetadataParser(string filepath) : this(fileSystem: new FileSystem(), filepath: filepath)
        {
        }

        public Id3Metadata Parse()
        {
            Id3Metadata metadata = null;

            if (this.fileSystem.File.Exists(this.filepath))
            {
                byte[] fileData = this.fileSystem.File.ReadAllBytes(this.filepath);

                if (isID3v2(fileData))
                {
                    metadata = new Id3Metadata();
                    ParseVersion(metadata, fileData);
                    ParseFlags(metadata, fileData);
                }
            }

            return metadata;
        }

        private static void ParseFlags(Id3Metadata metadata, byte[] fileData)
        {
            byte flagByte = fileData[5];
            metadata.isUnsynchronisationUsed = IsBitSet(flagByte, 7);
            metadata.containsExtendedHeader = IsBitSet(flagByte, 6);
            metadata.isExperimentalStage = IsBitSet(flagByte, 5);
        }

        private static bool IsBitSet(byte byteToSearch, int bitNumberToCheck)
        {
            return (byteToSearch & (1 << bitNumberToCheck)) != 0;
        }

        private static void ParseVersion(Id3Metadata metadata, byte[] fileData)
        {
            metadata.VersionMajor = 2;
            metadata.VersionMinor = fileData[3];
            metadata.VersionRevision = fileData[4];
        }

        private static bool isID3v2(byte[] fileData)
        {
            return System.Text.Encoding.ASCII.GetString(fileData, 0, 3).ToUpper() == "ID3";
        }
    }
}