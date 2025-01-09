namespace DateRecurrenceR.Helpers;

internal struct WeekDaysHelper
{
    public static int GetDiffToDay(DayOfWeek firstDayOfWeek, DayOfWeek dayOfWeek)
    {
        return (DaysInWeek + (int) dayOfWeek - (int) firstDayOfWeek) % DaysInWeek;
    }

    public static DayOfWeek IndexToDayOfWeek(int index, DayOfWeek firstDayOfWeek)
    {
        return (DayOfWeek)((7 + index + (int)firstDayOfWeek) % 7);
    }

    public static int DayToIndex(DayOfWeek dayOfWeek, DayOfWeek firstDayOfWeek)
    {
        return (7 + (int)dayOfWeek - (int)firstDayOfWeek) % 7;
    }
}