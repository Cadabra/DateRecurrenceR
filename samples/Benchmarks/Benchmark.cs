using BenchmarkDotNet.Attributes;
using DateRecurrenceR;
using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using Range = DateRecurrenceR.Core.Range;

namespace Benchmarks;

[MemoryDiagnoser]
public class Benchmark
{
    [Params(100, 10_000, 1_000_000)] public int Count { get; set; }
    private readonly WeekDays _weekDays = new(true, true, true, true, true, true, true);
    private readonly Interval _interval = new(1);

    // [Benchmark]
    public long Interface()
    {
        var sum = 0L;
        var rec = Recurrence.Monthly(DateOnly.MinValue, Count, new DayOfMonth(1), _interval);

        while (rec.MoveNext())
        {
            sum += rec.Current.DayNumber;
        }

        return sum;
    }
    
    // [Benchmark]
    public long Interface2()
    {
        var sum = 0L;
        var rec = Recurrence.Weekly(DateOnly.MinValue, Count, _weekDays, DayOfWeek.Monday, _interval);
    
        while (rec.MoveNext())
        {
            sum += rec.Current.DayNumber;
        }
    
        return sum;
    }
    
    [Benchmark]
    public long Structure2()
    {
        var sum = 0L;
        var pattern = new WeeklyByWeekDaysPattern(_interval, _weekDays, DayOfWeek.Monday);
    
        var sut = WeeklyByWeekDaysRecurrence.New(new Range(DateOnly.MinValue, Count), pattern);

        sut.Contains(DateOnly.MinValue);
            
        using var rec = sut.GetEnumerator();
    
        while (rec.MoveNext())
        {
            sum += rec.Current.DayNumber;
        }
    
        return sum;
    }
}