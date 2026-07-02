```

BenchmarkDotNet v0.15.8, macOS Tahoe 26.5.1 (25F80) [Darwin 25.5.0]
Apple M1 Pro, 1 CPU, 8 logical and 8 physical cores
.NET SDK 10.0.301
  [Host] : .NET 10.0.9 (10.0.9, 10.0.926.27113), Arm64 RyuJIT armv8.0-a

Toolchain=InProcessEmitToolchain  

```
| Method        | Mean        | Error    | StdDev   | Allocated |
|-------------- |------------:|---------:|---------:|----------:|
| DailyStruct   |    25.45 ns | 0.077 ns | 0.064 ns |         - |
| WeeklyStruct  |   128.13 ns | 0.249 ns | 0.221 ns |         - |
| MonthlyStruct | 1,080.20 ns | 1.520 ns | 1.269 ns |         - |
| YearlyStruct  |   119.71 ns | 0.206 ns | 0.192 ns |         - |
| DailyBoxed    |    25.45 ns | 0.065 ns | 0.058 ns |         - |
