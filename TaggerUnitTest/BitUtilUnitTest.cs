using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tagger;

namespace TaggerUnitTest
{
    [TestClass]
    public class BitUtilUnitTest
    {
        [TestMethod]
        public void TestIsBitSetBit7()
        {
            byte b = 0x80;
            Assert.IsTrue(BitUtil.IsBitSet(b, 7));
        }

        [TestMethod]
        public void TestIsBitSetBit6()
        {
            byte b = 0x40;
            Assert.IsTrue(BitUtil.IsBitSet(b, 6));
        }

        [TestMethod]
        public void TestIsBitSetBit5()
        {
            byte b = 0x20;
            Assert.IsTrue(BitUtil.IsBitSet(b, 5));
        }

        [TestMethod]
        public void TestIsBitSetBit4()
        {
            byte b = 0x10;
            Assert.IsTrue(BitUtil.IsBitSet(b, 4));
        }

        [TestMethod]
        public void TestIsBitSetBit3()
        {
            byte b = 0x08;
            Assert.IsTrue(BitUtil.IsBitSet(b, 3));
        }

        [TestMethod]
        public void TestIsBitSetBit2()
        {
            byte b = 0x04;
            Assert.IsTrue(BitUtil.IsBitSet(b, 2));
        }

        [TestMethod]
        public void TestIsBitSetBit1()
        {
            byte b = 0x02;
            Assert.IsTrue(BitUtil.IsBitSet(b, 1));
        }

        [TestMethod]
        public void TestIsBitSetBit0()
        {
            byte b = 0x01;
            Assert.IsTrue(BitUtil.IsBitSet(b, 0));
        }

        [TestMethod]
        public void TestIsBitSetShouldReturnFalseWhenBitIsNotSet()
        {
            byte b = 0xFC;
            Assert.IsFalse(BitUtil.IsBitSet(b, 0));
        }
    }
}
