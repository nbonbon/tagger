namespace Tagger
{
    public class BitUtil
    {
        /// <summary>
        /// Checks to see if a particular bit it set in a byte.
        /// </summary>
        /// <param name="byteToSearch">The byte data to check</param>
        /// <param name="bitNumberToCheck">The bit to check. 7654 3210 (MSB in lowest address - Big Endian)</param>
        /// <returns>True if the bit is set, False otherwise</returns>
        public static bool IsBitSet(byte byteToSearch, int bitNumberToCheck)
        {
            return (byteToSearch & (1 << bitNumberToCheck)) != 0;
        }

        public static uint ParseBigEndianUint32(byte[] data, int offset)
        {
            return (uint)((uint)data[offset] << 24) | ((uint)data[offset + 1] << 16) | ((uint)data[offset + 2] << 8) | (uint)data[offset + 3];
        }
    }
}
