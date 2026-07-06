```

BenchmarkDotNet v0.15.8, macOS Tahoe 26.5.1 (25F80) [Darwin 25.5.0]
Apple M1 Pro, 1 CPU, 8 logical and 8 physical cores
.NET SDK 10.0.301
  [Host] : .NET 10.0.9 (10.0.9, 10.0.926.27113), Arm64 RyuJIT armv8.0-a

Toolchain=InProcessEmitToolchain  

```
| Method        | Mean      | Error    | StdDev   | Median    | Allocated |
|-------------- |----------:|---------:|---------:|----------:|----------:|
| DailyStruct   |  24.46 ns | 0.041 ns | 0.034 ns |  24.45 ns |         - |
| WeeklyStruct  | 135.53 ns | 0.606 ns | 0.567 ns | 135.26 ns |         - |
| MonthlyStruct | 180.44 ns | 2.012 ns | 1.882 ns | 179.80 ns |         - |
| YearlyStruct  | 111.70 ns | 3.308 ns | 9.110 ns | 106.93 ns |         - |
| DailyBoxed    |  25.18 ns | 0.083 ns | 0.078 ns |  25.16 ns |         - |
