namespace DateRecurrenceR.Extensions;

internal static class DayOfWeekExtension
{
    public static DayOfWeek Next(this DayOfWeek dayOfWeek)
    {
        return (DayOfWeek)((8 + (int)dayOfWeek) % 7);
    }
    public static DayOfWeek Prev(this DayOfWeek dayOfWeek)
    {
        return (DayOfWeek)((6 + (int)dayOfWeek) % 7);
    }
}