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
        private static double[] tagSizeBitKey = null;

        public MetadataParser(IFileSystem fileSystem, string filepath)
        {
            this.fileSystem = fileSystem;
            this.filepath = filepath;
            PopulateTagSizeBitKey();
        }

        public MetadataParser(string filepath) : this(fileSystem: new FileSystem(), filepath: filepath)
        {
        }

        private static void PopulateTagSizeBitKey()
        {
            if (tagSizeBitKey != null)
            {
                return;
            }

            tagSizeBitKey = new double[32];
            tagSizeBitKey[0] = Math.Pow(2, 21);
            tagSizeBitKey[1] = Math.Pow(2, 22);
            tagSizeBitKey[2] = Math.Pow(2, 23);
            tagSizeBitKey[3] = Math.Pow(2, 24);
            tagSizeBitKey[4] = Math.Pow(2, 25);
            tagSizeBitKey[5] = Math.Pow(2, 26);
            tagSizeBitKey[6] = Math.Pow(2, 27);
            tagSizeBitKey[7] = 0;
            tagSizeBitKey[8] = Math.Pow(2, 14);
            tagSizeBitKey[9] = Math.Pow(2, 15);
            tagSizeBitKey[10] = Math.Pow(2, 16);
            tagSizeBitKey[11] = Math.Pow(2, 17);
            tagSizeBitKey[12] = Math.Pow(2, 18);
            tagSizeBitKey[13] = Math.Pow(2, 19);
            tagSizeBitKey[14] = Math.Pow(2, 20);
            tagSizeBitKey[15] = 0;
            tagSizeBitKey[16] = Math.Pow(2, 7);
            tagSizeBitKey[17] = Math.Pow(2, 8);
            tagSizeBitKey[18] = Math.Pow(2, 9);
            tagSizeBitKey[19] = Math.Pow(2, 10);
            tagSizeBitKey[20] = Math.Pow(2, 11);
            tagSizeBitKey[21] = Math.Pow(2, 12);
            tagSizeBitKey[22] = Math.Pow(2, 13);
            tagSizeBitKey[23] = 0;
            tagSizeBitKey[24] = Math.Pow(2, 0);
            tagSizeBitKey[25] = Math.Pow(2, 1);
            tagSizeBitKey[26] = Math.Pow(2, 2);
            tagSizeBitKey[27] = Math.Pow(2, 3);
            tagSizeBitKey[28] = Math.Pow(2, 4);
            tagSizeBitKey[29] = Math.Pow(2, 5);
            tagSizeBitKey[30] = Math.Pow(2, 6);
            tagSizeBitKey[31] = 0;
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
            metadata.TagSize = (((fileData[6] & 0x7F) << 25) | ((fileData[7] & 0x7F) << 18) | ((fileData[8] & 0x7F) << 11) | ((fileData[9] & 0x7F) << 4)) >> 4;
        }
    }
}