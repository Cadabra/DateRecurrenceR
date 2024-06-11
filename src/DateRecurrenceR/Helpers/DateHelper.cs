using System.Diagnostics.Contracts;

namespace DateRecurrenceR.Helpers;

internal static class DateHelper
{
    private const int DaysInWeek = 7;

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
    public static DateOnly GetDateByDayOfMonth(int year, int month, DayOfWeek dayOfWeek, NumberOfWeek numberOfWeek)
    {
        var date = new DateOnly(year, month, 1);
        var diffToFirstDay = (dayOfWeek - date.DayOfWeek + DaysInWeek) % DaysInWeek;

        var weekMultiplicand = numberOfWeek switch
        {
            NumberOfWeek.First => 0,
            NumberOfWeek.Second => 1,
            NumberOfWeek.Third => 2,
            NumberOfWeek.Fourth => 3,
            NumberOfWeek.Last => 4 - (DateTime.DaysInMonth(date.Year, date.Month) % 7 <= diffToFirstDay ? 1 : 0),
            _ => throw new ArgumentOutOfRangeException(nameof(numberOfWeek), numberOfWeek, null)
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