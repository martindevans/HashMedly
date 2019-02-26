using System;
using System.Runtime.CompilerServices;

namespace HashMedly.Generators.Generator32
{
    /// <summary>
    /// A calculator for murmur3 hash
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/MurmurHash</remarks>
    public struct Murmur3
        : IGenerator32
    {
        private readonly bool _initialized;

        private uint _length;
        private uint _hash;

        public int Hash
        {
            get
            {
                if (!_initialized)
                    throw new InvalidOperationException("Cannot get Hash from an unitialized Murmur3 instance");

                //Finish off the hash in a temp (we don't want to modify the actual value since that would cause multiple accesses to be different)
                var hash = _hash;

                hash ^= _length;
                hash *= 0x85ebca6b;
                hash ^= hash >> 13;
                hash *= 0xc2b2ae35;
                hash ^= hash >> 16;

                return unchecked((int)hash);
            }
        }

        private Murmur3(uint seed)
        {
            _hash = seed;
            _length = 0;

            _initialized = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UInt32(uint k)
        {
            _length += 4;

            unchecked
            {
                k = k * 0xCC9E2D51;
                k = ROL(k, 15);
                k = k * 0x1B873593;

                _hash = _hash ^ k;
                _hash = ROL(_hash, 13);
                _hash = _hash * 5 + 0xE6546B64;
            }
        }

        public static Murmur3 Create(uint seed = 9225900)
        {
            return new Murmur3(seed);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ROL(uint x, byte r)
        {
            return (x << r) | (x >> (32 - r));
        }

        public override int GetHashCode()
        {
            return Hash;
        }
    }
}
