using BenchmarkDotNet.Attributes;
using DateRecurrenceR;
using DateRecurrenceR.Core;

namespace Benchmarks;

/// <summary>
/// Verifies heap behaviour of the enumerator families. Read the <c>Allocated</c> column:
/// <c>-</c> (0 B) means the enumerator lived entirely on the stack.
/// </summary>
[MemoryDiagnoser]
public class AllocationBenchmark
{
    private static readonly DateOnly Begin = new(2024, 1, 1);
    private readonly WeekDays _weekDays = new(DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday);
    private readonly Interval _interval = new(1);

    // concrete struct enumerator, no interface — expected 0 B allocated
    [Benchmark]
    public long DailyStruct()
    {
        var sum = 0L;
        var e = Recurrence.Daily(Begin, 30, _interval);
        while (e.MoveNext()) sum += e.Current.DayNumber;
        return sum;
    }

    // concrete struct enumerator — expected 0 B allocated
    [Benchmark]
    public long WeeklyStruct()
    {
        var sum = 0L;
        var e = Recurrence.Weekly(Begin, 30, _weekDays, DayOfWeek.Monday, _interval);
        while (e.MoveNext()) sum += e.Current.DayNumber;
        return sum;
    }

    // concrete struct enumerator — expected 0 B allocated
    [Benchmark]
    public long MonthlyStruct()
    {
        var sum = 0L;
        var e = Recurrence.Monthly(Begin, 30, new DayOfMonth(15), _interval);
        while (e.MoveNext()) sum += e.Current.DayNumber;
        return sum;
    }

    // concrete struct enumerator — expected 0 B allocated
    [Benchmark]
    public long YearlyStruct()
    {
        var sum = 0L;
        var e = Recurrence.Yearly(Begin, 30, new DayOfYear(100), _interval);
        while (e.MoveNext()) sum += e.Current.DayNumber;
        return sum;
    }

    // assigning the struct to the interface boxes it — one 40 B box on the heap
    [Benchmark]
    public long DailyBoxed()
    {
        var sum = 0L;
        IEnumerator<DateOnly> e = Recurrence.Daily(Begin, 30, _interval);
        while (e.MoveNext()) sum += e.Current.DayNumber;
        return sum;
    }
}
