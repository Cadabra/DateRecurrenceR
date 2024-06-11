namespace DateRecurrenceR.Helpers;

internal struct WeekDaysHelper
{
    public static int GetDiffToDay(DayOfWeek firstDayOfWeek, DayOfWeek dayOfWeek)
    {
        return (7 + (int) dayOfWeek - (int) firstDayOfWeek) % 7;
    }
}