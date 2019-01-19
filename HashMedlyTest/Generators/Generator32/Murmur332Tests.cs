using System;
using HashMedly.Generators.Generator32;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashMedlyTest.Generators.Generator32
{
    [TestClass]
    public class Murmur332Tests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssertThat_GetHashFromUninitialized_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            new Murmur3().GetHashCode();
        }

        [TestMethod]
        public void AssertThat_EmptyHash_IsNotZero()
        {
            Assert.AreNotEqual(0, Murmur3.Create().Hash);
        }

        private void Test_ChangesValue(Func<Murmur3, Murmur3> mix)
        {
            var gen = Murmur3.Create();

            var before = gen.GetHashCode();
            var gen2 = mix(gen);
            var after = gen2.GetHashCode();

            Assert.AreNotEqual(before, after);
            Assert.AreNotEqual(0, after);
        }

        private void Test_DoesNotChangeValue(Func<Murmur3, Murmur3> mix)
        {
            var a = Murmur3.Create();
            var b = Murmur3.Create();

            mix(a);
            mix(b);

            Assert.AreEqual(a.Hash, b.Hash);
        }

        [TestMethod]
        public void AssertThat_MixingByte_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix((byte)122));
        }

        [TestMethod]
        public void AssertThat_SameByte_DoesNotChangeValue()
        {
            Test_DoesNotChangeValue(a => a.Mix((byte)122));
        }

        [TestMethod]
        public void AssertThat_MixingSByte_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix((sbyte)-121));
        }

        [TestMethod]
        public void AssertThat_SameSByte_DoesNotChangeValue()
        {
            Test_DoesNotChangeValue(a => a.Mix((byte)122));
        }

        [TestMethod]
        public void AssertThat_MixingBool_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix(true));
        }
        
        [TestMethod]
        public void AssertThat_SameBool_DoesNotChangeValue()
        {
            Test_DoesNotChangeValue(a => a.Mix(true));
        }

        [TestMethod]
        public void AssertThat_MixingUInt16_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix((ushort)12423));
        }

        [TestMethod]
        public void AssertThat_SameUInt16_DoesNotChangeValue()
        {
            Test_DoesNotChangeValue(a => a.Mix((ushort)12423));
        }

        [TestMethod]
        public void AssertThat_MixingInt16_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix((short)12423));
        }

        [TestMethod]
        public void AssertThat_SameInt16_DoesNotChangeValue()
        {
            Test_DoesNotChangeValue(a => a.Mix((short)12423));
        }

        [TestMethod]
        public void AssertThat_MixingChar_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix('#'));
        }

        [TestMethod]
        public void AssertThat_SameChar_DoesNotChangeValue()
        {
            Test_DoesNotChangeValue(a => a.Mix('@'));
        }

        [TestMethod]
        public void AssertThat_MixingUInt32_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix((uint)1293897234));
        }

        [TestMethod]
        public void AssertThat_SameUInt32_DoesNotChangeValue()
        {
            Test_DoesNotChangeValue(a => a.Mix((uint)1293897234));
        }

        [TestMethod]
        public void AssertThat_MixingInt32_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix((int)-1293897234));
        }

        [TestMethod]
        public void AssertThat_SameInt32_DoesNotChangeValue()
        {
            Test_DoesNotChangeValue(a => a.Mix((int)-1293897234));
        }

        [TestMethod]
        public void AssertThat_MixingFloat32_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix((float)1673.1253235));
        }

        [TestMethod]
        public void AssertThat_SameFloat32_DoesNotChangeValue()
        {
            Test_DoesNotChangeValue(a => a.Mix((float)1673.1253235));
        }

        [TestMethod]
        public void AssertThat_MixingUInt64_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix((ulong)12938972348735679534));
        }

        [TestMethod]
        public void AssertThat_SameUInt64_DoesNotChangeValue()
        {
            Test_DoesNotChangeValue(a => a.Mix((ulong)12938972348735679534));
        }

        [TestMethod]
        public void AssertThat_MixingInt64_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix((long)-129389748735679534));
        }

        [TestMethod]
        public void AssertThat_SameInt64_DoesNotChangeValue()
        {
            Test_DoesNotChangeValue(a => a.Mix((long)-129389748735679534));
        }

        [TestMethod]
        public void AssertThat_MixingFloat64_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix((double)1673.1253235));
        }

        [TestMethod]
        public void AssertThat_SameFloat64_DoesNotChangeValue()
        {
            Test_DoesNotChangeValue(a => a.Mix((double)1673.1253235));
        }

        [TestMethod]
        public void AssertThat_MixingDecimal_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix((decimal)16329681782073.12532235680709213835M));
        }

        [TestMethod]
        public void AssertThat_SameDecimal_DoesNotChangeValue()
        {
            Test_DoesNotChangeValue(a => a.Mix((decimal)16329681782073.12532235680709213835M));
        }

        [TestMethod]
        public void AssertThat_MixingString_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix((string)"Hello, World"));
        }

        [TestMethod]
        public void AssertThat_SameString_DoesNotChangeValue()
        {
            Test_DoesNotChangeValue(a => a.Mix((string)"Hello, World"));
        }

        [TestMethod]
        public void AssertThat_LongerString_ChangesValue()
        {
            var h1 = Murmur3.Create().Mix((string)"AA").Hash;
            var h2 = Murmur3.Create().Mix((string)"AAA").Hash;

            Assert.AreNotEqual(h1, h2);
        }

        [TestMethod]
        public void AssertThat_MixingObject_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix((object)new object()));
        }

        [TestMethod]
        public void AssertThat_MixingGuid_ChangesValue()
        {
            Test_ChangesValue(a => a.Mix(Guid.NewGuid()));
        }
    }
}
