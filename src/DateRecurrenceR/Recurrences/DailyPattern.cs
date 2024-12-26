using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

public readonly struct DailyPattern
{
    public DailyPattern(Interval interval)
    {
        Interval = interval;
    }

    public Interval Interval { get; }
}