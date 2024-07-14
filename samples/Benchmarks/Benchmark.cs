using BenchmarkDotNet.Attributes;
using DateRecurrenceR;
using DateRecurrenceR.Core;

namespace Benchmarks;

[MemoryDiagnoser]
public class Benchmark
{
    [Params(100, 10_000, 1_000_000)] public int Count { get; set; }

    [Benchmark]
    public long Rec()
    {
        var sum = 0L;
        var w = new WeekDaysArray();
        w[0] = true;
        var rec = Recurrence.Yearly(DateOnly.MinValue, DateOnly.MaxValue, Count, new DayOfYear(1), new Interval(1));

        while (rec.MoveNext())
        {
            sum += rec.Current.DayNumber;
        }

        return sum;
    }
}