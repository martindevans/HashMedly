# HashMedly

HashMedly is designed to make it easy to write a quality implementation of `GetHashCode()`. Traditionally this consists of visiting [this stackoverflow question](http://stackoverflow.com/q/263400/108234) and implementing top rated answer... unfortunately the top rated answer includes multiple solutions and it's not clear what the best thing is to do. This library aims to solve that.

HashMedly is available [on nuget](https://www.nuget.org/packages/HashMedly/):

 > Install-Package HashMedly

# TL;DR

If you don't care about the details of the library, use this:

```
Murmur3.Create()
    .Mix(_field1)
    .Mix(_field2)
    .GetHashCode();
```

This is very fast and murmur3 is a very good hash code. Job done.

# Is It Fast?

The library is designed to be pretty fast. The hash code types are all structs - this means that absolutely no allocations are made. Additionally the code is largely branchless, so it should run at the speed of *maths*. For some more concrete performance numbers clone the project and check out the `Profiler` project in the solution. This will test the various hashes versus a baseline implementation. This is a slightly unfair comparison because the baseline is a pretty bad hash function, but it's what most people would implement anyway:

|     Method |   Median |     StdDev | Scaled | Scaled-SD |
|------------|----------|------------|--------|-----------|
|   Baseline |    32 ns |  1.2437 ns |   1.00 |      0.00 |
| Murmur3_32 |   511 ns |  5.4898 ns |  15.68 |      0.56 |
|   FNV1A_32 | 1,323 ns | 11.8824 ns |  40.52 |      1.42 |
|   FNV1A_64 | 1,850 ns | 71.6737 ns |  56.23 |      2.86 |

So overall murmur3 is about 15 times slower than what you'd naively implement but it's a much better hash code so it's probably a worthwhile tradeoff considering this is all happening in *under 2 microseconds*.

# Implementing A Hash Function

To implement a hash function you should create a new struct which implements either IGenerator8 or IGenerator32 - which one depends upon whether you want to process the data in 8 bit chunks (such as FNV), or 32 bit chunks (such as Murmur3). Once you have implemented this you should *never use IGenerator directly* - doing so will cause boxing and destroy performance! The interface exists purely for generic constraints. Once you have implemented the struct you will automatically get all of the `Mix` methods for free (they're extension methods).
