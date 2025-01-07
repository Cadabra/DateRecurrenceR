using DateRecurrenceR.Core;

namespace DateRecurrenceR.Helpers;

internal struct YearlyRecurrenceHelper
{
    public static bool TryGetStartDate(DateOnly beginDate,
        DateOnly fromDate,
        int dayOfYear,
        int interval,
        out DateOnly startDate)
    {
        var deltaToStartYear = fromDate.Year - beginDate.Year;
        var yearsDiff = deltaToStartYear / interval * interval;

        if (deltaToStartYear % interval > 0 || fromDate.DayOfYear > dayOfYear) yearsDiff += interval;

        if (DateOutOfRangeByYear(beginDate, yearsDiff))
        {
            startDate = default;
            return false;
        }

        beginDate = beginDate.AddYears(yearsDiff);

        startDate = DateHelper.GetDateByDayOfYear(beginDate.Year, dayOfYear);

        return true;
    }

    public static (DateOnly _stopDate, int _count) GetEndDateAndCount(
        DateOnly startDate,
        MonthOfYear patternMonthOfYear,
        DayOfMonth patternDayOfMonth,
        Interval patternIndexOfDay,
        int count)
    {
        throw new NotImplementedException();
    }

    public static (DateOnly _stopDate, int _count) GetEndDateAndCount(
        DateOnly startDate,
        MonthOfYear patternMonthOfYear,
        DayOfMonth patternDayOfMonth,
        Interval patternIndexOfDay,
        DateOnly endDate)
    {
        throw new NotImplementedException();
    }

    public static (DateOnly _stopDate, int _count) GetEndDateAndCount(
        DateOnly startDate,
        int dayOfYear,
        Interval interval,
        int count)
    {
        throw new NotImplementedException();
    }

    public static (DateOnly _stopDate, int _count) GetEndDateAndCount(
        DateOnly startDate,
        int dayOfYear,
        Interval interval,
        DateOnly endDate)
    {
        throw new NotImplementedException();
    }

    public static (DateOnly _stopDate, int _count) GetEndDateAndCount(
        DateOnly startDate,
        MonthOfYear patternMonthOfYear,
        DayOfWeek patternDayOfWeek,
        IndexOfDay patternIndexOfDay,
        Interval patternInterval,
        int rangeCount)
    {
        throw new NotImplementedException();
    }

    public static (DateOnly _stopDate, int _count) GetEndDateAndCount(
        DateOnly startDate,
        MonthOfYear patternMonthOfYear,
        DayOfWeek patternDayOfWeek,
        IndexOfDay patternIndexOfDay,
        Interval patternInterval,
        DateOnly endDate)
    {
        throw new NotImplementedException();
    }

    private static bool DateOutOfRangeByYear(DateOnly beginDate, int addYear)
    {
        return DateOnly.MaxValue.Year - beginDate.Year < addYear;
    }
}