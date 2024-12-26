using System.Runtime.CompilerServices;
using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Experiment.Helpers;

internal struct DateEnumeratorHelper
{
    public static bool TryGetDailyParams(DateOnly beginDate,
        DateOnly fromDate,
        int count,
        Interval interval,
        out EnumeratorParams parameters
    )
    {
        var canStart = DailyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            interval,
            out var startDate);

        if (!canStart)
        {
            parameters = new EnumeratorParams();
            return false;
        }

        var expectedCount = count;
        var maxCount = (DateOnly.MaxValue.DayNumber - startDate.DayNumber) / interval + 1;

        var actualCount = Math.Min(expectedCount, maxCount);

        parameters = EnumeratorParams.NewDaily(startDate, actualCount, interval, GetNextDate);
        return true;

        DateOnly GetNextDate(DateOnly date)
        {
            if (DateOnly.MaxValue.DayNumber - date.DayNumber < interval)
                date = DateOnly.MinValue;
            else
                date = date.AddDays(interval);

            return date;
        }
    }

    public static bool TryGetWeeklyParams(DateOnly beginDate,
        DateOnly fromDate,
        int count,
        WeekDays weekDays,
        DayOfWeek firstDayOfWeek,
        Interval interval,
        out EnumeratorParams parameters
    )
    {
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval, firstDayOfWeek);

        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            patternHash,
            weekDays,
            firstDayOfWeek,
            interval,
            out var startDate);

        if (!canStart)
        {
            parameters = new EnumeratorParams();
            return false;
        }

        var expectedCount = count;
        var maxCount = (DateOnly.MaxValue.DayNumber - startDate.DayNumber) / interval + 1;

        var actualCount = Math.Min(expectedCount, maxCount);

        parameters = EnumeratorParams.NewWeekly(startDate, actualCount, GetDeg(patternHash));
        return true;

        // DateOnly GetNextDate(DateOnly date)
        // {
        //     date = date.AddDays(patternHash[date.DayOfWeek]);
        //
        //     return date;
        // }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static GetNewNextDateDelegate GetDeg(WeeklyHash hash)
    {
        
        return [MethodImpl(MethodImplOptions.AggressiveInlining)](DateOnly date) =>
        {
            date = date.AddDays(hash[date.DayOfWeek]);

            return date;
        };
    }

    // private static GetNewNextDateDelegate _getNextDate = (DateOnly date) =>
    // {
    //     date = date.AddDays(patternHash[date.DayOfWeek]);
    //
    //     return date;
    // };

    public static bool TryGetParams(DateOnly beginDate,
        DateOnly fromDate,
        int count,
        DayOfMonth dayOfMonth,
        Interval interval,
        out EnumeratorParams parameters
    )
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfMonth,
            interval,
            out var startDate);

        if (!canStart)
        {
            parameters = new EnumeratorParams();
            return false;
        }

        var (_, actualCount) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            startDate,
            dayOfMonth,
            interval,
            count);

        parameters = EnumeratorParams.NewMonthly(startDate, actualCount, interval, GetNextDate);

        return true;

        DateOnly GetNextDate(DateOnly date)
        {
            if (GetMonthNumber(DateOnly.MaxValue) - GetMonthNumber(date) < interval)
                date = DateOnly.MinValue;
            else
                date = date.AddMonths(interval);

            return DateOnlyHelper.GetDateByDayOfMonth(date.Year, date.Month, dayOfMonth);
        }
    }

    public static bool TryGetDateByDayOfMonthParams(DateOnly beginDate,
        DateOnly fromDate,
        int count,
        DayOfMonth dayOfMonth,
        MonthOfYear numberOfMonth,
        Interval interval,
        out EnumeratorParams parameters
    )
    {
        var date = DateOnlyHelper.GetDateByDayOfMonth(beginDate.Year, numberOfMonth, dayOfMonth);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            interval,
            out var startDate);

        if (!canStart)
        {
            parameters = new EnumeratorParams();
            return false;
        }

        var (_, actualCount) = YearlyRecurrenceHelper.GetEndDateAndCount(
            startDate,
            numberOfMonth,
            dayOfMonth,
            interval,
            count);

        parameters = EnumeratorParams.NewYearly(startDate, actualCount, interval, GetNextDate);
        return true;

        DateOnly GetNextDate(DateOnly date)
        {
            if (DateOnly.MaxValue.Year - date.Year < interval)
                date = DateOnly.MinValue;
            else
                date = date.AddYears(interval);

            return DateOnlyHelper.GetDateByDayOfMonth(date.Year, numberOfMonth, dayOfMonth);
        }
    }

    private static int GetMonthNumber(DateOnly date)
    {
        return MonthsInYear * date.Year + date.Month;
    }
}