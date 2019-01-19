using System;
using System.Runtime.CompilerServices;

namespace HashMedly.Generators.Generator8
{
    /// <summary>
    /// Base interface for hash generators. You should *not* use this interface directly (using a struct as an interface causes boxing).
    /// This exists purely for the extension method generic constraints (don't worry about this unless you're implementing a new generator)
    /// </summary>
    public interface IGenerator8
    {
        void Byte(byte value);
    }

    public static class IGenerator8Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, byte value) where T : struct, IGenerator8
        {
            hash.Byte(value);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, sbyte value) where T : struct, IGenerator8
        {
            hash.Byte(unchecked((byte)value));

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, bool value) where T : struct, IGenerator8
        {
            var u = new Union8 { Bool = value };

            hash.Byte(u.Byte1);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, short value) where T : struct, IGenerator8
        {
            var u = new Union16 { Int16 = value };

            hash.Byte(u.Byte1);
            hash.Byte(u.Byte2);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, ushort value) where T : struct, IGenerator8
        {
            var u = new Union16 { UInt16 = value };

            hash.Byte(u.Byte1);
            hash.Byte(u.Byte2);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, char value) where T : struct, IGenerator8
        {
            var u = new Union16 { Char = value };

            hash.Byte(u.Byte1);
            hash.Byte(u.Byte2);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, uint value) where T : struct, IGenerator8
        {
            var u = new Union32 { UInt32 = value };

            hash.Byte(u.Byte1);
            hash.Byte(u.Byte2);
            hash.Byte(u.Byte3);
            hash.Byte(u.Byte4);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, int value) where T : struct, IGenerator8
        {
            var u = new Union32 { Int32 = value };

            hash.Byte(u.Byte1);
            hash.Byte(u.Byte2);
            hash.Byte(u.Byte3);
            hash.Byte(u.Byte4);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, float value) where T : struct, IGenerator8
        {
            var u = new Union32 { Single = value };

            hash.Byte(u.Byte1);
            hash.Byte(u.Byte2);
            hash.Byte(u.Byte3);
            hash.Byte(u.Byte4);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, long value) where T : struct, IGenerator8
        {
            var u = new Union64 { Int64 = value };

            hash.Byte(u.Byte1);
            hash.Byte(u.Byte2);
            hash.Byte(u.Byte3);
            hash.Byte(u.Byte4);
            hash.Byte(u.Byte5);
            hash.Byte(u.Byte6);
            hash.Byte(u.Byte7);
            hash.Byte(u.Byte8);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, ulong value) where T : struct, IGenerator8
        {
            var u = new Union64 { UInt64 = value };

            hash.Byte(u.Byte1);
            hash.Byte(u.Byte2);
            hash.Byte(u.Byte3);
            hash.Byte(u.Byte4);
            hash.Byte(u.Byte5);
            hash.Byte(u.Byte6);
            hash.Byte(u.Byte7);
            hash.Byte(u.Byte8);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, double value) where T : struct, IGenerator8
        {
            var u = new Union64 { Double = value };

            hash.Byte(u.Byte1);
            hash.Byte(u.Byte2);
            hash.Byte(u.Byte3);
            hash.Byte(u.Byte4);
            hash.Byte(u.Byte5);
            hash.Byte(u.Byte6);
            hash.Byte(u.Byte7);
            hash.Byte(u.Byte8);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, decimal value) where T : struct, IGenerator8
        {
            var u = new Union128 { Decimal = value };

            hash.Byte(u.Byte1);
            hash.Byte(u.Byte2);
            hash.Byte(u.Byte3);
            hash.Byte(u.Byte4);
            hash.Byte(u.Byte5);
            hash.Byte(u.Byte6);
            hash.Byte(u.Byte7);
            hash.Byte(u.Byte8);

            hash.Byte(u.Byte9);
            hash.Byte(u.Byte10);
            hash.Byte(u.Byte11);
            hash.Byte(u.Byte12);
            hash.Byte(u.Byte13);
            hash.Byte(u.Byte14);
            hash.Byte(u.Byte15);
            hash.Byte(u.Byte16);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, string value) where T : struct, IGenerator8
        {
            for (var i = 0; i < value.Length; i++)
                hash = hash.Mix(value[i]);

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T>(this T hash, Guid value) where T : struct, IGenerator8
        {
            var u = new Union128 { Guid = value };

            return hash
                .Mix(u.UInt32_1)
                .Mix(u.UInt32_2)
                .Mix(u.UInt32_3)
                .Mix(u.UInt32_4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mix<T, TO>(this T hash, TO value) where T : struct, IGenerator8
        {
            return hash.Mix(value.GetHashCode());
        }
    }
}
