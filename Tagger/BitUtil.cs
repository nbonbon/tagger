namespace Tagger
{
    public class BitUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteToSearch">The byte data to check</param>
        /// <param name="bitNumberToCheck">The bit to check. 7654 3210 (MSB in lowest address - Big Endian)</param>
        /// <returns></returns>
        public static bool IsBitSet(byte byteToSearch, int bitNumberToCheck)
        {
            return (byteToSearch & (1 << bitNumberToCheck)) != 0;
        }
    }
}
