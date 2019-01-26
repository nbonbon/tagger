using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagger
{
    class Program
    {
        static void Main(string[] args)
        {
            MetadataParser parser = new MetadataParser(args[0]);
            Id3Metadata metadata = parser.Parse();
            Console.WriteLine(metadata);
        }
    }
}
