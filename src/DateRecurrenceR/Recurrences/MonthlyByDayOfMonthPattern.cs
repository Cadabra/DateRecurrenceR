using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

public readonly struct MonthlyByDayOfMonthPattern
{
    public MonthlyByDayOfMonthPattern(Interval interval, DayOfMonth dayOfMonth)
    {
        Interval = interval;
        DayOfMonth = dayOfMonth;
    }

    public Interval Interval { get; }
    public DayOfMonth DayOfMonth { get; }
}