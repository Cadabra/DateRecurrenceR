using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

public readonly struct YearlyByDayOfMonthPattern
{
    public YearlyByDayOfMonthPattern(Interval interval, DayOfMonth dayOfMonth, MonthOfYear monthOfYear)
    {
        Interval = interval;
        DayOfMonth = dayOfMonth;
        MonthOfYear = monthOfYear;
    }

    public Interval Interval { get; }
    public DayOfMonth DayOfMonth { get; }
    public MonthOfYear MonthOfYear { get; }
}