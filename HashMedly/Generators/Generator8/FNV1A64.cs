using System;
using System.Runtime.CompilerServices;

namespace HashMedly.Generators.Generator8
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// A calculator for 64 bit FNV1A hash
    /// </summary>
    /// <remarks>http://isthe.com/chongo/tech/comp/fnv/#FNV-1a</remarks>
    public struct FNV1A64
        : IGenerator8
    {
        private bool _initialized;

        private ulong _hash;
        public long Hash
        {
            get
            {
                if (!_initialized)
                    throw new InvalidOperationException("Cannot get Hash from an unitialized FNV1A instance");
                return unchecked((long)_hash);
            }
        }

        public static FNV1A64 Create()
        {
            return new FNV1A64 {
                _hash = 14695981039346656037,
                _initialized = true
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Byte(byte value)
        {
            unchecked
            {
                _hash ^= value;
                _hash *= 1099511628211;

                //This is *not* part of pure FNV1A!
                //When FNV1A hits a zero value it stays there forever! To avoid this pathological value we unconditionally set
                //the last bit to 1. This ensures that a zero value is never returned from this method at the expense of a single
                //bit of the hash entropy.
                _hash |= 1;
            }
        }

        public override int GetHashCode()
        {
            var upper = unchecked((int)Hash);
            var lower = unchecked((int)(Hash >> 32));

            return lower ^ upper;
        }
    }
}
