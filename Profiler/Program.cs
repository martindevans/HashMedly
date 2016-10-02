using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using HashMedly.Generators.Generator8;

namespace Profiler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ////Pure performance test
            //BenchmarkRunner.Run<Performance>();
            //Console.WriteLine("Press any key to continue");
            //Console.ReadKey();

            //Collisions
            Collisions.Run();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            ////Distribution test
            //Distribution.Run();
            //Console.WriteLine("Press any key to continue");
            //Console.ReadKey();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }

    public class Performance
    {
        private int _int = 27;
        private long _long = 34545734;
        private byte _byte = 127;
        private bool _bool = true;
        private string _string = "The quick brown fox jumped over the lazy dog and said \"Hello, World\"";

        [Benchmark(Baseline = true)]
        public int Baseline()
        {
            //This is an absolutely *dreadful* hash generator! But it's what a lot of naieve `GetHashCode` implementations look like so it's a good baseline

            unchecked
            {
                return _int.GetHashCode() * 17
                       + _long.GetHashCode() * 19
                       + _byte.GetHashCode() * 23
                       + _bool.GetHashCode() * 29
                       + _string.GetHashCode() * 31;
            }
        }

        [Benchmark]
        public int Murmur3_32()
        {
            var hash = HashMedly.Generators.Generator32.Murmur3.Create();

            hash = HashMedly.Generators.Generator32.IGenerator32Extensions.Mix(hash, _int);
            hash = HashMedly.Generators.Generator32.IGenerator32Extensions.Mix(hash, _long);
            hash = HashMedly.Generators.Generator32.IGenerator32Extensions.Mix(hash, _byte);
            hash = HashMedly.Generators.Generator32.IGenerator32Extensions.Mix(hash, _bool);
            hash = HashMedly.Generators.Generator32.IGenerator32Extensions.Mix(hash, _string);

            return hash.GetHashCode();
        }

        [Benchmark]
        public int FNV1A_32()
        {
            return HashMedly.Generators.Generator8.FNV1A32
                 .Create()
                 .Mix(_int)
                 .Mix(_long)
                 .Mix(_byte)
                 .Mix(_bool).Mix(_string)
                .GetHashCode();
        }

        [Benchmark]
        public int FNV1A_64()
        {
            return HashMedly.Generators.Generator8.FNV1A64
                .Create()
                .Mix(_int)
                .Mix(_long)
                .Mix(_byte)
                .Mix(_bool).Mix(_string)
                .GetHashCode();
        }
    }

    public class Collisions
    {
        private static readonly BaseHasher[] _tests = {
            new FNV1A32(),
            new FNV1A64(),
            new Murmur332(),
            new Terribad(),
            new Noise(),
            new Const(),
            new GetHashCode()
        };

        public static void Run()
        {
            PerformTest("English Words", EnglishWordsTest);
            PerformTest("Sequential Numbers", SequentialNumbers);
        }

        private static void PerformTest(string name, Func<IEnumerable<BaseHasher>, IEnumerable<KeyValuePair<double, BaseHasher>>> test)
        {
            Console.WriteLine("## {0} ##", name);

            foreach (var result in test(_tests))
                Console.WriteLine("{0}\t{1}", result.Value, result.Key);

            Console.WriteLine();
        }

        private static IEnumerable<KeyValuePair<double, BaseHasher>> SequentialNumbers(IEnumerable<BaseHasher> tests)
        {
            var numbers = Enumerable.Range(1, 216553).Select(a => (ulong)a).ToArray();

            return tests
                 .Select(t => new KeyValuePair<double, BaseHasher>(CountCollisions(numbers, t), t))
                 .OrderBy(a => a.Key);
        }

        private static IEnumerable<KeyValuePair<double, BaseHasher>> EnglishWordsTest(IEnumerable<BaseHasher> tests)
        {
            var words = new HashSet<string>();
            words.UnionWith(File.ReadAllLines("EnglishWords.txt")
                                    .Where(l => !l.StartsWith("#")));

            return tests
                 .Select(t => new KeyValuePair<double, BaseHasher>(CountCollisions(words, t), t))
                 .OrderBy(a => a.Key);
        }

        private static ulong CountCollisions(IEnumerable<ulong> numbers, BaseHasher hasher)
        {
            ulong collisions = 0;
            var hashes = new HashSet<int>();
            foreach (var number in numbers)
            {
                var hash = hasher.Hash(number);
                if (!hashes.Add(hash))
                    collisions++;
            }

            return collisions;
        }

        private static ulong CountCollisions(IEnumerable<string> words, BaseHasher hasher)
        {
            ulong collisions = 0;
            var hashes = new HashSet<int>();
            foreach (var word in words)
            {
                var hash = hasher.Hash(word);
                if (!hashes.Add(hash))
                    collisions++;
            }

            return collisions;
        }
    }

    /// <summary>
    /// https://en.wikipedia.org/wiki/Diehard_tests
    /// </summary>
    public class Distribution
    {
        private static readonly BaseHasher[] _tests = {
            new FNV1A32(),
            new FNV1A64(),
            new Noise(),
            new Const()
        };

        public static void Run()
        {
            PerformTest("Birthday Spacings", BirthdaySpacingsTest);
        }

        private static void PerformTest(string name, Func<IEnumerable<BaseHasher>, IEnumerable<KeyValuePair<double, BaseHasher>>> test)
        {
            Console.WriteLine("## {0} ##", name);

            foreach (var result in test(_tests))
                Console.WriteLine("{0}\t{1}", result.Value, result.Key);

            Console.WriteLine();
        }

        private static IEnumerable<KeyValuePair<double, BaseHasher>> BirthdaySpacingsTest(IEnumerable<BaseHasher> tests)
        {
            throw new NotImplementedException();
        }
    }

    #region hashers
    public abstract class BaseHasher
    {
        public abstract int Hash(IReadOnlyList<byte> values);

        public abstract int Hash(ulong value);

        public abstract int Hash(string value);
    }

    public class Const
        : BaseHasher
    {
        public override int Hash(IReadOnlyList<byte> values)
        {
            return 4;
        }

        public override int Hash(ulong value)
        {
            return 4;
        }

        public override int Hash(string value)
        {
            return 4;
        }

        public override string ToString()
        {
            return "Constant";
        }
    }

    public class Noise
        : BaseHasher
    {
        private readonly Random _random = new Random();
        private readonly byte[] _bytes = new byte[4];

        public override int Hash(string value)
        {
            _random.NextBytes(_bytes);
            return _bytes[0] | _bytes[1] << 8 | _bytes[2] << 16 | _bytes[3] << 24;
        }

        public override int Hash(IReadOnlyList<byte> values)
        {
            return Hash("");
        }

        public override int Hash(ulong value)
        {
            return Hash("");
        }

        public override string ToString()
        {
            return "Uniform Rng";
        }
    }

    public class Terribad
        : BaseHasher
    {
        public override int Hash(IReadOnlyList<byte> values)
        {
            var hash = 17;
            for (int i = 0; i < values.Count; i++)
            {
                unchecked
                {
                    hash = hash * 17 + values[i];
                }
            }

            return hash;
        }

        public override int Hash(ulong value)
        {
            var high = unchecked((uint)(value >> 32));
            var low = unchecked((uint)value);
            return unchecked((int)(high ^ low));
        }

        public override int Hash(string value)
        {
            var hash = 17;
            for (int i = 0; i < value.Length; i++)
            {
                unchecked
                {
                    hash = hash * 17 + value[i];
                }
            }

            return hash;
        }

        public override string ToString()
        {
            return "Terribad";
        }
    }

    public class GetHashCode
        : BaseHasher
    {
        public override int Hash(IReadOnlyList<byte> values)
        {
            return values.Select(a => (int)a).Aggregate((a, b) => a + b).GetHashCode();
        }

        public override int Hash(string value)
        {
            return value.GetHashCode();
        }

        public override int Hash(ulong value)
        {
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return "GetHashCode()";
        }
    }

    // ReSharper disable once InconsistentNaming
    public class FNV1A32
        : BaseHasher
    {
        public override int Hash(IReadOnlyList<byte> values)
        {
            var h = HashMedly.Generators.Generator8.FNV1A32.Create();
            for (var i = 0; i < values.Count; i++)
                h = h.Mix(values[i]);

            return h.GetHashCode();
        }

        public override int Hash(string value)
        {
            var h = HashMedly.Generators.Generator8.FNV1A32.Create();
            h = h.Mix(value);
            return h.GetHashCode();
        }

        public override int Hash(ulong value)
        {
            var h = HashMedly.Generators.Generator8.FNV1A32.Create();
            h = h.Mix(value);
            return h.GetHashCode();
        }

        public override string ToString()
        {
            return "FNV1A 32bit";
        }
    }

    // ReSharper disable once InconsistentNaming
    public class FNV1A64
        : BaseHasher
    {
        public override int Hash(IReadOnlyList<byte> values)
        {
            var h = HashMedly.Generators.Generator8.FNV1A64.Create();
            for (var i = 0; i < values.Count; i++)
                h = h.Mix(values[i]);

            return h.GetHashCode();
        }

        public override int Hash(string value)
        {
            var h = HashMedly.Generators.Generator8.FNV1A64.Create();
            h = h.Mix(value);
            return h.GetHashCode();
        }

        public override int Hash(ulong value)
        {
            var h = HashMedly.Generators.Generator8.FNV1A64.Create();
            h = h.Mix(value);
            return h.GetHashCode();
        }

        public override string ToString()
        {
            return "FNV1A 64bit";
        }
    }

    public class Murmur332
        : BaseHasher
    {
        public override int Hash(IReadOnlyList<byte> values)
        {
            var h = HashMedly.Generators.Generator32.Murmur3.Create();
            for (var i = 0; i < values.Count; i++)
                h = HashMedly.Generators.Generator32.IGenerator32Extensions.Mix(h, values[i]);

            return h.GetHashCode();
        }

        public override int Hash(string value)
        {
            var h = HashMedly.Generators.Generator32.Murmur3.Create();
            h = HashMedly.Generators.Generator32.IGenerator32Extensions.Mix(h, value);
            return h.GetHashCode();
        }

        public override int Hash(ulong value)
        {
            var h = HashMedly.Generators.Generator32.Murmur3.Create();
            h = HashMedly.Generators.Generator32.IGenerator32Extensions.Mix(h, value);
            return h.GetHashCode();
        }

        public override string ToString()
        {
            return "Murmur3 32bit";
        }
    }
    #endregion
}
