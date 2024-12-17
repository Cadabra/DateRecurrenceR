using System.Diagnostics;
using System.Diagnostics.Contracts;
using DateRecurrenceR.Core;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Helpers;

internal struct DateHelper
{
    [Pure]
    public static bool TryGetDateOfDayOfWeek(DateOnly dateInSameWeek,
        DayOfWeek dayOfWeek,
        DayOfWeek firstDayOfWeek,
        out DateOnly result)
    {
        var diffToDay = WeekDaysHelper.GetDiffToDay(firstDayOfWeek, dayOfWeek);
        var startDayNumber = dateInSameWeek.DayNumber -
                             WeekDaysHelper.GetDiffToDay(firstDayOfWeek, dateInSameWeek.DayOfWeek);
        var newDayNumber = startDayNumber + diffToDay;
        if (newDayNumber < DateOnly.MinValue.DayNumber || newDayNumber > DateOnly.MaxValue.DayNumber)
        {
            result = default;
            return false;
        }

        result = DateOnly.FromDayNumber(newDayNumber);
        return true;
    }

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

    [Pure]
    public static int CalculateDaysToNextInterval(int startDay, int fromDay, int interval, WeeklyHash weeklyHash)
    {
        Debug.Assert(fromDay >= startDay);

        var daysInInterval = DaysInWeek * interval;
        var daysDif = fromDay - startDay;

        var modDif = daysDif / daysInInterval * daysInInterval;

        for (var i = 0; (i < DaysInWeek) & (daysDif > modDif); i++)
        {
            var dayOfWeek = (DayOfWeek) (((uint) startDay + modDif + 1) % DaysInWeek);
            modDif += weeklyHash[dayOfWeek];
        }

        return modDif;
    }
}