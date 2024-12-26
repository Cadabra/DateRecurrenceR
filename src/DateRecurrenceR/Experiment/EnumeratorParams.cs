using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Experiment;

internal delegate DateOnly GetNewNextDateDelegate(DateOnly date);

public struct EnumeratorParams
{
    // public int[] Params { get; set; }
    public int P1 { get; init; }
    public int P2 { get; init; }
    public int P3 { get; init; }
    public int P4 { get; init; }
    internal GetNewNextDateDelegate GetNextDate { get; init; }

    private EnumeratorParams(int p1, int p2, int p3, int p4, GetNewNextDateDelegate getNextDate)
    {
        P1 = p1;
        P2 = p2;
        P3 = p3;
        P4 = p4;
        GetNextDate = getNextDate;
    }

    internal static EnumeratorParams NewDaily(DateOnly startDate, int takeCount, int interval,
        GetNewNextDateDelegate getNextDate)
    {
        return new EnumeratorParams(startDate.DayNumber, takeCount, interval, 0, getNextDate);
    }

    internal static EnumeratorParams NewWeekly(DateOnly start, int takeCount, GetNewNextDateDelegate getNextDate)
    {
        return new EnumeratorParams(start.DayNumber, takeCount, 0, 0, getNextDate);
    }

    internal static EnumeratorParams NewMonthly(DateOnly startDate, int takeCount, int interval,
        GetNewNextDateDelegate getNextDate)
    {
        return new EnumeratorParams(startDate.DayNumber, takeCount, interval, 0, getNextDate);
    }

    internal static EnumeratorParams NewYearly(DateOnly startDate, int takeCount, int interval,
        GetNewNextDateDelegate getNextDate)
    {
        return new EnumeratorParams(startDate.DayNumber, takeCount, interval, 0, getNextDate);
    }

    private static readonly GetNewNextDateDelegate EmptyDelegate = (_) => DateOnly.MinValue;
}