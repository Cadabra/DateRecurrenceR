// Console.WriteLine(WeekDaysHelper.GetDiffToDay(DayOfWeek.Monday, DayOfWeek.Sunday));
// Console.WriteLine(DateOnly.MinValue.ToString("yyyy-M-d dddd"));
// Console.WriteLine(DateOnly.MaxValue.ToString("yyyy-M-d dddd"));

using BenchmarkDotNet.Running;
using Benchmarks;

var summary = BenchmarkRunner.Run<Benchmark>();