```

BenchmarkDotNet v0.15.8, macOS Tahoe 26.5.1 (25F80) [Darwin 25.5.0]
Apple M1 Pro, 1 CPU, 8 logical and 8 physical cores
.NET SDK 10.0.301
  [Host] : .NET 10.0.9 (10.0.9, 10.0.926.27113), Arm64 RyuJIT armv8.0-a

Toolchain=InProcessEmitToolchain  

```
| Method        | Mean      | Error    | StdDev   | Allocated |
|-------------- |----------:|---------:|---------:|----------:|
| DailyStruct   |  25.08 ns | 0.026 ns | 0.023 ns |         - |
| WeeklyStruct  | 131.85 ns | 1.726 ns | 1.442 ns |         - |
| MonthlyStruct | 179.93 ns | 0.251 ns | 0.196 ns |         - |
| YearlyStruct  | 106.95 ns | 0.098 ns | 0.077 ns |         - |
| DailyBoxed    |  26.04 ns | 0.530 ns | 0.689 ns |         - |
