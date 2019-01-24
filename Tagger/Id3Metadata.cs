namespace Tagger
{
    public class Id3Metadata
    {
        public byte VersionMajor { get; set; }
        public byte VersionMinor { get; set; }
        public byte VersionRevision { get; set; }

        public string Version
        {
            get
            {
                return VersionMajor.ToString() + "." + VersionMinor + "." + VersionRevision;
            }

            private set{ }
        }

        public bool IsUnsynchronisationUsed { get; set; }
        public bool ContainsExtendedHeader { get; set; }
        public bool IsExperimentalStage { get; set; }
        public int TagSize { get; set; }
    }
}
