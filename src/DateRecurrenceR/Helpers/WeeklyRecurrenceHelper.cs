using DateRecurrenceR.Core;
using DateRecurrenceR.Extensions;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Helpers;

internal struct WeeklyRecurrenceHelper
{
    public static WeeklyHash GetPatternHash(WeekDays weekDays, int interval, DayOfWeek firstDayOfWeek)
    {
        var hash = new WeeklyHash();

        var increment = DaysInWeek * (interval - 1) + 1;

        var weekDayIndex = ((int)weekDays.GetMinByFirstDayOfWeek(firstDayOfWeek) + 6) % DaysInWeek;

        for (var i = DaysInWeek; i > 0; i--)
        {
            switch (weekDayIndex)
            {
                case 0:
                    if (weekDays[DayOfWeek.Sunday])
                    {
                        hash[DayOfWeek.Sunday] = increment;
                        increment = 1;
                    }
                    else
                    {
                        hash[DayOfWeek.Sunday] = 0;
                        increment++;
                    }

                    break;
                case 1:
                    if (weekDays[DayOfWeek.Monday])
                    {
                        hash[DayOfWeek.Monday] = increment;
                        increment = 1;
                    }
                    else
                    {
                        hash[DayOfWeek.Monday] = 0;
                        increment++;
                    }

                    break;
                case 2:
                    if (weekDays[DayOfWeek.Tuesday])
                    {
                        hash[DayOfWeek.Tuesday] = increment;
                        increment = 1;
                    }
                    else
                    {
                        hash[DayOfWeek.Tuesday] = 0;
                        increment++;
                    }

                    break;
                case 3:
                    if (weekDays[DayOfWeek.Wednesday])
                    {
                        hash[DayOfWeek.Wednesday] = increment;
                        increment = 1;
                    }
                    else
                    {
                        hash[DayOfWeek.Wednesday] = 0;
                        increment++;
                    }

                    break;
                case 4:
                    if (weekDays[DayOfWeek.Thursday])
                    {
                        hash[DayOfWeek.Thursday] = increment;
                        increment = 1;
                    }
                    else
                    {
                        hash[DayOfWeek.Thursday] = 0;
                        increment++;
                    }

                    break;
                case 5:
                    if (weekDays[DayOfWeek.Friday])
                    {
                        hash[DayOfWeek.Friday] = increment;
                        increment = 1;
                    }
                    else
                    {
                        hash[DayOfWeek.Friday] = 0;
                        increment++;
                    }

                    break;
                case 6:
                    if (weekDays[DayOfWeek.Saturday])
                    {
                        hash[DayOfWeek.Saturday] = increment;
                        increment = 1;
                    }
                    else
                    {
                        hash[DayOfWeek.Saturday] = 0;
                        increment++;
                    }

                    break;
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
        var testCount = GetCount(startDate, weekDays, interval, count);

        var endDayNumber = GetEndDateNumber(startDate, weekDays, interval, testCount);

        return (DateOnly.FromDayNumber(endDayNumber), testCount);
    }

    public static (DateOnly, int) GetEndDateAndCount(
        DateOnly startDate,
        WeekDays weekDays,
        DayOfWeek firstDayOfWeek,
        int interval,
        DateOnly endDate)
    {
        var testCount = GetCount(startDate, weekDays, firstDayOfWeek, interval, endDate);
        var endDayNumber = GetEndDateNumber(startDate, weekDays, interval, testCount);

        return (DateOnly.FromDayNumber(endDayNumber), testCount);
    }

    internal static int GetEndDateNumber(in DateOnly startDate, in WeekDays weekDays, in int interval, in int daysCount)
    {
        if (daysCount == 1)
        {
            return startDate.DayNumber;
        }

        var endDayNumber = startDate.DayNumber;
        var periodDays = DaysInWeek * interval;
        var periodsCount = daysCount / weekDays.CountOfSelected;
        var tailDaysCount = daysCount % weekDays.CountOfSelected;

        if (periodsCount > 1)
        {
            endDayNumber += (periodsCount - 1) * periodDays;
        }

        int selectedIndex;
        if (tailDaysCount == 0)
        {
            selectedIndex = weekDays.GetCountOfSelected(6, startDate.DayOfWeek) - 1;
        }
        else
        {
            selectedIndex = tailDaysCount - 1;
            endDayNumber += periodDays;
        }

        weekDays.TryGet(selectedIndex, startDate.DayOfWeek, out var x);
        var xx = WeekDaysHelper.GetDiffToDay(startDate.DayOfWeek, x);
        return endDayNumber + xx;
    }

    internal static int GetEndDateNumber(in DateOnly startDate, in WeekDays weekDays, DayOfWeek firstDayOfWeek,
        in int interval, in DateOnly endDate)
    {
        if (startDate == endDate)
        {
            return startDate.DayNumber;
        }

        var endDayNumber = startDate.DayNumber;
        var periodDays = DaysInWeek * interval;
        var daysCount = endDate.DayNumber - startDate.DayNumber + 1;
        var periodsCount = daysCount / periodDays;
        var tailDaysCount = daysCount % periodDays;

        if (periodsCount > 1)
        {
            endDayNumber += (periodsCount - 1) * periodDays;
        }

        DayOfWeek selectedIndexA;
        int selectedIndex;
        if (tailDaysCount == 0)
        {
            selectedIndex = weekDays.GetCountOfSelected(6, startDate.DayOfWeek);
        }
        else if (tailDaysCount >= DaysInWeek)
        {
            endDayNumber += periodDays;
            var di = WeekDaysHelper.DayToIndex(firstDayOfWeek.Prev(), startDate.DayOfWeek);
            selectedIndex = weekDays.GetCountOfSelected(di, startDate.DayOfWeek);
        }
        else
        {
            endDayNumber += periodDays;
            selectedIndex = weekDays.GetCountOfSelected(tailDaysCount, startDate.DayOfWeek);
        }

        weekDays.TryGet(selectedIndex - 1, startDate.DayOfWeek, out selectedIndexA);
        var xx = WeekDaysHelper.GetDiffToDay(startDate.DayOfWeek, selectedIndexA);
        return endDayNumber + xx;
    }

    internal static int GetCount(DateOnly startDate, WeekDays weekDays, DayOfWeek firstDayOfWeek, int interval,
        DateOnly endDate)
    {
        var actualCount = 0;

        var periodDays = DaysInWeek * interval;
        var rangeDaysCount = endDate.DayNumber - startDate.DayNumber + 1;

        var desiredPeriodCount = rangeDaysCount / periodDays;
        var tailDaysCount = rangeDaysCount % periodDays;

        actualCount += desiredPeriodCount * weekDays.CountOfSelected;

        if (tailDaysCount == 0)
        {
            return actualCount;
        }

        if (tailDaysCount < DaysInWeek)
        {
            var c = weekDays.GetCountOfSelected(tailDaysCount - 1, startDate.DayOfWeek);

            return actualCount + c;
        }

        var cc = weekDays.GetCountOfSelected(firstDayOfWeek.Prev(), startDate.DayOfWeek);

        return actualCount + cc;
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