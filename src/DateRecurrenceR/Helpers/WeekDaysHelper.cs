namespace DateRecurrenceR.Helpers;

internal struct WeekDaysHelper
{
    public static int GetDiffToDay(DayOfWeek firstDayOfWeek, DayOfWeek dayOfWeek)
    {
        return (DaysInWeek + (int) dayOfWeek - (int) firstDayOfWeek) % DaysInWeek;
    }
}