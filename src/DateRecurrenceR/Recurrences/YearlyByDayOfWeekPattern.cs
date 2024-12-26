using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

public readonly struct YearlyByDayOfWeekPattern
{
    public YearlyByDayOfWeekPattern(
        Interval interval,
        DayOfWeek dayOfWeek,
        IndexOfDay indexOfDay,
        MonthOfYear monthOfYear)
    {
        Interval = interval;
        DayOfWeek = dayOfWeek;
        IndexOfDay = indexOfDay;
        MonthOfYear = monthOfYear;
    }

    public Interval Interval { get; }
    public DayOfWeek DayOfWeek { get; }
    public IndexOfDay IndexOfDay { get; }
    public MonthOfYear MonthOfYear { get; }
}