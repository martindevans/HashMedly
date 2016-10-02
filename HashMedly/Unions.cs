using System;
using System.Runtime.InteropServices;

namespace HashMedly
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct Union8
    {
        [FieldOffset(0)]public byte UInt8;
        [FieldOffset(0)]public sbyte Int8;
        [FieldOffset(0)]public bool Bool;

        [FieldOffset(0)]public readonly byte Byte1;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct Union16
    {
        [FieldOffset(0)]public ushort UInt16;
        [FieldOffset(0)]public short Int16;
        [FieldOffset(0)]public char Char;

        [FieldOffset(0)]public readonly byte Byte1;
        [FieldOffset(1)]public readonly byte Byte2;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct Union32
    {
        [FieldOffset(0)]public uint UInt32;
        [FieldOffset(0)]public int Int32;
        [FieldOffset(0)]public float Single;

        [FieldOffset(0)]public readonly ushort UInt16_1;
        [FieldOffset(2)]public readonly ushort UInt16_2;

        [FieldOffset(0)]public char Char_1;
        [FieldOffset(2)]public char Char_2;

        [FieldOffset(0)]public readonly byte Byte1;
        [FieldOffset(1)]public readonly byte Byte2;
        [FieldOffset(2)]public readonly byte Byte3;
        [FieldOffset(3)]public readonly byte Byte4;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct Union64
    {
        [FieldOffset(0)]public ulong UInt64;
        [FieldOffset(0)]public long Int64;
        [FieldOffset(0)]public double Double;

        [FieldOffset(0)]public readonly uint UInt32_1;
        [FieldOffset(4)]public readonly uint UInt32_2;

        [FieldOffset(0)]public readonly byte Byte1;
        [FieldOffset(1)]public readonly byte Byte2;
        [FieldOffset(2)]public readonly byte Byte3;
        [FieldOffset(3)]public readonly byte Byte4;
        [FieldOffset(4)]public readonly byte Byte5;
        [FieldOffset(5)]public readonly byte Byte6;
        [FieldOffset(6)]public readonly byte Byte7;
        [FieldOffset(7)]public readonly byte Byte8;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct Union128
    {
        [FieldOffset(0)]public decimal Decimal;
        [FieldOffset(0)]public Guid Guid;

        [FieldOffset(0)]public readonly uint UInt32_1;
        [FieldOffset(4)]public readonly uint UInt32_2;
        [FieldOffset(8)]public readonly uint UInt32_3;
        [FieldOffset(12)]public readonly uint UInt32_4;

        [FieldOffset(0)]public readonly byte Byte1;
        [FieldOffset(1)]public readonly byte Byte2;
        [FieldOffset(2)]public readonly byte Byte3;
        [FieldOffset(3)]public readonly byte Byte4;
        [FieldOffset(4)]public readonly byte Byte5;
        [FieldOffset(5)]public readonly byte Byte6;
        [FieldOffset(6)]public readonly byte Byte7;
        [FieldOffset(7)]public readonly byte Byte8;
        [FieldOffset(8)]public readonly byte Byte9;
        [FieldOffset(9)]public readonly byte Byte10;
        [FieldOffset(10)]public readonly byte Byte11;
        [FieldOffset(11)]public readonly byte Byte12;
        [FieldOffset(12)]public readonly byte Byte13;
        [FieldOffset(13)]public readonly byte Byte14;
        [FieldOffset(14)]public readonly byte Byte15;
        [FieldOffset(15)]public readonly byte Byte16;
    }
}
