using System;
using System.Runtime.CompilerServices;

namespace HashMedly.Generators.Generator8
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// A calculator for 32 bit FNV1A hash
    /// </summary>
    /// <remarks>http://isthe.com/chongo/tech/comp/fnv/#FNV-1a</remarks>
    public struct FNV1A32
        : IGenerator8
    {
        private bool _initialized;

        private uint _hash;
        public int Hash
        {
            get
            {
                if (!_initialized)
                    throw new InvalidOperationException("Cannot get Hash from an unitialized FNV1A instance");
                return unchecked((int)_hash);
            }
        }

        public static FNV1A32 Create()
        {
            return new FNV1A32 {
                _hash = 2166136261,
                _initialized = true
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Byte(byte value)
        {
            unchecked
            {
                _hash ^= value;
                _hash *= 16777619;

                //This is *not* part of pure FNV1A!
                //When FNV1A hits a zero value it stays there forever! To avoid this pathological value we unconditionally set
                //the last bit to 1. This ensures that a zero value is never returned from this method at the expense of a single
                //bit of the hash entropy.
                _hash |= 1;
            }
        }

        public override int GetHashCode()
        {
            return Hash;
        }
    }
}
