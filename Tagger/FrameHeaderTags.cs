using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagger
{
    public class TagAlterPreservation
    {
        public const bool Preserved = false;
        public const bool Discarded = true;
    }

    public class FileAlterPreservation
    {
        public const bool Preserved = false;
        public const bool Discarded = true;
    }

    public class Compression
    {
        public const bool Uncompressed = false;
        public const bool Compressed = true;
    }

    public class Encryption
    {
        public const bool Unencrypted = false;
        public const bool encrypted = true;
    }

    public class GroupIdentity
    {
        public const bool DoesNotContainGroupIdentity = false;
        public const bool ContainsGroupIdentity = true;
    }
}
