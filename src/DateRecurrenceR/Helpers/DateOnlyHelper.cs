using System.Diagnostics.Contracts;
using DateRecurrenceR.Core;

namespace DateRecurrenceR.Helpers;

internal struct DateOnlyHelper
{
    [Pure]
    public static DateOnly GetDateByDayOfMonth(int year, int month, DayOfWeek dayOfWeek, IndexOfDay indexOfDay)
    {
        var date = new DateOnly(year, month, 1);
        var diffToFirstDay = (dayOfWeek - date.DayOfWeek + DaysInWeek) % DaysInWeek;

        var weekMultiplicand = indexOfDay switch
        {
            IndexOfDay.First => 0,
            IndexOfDay.Second => 1,
            IndexOfDay.Third => 2,
            IndexOfDay.Fourth => 3,
            IndexOfDay.Last => 4 -
                               (DateTime.DaysInMonth(date.Year, date.Month) % DaysInWeek <= diffToFirstDay ? 1 : 0),
            _ => throw new ArgumentOutOfRangeException(nameof(indexOfDay), indexOfDay, null)
        };

        var dayDiff = diffToFirstDay + DaysInWeek * weekMultiplicand;

        return date.AddDays(dayDiff);
    }

    [Pure]
    public static DateOnly GetDateByDayOfMonth(int year, int month, int dayOfMonth)
    {
        var saveDay = dayOfMonth > 28
            ? Math.Min(dayOfMonth, DateTime.DaysInMonth(year, month))
            : dayOfMonth;

        return new DateOnly(year, month, saveDay);
    }

    [Pure]
    public static DateOnly GetDateByDayOfYear(int year, int dayOfYear)
    {
        const int feb = 2;
        var saveDay = dayOfYear > 365
            ? DateTime.DaysInMonth(year, feb) > 28 ? dayOfYear : 365
            : dayOfYear;

        return new DateOnly(year, 1, 1).AddDays(saveDay - 1);
    }
}