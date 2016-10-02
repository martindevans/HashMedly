using System;
using System.Runtime.CompilerServices;

namespace HashMedly.Generators.Generator32
{
    /// <summary>
    /// Base interface for hash generators. You should *not* use this interface directly (using a struct as an interface causes boxing).
    /// This exists purely for the extension method generic constraints (don't worry about this unless you're implementing a new generator)
    /// </summary>
    public interface IGenerator32
    {
        /// <summary>
        /// Combine 32 bits more of state into the hash function
        /// </summary>
        /// <remarks>Generally you should not directly use this method, instead use one of the `Mix` extensions</remarks>
        /// <param name="value"></param>
        void UInt32(uint value);
    }

    public static class IGenerator32Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, byte value) where T : struct, IGenerator32
        {
            hash.UInt32(value);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, sbyte value) where T : struct, IGenerator32
        {
            var u = new Union8 { Int8 = value };

            hash.UInt32(u.UInt8);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, bool value) where T : struct, IGenerator32
        {
            var u = new Union8 { Bool = value };

            hash.UInt32(u.Byte1);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, short value) where T : struct, IGenerator32
        {
            var u = new Union16 { Int16 = value };

            hash.UInt32(u.UInt16);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, ushort value) where T : struct, IGenerator32
        {
            hash.UInt32(value);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, char value) where T : struct, IGenerator32
        {
            var u = new Union16 { Char = value };

            hash.UInt32(u.UInt16);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, uint value) where T : struct, IGenerator32
        {
            var u = new Union32 { UInt32 = value };

            hash.UInt32(u.UInt32);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, int value) where T : struct, IGenerator32
        {
            var u = new Union32 { Int32 = value };

            hash.UInt32(u.UInt32);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, float value) where T : struct, IGenerator32
        {
            var u = new Union32 { Single = value };

            hash.UInt32(u.UInt32);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, long value) where T : struct, IGenerator32
        {
            var u = new Union64 { Int64 = value };

            hash.UInt32(u.UInt32_1);
            hash.UInt32(u.UInt32_2);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, ulong value) where T : struct, IGenerator32
        {
            var u = new Union64 { UInt64 = value };

            hash.UInt32(u.UInt32_1);
            hash.UInt32(u.UInt32_2);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, double value) where T : struct, IGenerator32
        {
            var u = new Union64 { Double = value };

            hash.UInt32(u.UInt32_1);
            hash.UInt32(u.UInt32_2);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, decimal value) where T : struct, IGenerator32
        {
            var u = new Union128 { Decimal = value };

            hash.UInt32(u.UInt32_1);
            hash.UInt32(u.UInt32_2);

            hash.UInt32(u.UInt32_3);
            hash.UInt32(u.UInt32_4);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, string value) where T : struct, IGenerator32
        {
            //Mix characters in 32 bit blocks (i.e. pairs of chars)
            var limit = value.Length & ~1;
            for (var i = 0; i < limit; i += 2)
            {
                var joined = new Union32
                {
                    Char_1 = value[i + 0],
                    Char_2 = value[i + 1],
                };

                hash.UInt32(joined.UInt32);
            }

            //Mix in the last char if necessary
            if (value.Length - limit > 0)
                hash.UInt32(value[value.Length - 1]);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, Guid value) where T : struct, IGenerator32
        {
            var u = new Union128 { Guid = value };

            hash.UInt32(u.UInt32_1);
            hash.UInt32(u.UInt32_2);
            hash.UInt32(u.UInt32_3);
            hash.UInt32(u.UInt32_4);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T, TO>(this T hash, TO value) where T : struct, IGenerator32
        {
            return hash.Mix(value.GetHashCode());
        }
    }
}
