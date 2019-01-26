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
        public uint TagSize { get; set; }
        public ExtendedHeader ExtendedHeader { get; set; }

        public override string ToString()
        {
            string result = string.Empty;
            result += "Version: " + Version;
            result += "ContainsExtendedHeader: " + ContainsExtendedHeader;
            result += "IsExperimentalStage: " + IsExperimentalStage;
            result += "TagSize: " + TagSize;
            result += ExtendedHeader.ToString();
            return result;
        }
    }
}
