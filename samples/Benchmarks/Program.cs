using BenchmarkDotNet.Running;
using Benchmarks;

BenchmarkSwitcher.FromAssembly(typeof(AllocationBenchmark).Assembly).Run(args);