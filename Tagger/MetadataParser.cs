using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace Tagger
{
    public class MetadataParser
    {
        private readonly IFileSystem fileSystem;
        private readonly string filepath;

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
                    ParseHeader(metadata, fileData);

                    if (metadata.ContainsExtendedHeader)
                    {
                        metadata.ExtendedHeader = new ExtendedHeader();
                        ParseExtendedHeaderSize(metadata, fileData);
                    }
                }
            }

            return metadata;
        }

        private static void ParseHeader(Id3Metadata metadata, byte[] fileData)
        {
            ParseVersion(metadata, fileData);
            ParseFlags(metadata, fileData);
            ParseTagSize(metadata, fileData);
        }

        private static bool isID3v2(byte[] fileData)
        {
            return System.Text.Encoding.ASCII.GetString(fileData, 0, 3).ToUpper() == "ID3";
        }

        public static void ParseVersion(Id3Metadata metadata, byte[] fileData)
        {
            metadata.VersionMajor = 2;
            metadata.VersionMinor = fileData[3];
            metadata.VersionRevision = fileData[4];
        }

        public static void ParseFlags(Id3Metadata metadata, byte[] fileData)
        {
            byte flagByte = fileData[5];
            metadata.IsUnsynchronisationUsed = IsBitSet(flagByte, 7);
            metadata.ContainsExtendedHeader = IsBitSet(flagByte, 6);
            metadata.IsExperimentalStage = IsBitSet(flagByte, 5);
        }

        private static bool IsBitSet(byte byteToSearch, int bitNumberToCheck)
        {
            return (byteToSearch & (1 << bitNumberToCheck)) != 0;
        }

        public static void ParseTagSize(Id3Metadata metadata, byte[] fileData)
        {
            metadata.TagSize = (uint)(((fileData[6] & 0x7F) << 25) | ((fileData[7] & 0x7F) << 18) | ((fileData[8] & 0x7F) << 11) | ((fileData[9] & 0x7F) << 4)) >> 4;
        }

        public static void ParseExtendedHeaderSize(Id3Metadata metadata, byte[] fileData)
        {
            if (metadata.ExtendedHeader != null)
            {
                metadata.ExtendedHeader.ExtendedHeaderSize = (uint)((uint)fileData[10] << 24) | ((uint)fileData[11] << 16) | ((uint)fileData[12] << 8) | (uint)fileData[13]; 
            }
        }
    }
}