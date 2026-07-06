using DateRecurrenceR.Core;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Helpers;

internal static class WeeklyRecurrenceHelper
{
    public static WeeklyHash GetPatternHash(WeekDays weekDays, int interval, DayOfWeek firstDayOfWeek)
    {
        var hash = new WeeklyHash();

        var increment = DaysInWeek * (interval - 1) + 1;

        var weekDayIndex = ((int)weekDays.GetMinByFirstDayOfWeek(firstDayOfWeek) + 6) % DaysInWeek;

        for (var i = DaysInWeek; i > 0; i--)
        {
            var day = (DayOfWeek)weekDayIndex;
            if (weekDays[day])
            {
                hash[day] = increment;
                increment = 1;
            }
            else
            {
                hash[day] = 0;
                increment++;
            }

            weekDayIndex = (DaysInWeek + weekDayIndex - 1) % DaysInWeek;
        }

        return hash;
    }

    public static bool TryGetStartDate(DateOnly beginDate,
        DateOnly fromDate,
        WeeklyHash weeklyHash,
        WeekDays weekDays,
        DayOfWeek firstDayOfWeek,
        int interval,
        out DateOnly startDate)
    {
        if (!DateHelper.TryGetDateOfDayOfWeek(beginDate, weekDays.GetMinByFirstDayOfWeek(firstDayOfWeek),
                firstDayOfWeek, out startDate))
        {
            startDate = default;
            return false;
        }

        if (startDate >= fromDate) return true;

        var difDays =
            DateHelper.CalculateDaysToNextInterval(startDate.DayNumber, fromDate.DayNumber, interval, weeklyHash);
        var startDay = startDate.DayNumber + difDays;

        if (startDay > DateOnly.MaxValue.DayNumber)
        {
            startDate = default;
            return false;
        }

        startDate = DateOnly.FromDayNumber(startDay);
        return true;
    }

    public static (DateOnly, int) GetEndDateAndCount(
        DateOnly startDate,
        WeekDays weekDays,
        DayOfWeek firstDayOfWeek,
        int interval,
        int count)
    {
        var daysCount = GetCount(startDate, weekDays, interval, count);
        var endDayNumber = GetEndDateNumber(startDate, weekDays, firstDayOfWeek, interval, daysCount);

        return (DateOnly.FromDayNumber(endDayNumber), daysCount);
    }

    public static (DateOnly, int) GetEndDateAndCount(
        DateOnly startDate,
        WeekDays weekDays,
        DayOfWeek firstDayOfWeek,
        int interval,
        DateOnly endDate)
    {
        var daysCount = GetCount(startDate, weekDays, firstDayOfWeek, interval, endDate);
        var endDayNumber = GetEndDateNumber(startDate, weekDays, firstDayOfWeek, interval, daysCount);

        return (DateOnly.FromDayNumber(endDayNumber), daysCount);
    }

    internal static int GetEndDateNumber(in DateOnly startDate, in WeekDays weekDays, DayOfWeek firstDayOfWeek,
        in int interval, in int daysCount)
    {
        if (daysCount == 1)
        {
            return startDate.DayNumber;
        }

        var lastIndex = daysCount - 1;
        var periodsCount = lastIndex / weekDays.CountOfSelected;
        var tailIndex = lastIndex % weekDays.CountOfSelected;

        return startDate.DayNumber
               + periodsCount * DaysInWeek * interval
               + GetSelectedDayOffset(weekDays, startDate.DayOfWeek, firstDayOfWeek, interval, tailIndex);
    }

    /// <summary>
    /// Returns the offset in days from the start day to the occurrence at <paramref name="selectedIndex"/>
    /// (zero-based, in enumeration order starting at the start day). Wrapping past the end of the week lands
    /// in the next on-grid week, skipping the off-grid weeks.
    /// </summary>
    private static int GetSelectedDayOffset(in WeekDays weekDays, DayOfWeek startDayOfWeek, DayOfWeek firstDayOfWeek,
        int interval, int selectedIndex)
    {
        weekDays.TryGet(selectedIndex, startDayOfWeek, out var day);
        var diff = WeekDaysHelper.GetDiffToDay(startDayOfWeek, day);

        if (WeekDaysHelper.DayToIndex(startDayOfWeek, firstDayOfWeek) + diff >= DaysInWeek)
        {
            diff += (interval - 1) * DaysInWeek;
        }

        return diff;
    }

    internal static int GetCount(DateOnly startDate, WeekDays weekDays, DayOfWeek firstDayOfWeek, int interval,
        DateOnly endDate)
    {
        if (endDate < startDate)
        {
            return 0;
        }

        var periodDays = DaysInWeek * interval;
        var rangeDaysCount = endDate.DayNumber - startDate.DayNumber + 1;

        var actualCount = rangeDaysCount / periodDays * weekDays.CountOfSelected;
        var tailDaysCount = rangeDaysCount % periodDays;

        for (var i = 0; i < weekDays.CountOfSelected; i++)
        {
            if (GetSelectedDayOffset(weekDays, startDate.DayOfWeek, firstDayOfWeek, interval, i) < tailDaysCount)
            {
                actualCount++;
            }
        }

        return actualCount;
    }

    internal static int GetCount(DateOnly startDate, WeekDays weekDays, int interval, int expectedCount)
    {
        if (expectedCount == 1)
        {
            return expectedCount;
        }

        var actualCount = 0;
        var actualPeriodCount = Math.Min(
            expectedCount / weekDays.CountOfSelected,
            (DateOnly.MaxValue.DayNumber - startDate.DayNumber) / (DaysInWeek * interval));

        actualCount += actualPeriodCount * weekDays.CountOfSelected;

        if (actualCount == expectedCount)
        {
            return actualCount;
        }

        var restDays = DateOnly.MaxValue.DayNumber - startDate.DayNumber - actualPeriodCount * weekDays.CountOfSelected;
        if (restDays >= DaysInWeek)
        {
            return expectedCount;
        }

        var c = weekDays.GetCountOfSelected(restDays - 1, startDate.DayOfWeek);

        return actualCount + c;
    }
}
