using System.IO.Abstractions;

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

                if (Id3Metadata.isID3v2(fileData))
                {
                    metadata = new Id3Metadata(fileData);
                    metadata.Parse();
                }
            }

            return metadata;
        }
    }
}