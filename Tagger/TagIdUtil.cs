using System.Text;

namespace Tagger
{
    public class TagIdUtil
    {
        private static Encoding textEnconding = Encoding.GetEncoding("iso-8859-1");

        public static bool isID3v2(byte[] fileData)
        {
            return textEnconding.GetString(fileData, 0, 3).ToUpper() == "ID3";
        }
    }
}
