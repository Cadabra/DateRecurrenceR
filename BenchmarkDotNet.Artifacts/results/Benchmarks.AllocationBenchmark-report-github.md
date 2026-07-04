```

BenchmarkDotNet v0.15.8, macOS Tahoe 26.5.1 (25F80) [Darwin 25.5.0]
Apple M1 Pro, 1 CPU, 8 logical and 8 physical cores
.NET SDK 10.0.301
  [Host] : .NET 10.0.9 (10.0.9, 10.0.926.27113), Arm64 RyuJIT armv8.0-a

Toolchain=InProcessEmitToolchain  

```
| Method        | Mean        | Error    | StdDev   | Allocated |
|-------------- |------------:|---------:|---------:|----------:|
| DailyStruct   |    25.47 ns | 0.060 ns | 0.050 ns |         - |
| WeeklyStruct  |   119.58 ns | 0.066 ns | 0.059 ns |         - |
| MonthlyStruct | 1,079.47 ns | 0.677 ns | 0.566 ns |         - |
| YearlyStruct  |   106.12 ns | 0.264 ns | 0.206 ns |         - |
| DailyBoxed    |    25.38 ns | 0.057 ns | 0.051 ns |         - |
