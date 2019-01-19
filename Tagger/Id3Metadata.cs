using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool isUnsynchronisationUsed { get; set; }
        public bool containsExtendedHeader { get; set; }
        public bool isExperimentalStage { get; set; }
    }
}
