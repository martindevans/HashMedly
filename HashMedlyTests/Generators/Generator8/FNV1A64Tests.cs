using System;
using HashMedly.Generators.Generator8;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashMedlyTests.Generators.Generator8
{
    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class FNVA64Tests
    {
        private FNV1A64 _hash = FNV1A64.Create();

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssertThat_GetHashFromUninitialized_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            new FNV1A64().GetHashCode();
        }

        [TestMethod]
        public void AssertThat_EmptyHash_IsNotZero()
        {
            Assert.AreNotEqual(0, _hash.Hash);
        }

        private void Test_ChangesValue(Func<FNV1A64> mix)
        {
            var before = _hash.GetHashCode();
            var hash = mix();
            var after = hash.GetHashCode();

            Assert.AreNotEqual(before, after);
            Assert.AreNotEqual(0, after);
        }

        [TestMethod]
        public void AssertThat_MixingByte_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix((byte)122));
        }

        [TestMethod]
        public void AssertThat_MixingSByte_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix((sbyte)-121));
        }

        [TestMethod]
        public void AssertThat_MixingBool_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix(true));
        }

        [TestMethod]
        public void AssertThat_MixingUInt16_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix((ushort)12423));
        }

        [TestMethod]
        public void AssertThat_MixingInt16_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix((short)12423));
        }

        [TestMethod]
        public void AssertThat_MixingChar_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix('#'));
        }

        [TestMethod]
        public void AssertThat_MixingUInt32_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix((uint)1293897234));
        }

        [TestMethod]
        public void AssertThat_MixingInt32_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix((int)-1293897234));
        }

        [TestMethod]
        public void AssertThat_MixingFloat32_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix((float)1673.1253235));
        }

        [TestMethod]
        public void AssertThat_MixingUInt64_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix((ulong)12938972348735679534));
        }

        [TestMethod]
        public void AssertThat_MixingInt64_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix((long)-129389748735679534));
        }

        [TestMethod]
        public void AssertThat_MixingFloat64_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix((double)1673.1253235));
        }

        [TestMethod]
        public void AssertThat_MixingDecimal_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix((decimal)16329681782073.12532235680709213835M));
        }

        [TestMethod]
        public void AssertThat_MixingString_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix((string)"Hello, World"));
        }

        [TestMethod]
        public void AssertThat_MixingObject_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix((object)new object()));
        }

        [TestMethod]
        public void AssertThat_MixingGuid_ChangesValue()
        {
            Test_ChangesValue(() => _hash.Mix(Guid.NewGuid()));
        }
    }
}
