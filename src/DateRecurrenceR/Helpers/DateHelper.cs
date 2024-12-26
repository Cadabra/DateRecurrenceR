using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
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
    public static int CalculateDaysToNextInterval(int startDay, int fromDay, int interval, WeeklyHash weeklyHash)
    {
        Debug.Assert(fromDay >= startDay);

        var daysInInterval = DaysInWeek * interval;
        var daysDif = fromDay - startDay;

        var modDif = daysDif / daysInInterval * daysInInterval;

        if (daysDif <= modDif) return modDif;

        var dayOfWeek = (DayOfWeek)(((uint)startDay + modDif + 1) % DaysInWeek);
        modDif += weeklyHash[dayOfWeek];
        if (daysDif <= modDif) return modDif;

        dayOfWeek = (DayOfWeek)(((uint)startDay + modDif + 1) % DaysInWeek);
        modDif += weeklyHash[dayOfWeek];
        if (daysDif <= modDif) return modDif;

        dayOfWeek = (DayOfWeek)(((uint)startDay + modDif + 1) % DaysInWeek);
        modDif += weeklyHash[dayOfWeek];
        if (daysDif <= modDif) return modDif;

        dayOfWeek = (DayOfWeek)(((uint)startDay + modDif + 1) % DaysInWeek);
        modDif += weeklyHash[dayOfWeek];
        if (daysDif <= modDif) return modDif;

        dayOfWeek = (DayOfWeek)(((uint)startDay + modDif + 1) % DaysInWeek);
        modDif += weeklyHash[dayOfWeek];
        if (daysDif <= modDif) return modDif;

        dayOfWeek = (DayOfWeek)(((uint)startDay + modDif + 1) % DaysInWeek);
        modDif += weeklyHash[dayOfWeek];
        if (daysDif <= modDif) return modDif;

        dayOfWeek = (DayOfWeek)(((uint)startDay + modDif + 1) % DaysInWeek);
        modDif += weeklyHash[dayOfWeek];

        return modDif;
    }
}