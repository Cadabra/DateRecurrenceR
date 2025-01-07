using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

public readonly struct YearlyByDayOfYearPattern
{
    public YearlyByDayOfYearPattern(Interval interval, DayOfYear dayOfYear)
    {
        Interval = interval;
        DayOfYear = dayOfYear;
    }

    public Interval Interval { get; }
    public DayOfYear DayOfYear { get; }
}