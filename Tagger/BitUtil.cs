namespace Tagger
{
    public class BitUtil
    {
        public static bool IsBitSet(byte byteToSearch, int bitNumberToCheck)
        {
            return (byteToSearch & (1 << bitNumberToCheck)) != 0;
        }
    }
}
