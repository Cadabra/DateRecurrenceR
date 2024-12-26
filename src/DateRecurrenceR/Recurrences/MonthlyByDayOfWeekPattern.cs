using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

public readonly struct MonthlyByDayOfWeekPattern
{
    public MonthlyByDayOfWeekPattern(Interval interval, DayOfWeek dayOfWeek, IndexOfDay indexOfDay)
    {
        Interval = interval;
        DayOfWeek = dayOfWeek;
        IndexOfDay = indexOfDay;
    }

    public Interval Interval { get; }
    public DayOfWeek DayOfWeek { get; }
    public IndexOfDay IndexOfDay { get; }
}