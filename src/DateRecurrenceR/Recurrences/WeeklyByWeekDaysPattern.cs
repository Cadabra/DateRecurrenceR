using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

public readonly struct WeeklyByWeekDaysPattern
{
    public WeeklyByWeekDaysPattern(Interval interval, WeekDays weekDays, DayOfWeek firstDayOfWeek)
    {
        Interval = interval;
        WeekDays = weekDays;
        FirstDayOfWeek = firstDayOfWeek;
    }

    public Interval Interval { get; }
    public WeekDays WeekDays { get; }
    public DayOfWeek FirstDayOfWeek { get; }
}