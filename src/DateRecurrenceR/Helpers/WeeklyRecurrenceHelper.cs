using DateRecurrenceR.Core;
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
        var intervalDayCount = 7 * interval;

        var actualCount = weekDays.GetCountFrom(startDate.DayOfWeek, firstDayOfWeek);

        if (!DateHelper.TryGetFirstDayOfNextWeek(startDate, firstDayOfWeek, out DateOnly nextWeekFirstDay))
        {
            return (default, default);
        }

        var daysInInterval = weekDays.GetCount();
        var intervalCount = (count - actualCount) / daysInInterval;

        var safeCount = Math.Min((DateOnly.MaxValue.DayNumber - nextWeekFirstDay.DayNumber) / intervalDayCount, intervalCount);
        var endDayNumber = nextWeekFirstDay.DayNumber + safeCount * intervalCount;

        var tail = Math.Min(7, DateOnly.MaxValue.DayNumber - endDayNumber);
        var mt = WeekDaysHelper.IndexToDayOfWeek(tail, firstDayOfWeek);
        if (weekDays.TryGetDayFromLeft(mt, firstDayOfWeek, out var result))
        {
            safeCount += weekDays.GetCountBefore(result, firstDayOfWeek);
            endDayNumber += WeekDaysHelper.DayToIndex(result, firstDayOfWeek);
        }
        
        return (DateOnly.FromDayNumber(endDayNumber), safeCount);
    }

    public static (DateOnly, int) GetEndDateAndCount(
        DateOnly startDate,
        WeekDays weekDays,
        DayOfWeek firstDayOfWeek,
        int interval,
        DateOnly endDate)
    {
        var intervalDayCount = 7 * interval;

        var count = 0;
        if (DateHelper.TryGetFirstDayOfNextWeek(startDate, firstDayOfWeek, out DateOnly firstDateOfNextWeek))
        {
            count = (endDate.DayNumber - firstDateOfNextWeek.DayNumber) / intervalDayCount;
        }

        count += weekDays.GetCountFrom(startDate.DayOfWeek, firstDayOfWeek);

        var actualEndDayNumber = firstDateOfNextWeek.DayNumber + intervalDayCount * count;
        var tail = (endDate.DayNumber - firstDateOfNextWeek.DayNumber) % intervalDayCount;
        if (tail > 7)
        {
            count += weekDays.GetCount();
            var lastSelectedDay = weekDays.GetMaxDay(firstDayOfWeek);
            actualEndDayNumber += WeekDaysHelper.GetDiffToDay(firstDayOfWeek, lastSelectedDay);
        }
        else if (weekDays.TryGetDayFromLeft(endDate.DayOfWeek, firstDayOfWeek, out var result))
        {
            count += weekDays.GetCountBefore(endDate.DayOfWeek, firstDayOfWeek);
            actualEndDayNumber += WeekDaysHelper.GetDiffToDay(firstDayOfWeek, result);
        }

        return (DateOnly.FromDayNumber(actualEndDayNumber), count);
    }
}