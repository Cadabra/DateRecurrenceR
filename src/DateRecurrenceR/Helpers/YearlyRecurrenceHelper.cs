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

        startDate = DateOnlyHelper.GetDateByDayOfYear(beginDate.Year, dayOfYear);

        return true;
    }

    public static (DateOnly stopDate, int count) GetEndDateAndCount(
        DateOnly startDate,
        int dayOfYear,
        Interval interval,
        int count)
    {
        var yearsDiff = DateOnly.MaxValue.Year - startDate.Year;
        
        var actualCount = Math.Min(yearsDiff / interval, count);

        var stopDateYear = startDate.Year + actualCount * interval;

        return (new DateOnly(stopDateYear, 1, 1).AddDays(dayOfYear - 1), actualCount);
    }

    public static (DateOnly stopDate, int count) GetEndDateAndCount(
        DateOnly startDate,
        int dayOfYear,
        Interval interval,
        DateOnly endDate)
    {
        var yearsDiff = endDate.Year - startDate.Year;
        var actualCount = yearsDiff / interval;

        if (yearsDiff % interval == 0 && endDate.DayOfYear < dayOfYear)
        {
            actualCount--;
        }

        var stopDateYear = startDate.Year + actualCount * interval;
        var stopDate = new DateOnly(stopDateYear, 1, 1).AddDays(dayOfYear - 1);
        return (stopDate, actualCount);
    }

    public static (DateOnly stopDate, int count) GetEndDateAndCount(
        DateOnly startDate,
        MonthOfYear monthOfYear,
        DayOfMonth dayOfMonth,
        Interval interval,
        int count)
    {
        var yearsDiff = DateOnly.MaxValue.Year - startDate.Year;
        var actualCount = Math.Min(yearsDiff / interval, count);

        var stopDateYear = startDate.Year + actualCount * interval;
        var stopDate = DateOnlyHelper.GetDateByDayOfMonth(stopDateYear, monthOfYear, dayOfMonth);

        return (stopDate, actualCount);
    }

    public static (DateOnly stopDate, int count) GetEndDateAndCount(
        DateOnly startDate,
        MonthOfYear monthOfYear,
        DayOfMonth dayOfMonth,
        Interval interval,
        DateOnly endDate)
    {
        var yearsDiff = endDate.Year - startDate.Year;
        var actualCount = yearsDiff / interval;

        var stopDateYear = startDate.Year + actualCount * interval;
        var stopDate = DateOnlyHelper.GetDateByDayOfMonth(stopDateYear, monthOfYear, dayOfMonth);
        if (yearsDiff % interval == 0 && stopDate < endDate )
        {
            actualCount--;
        }

        stopDateYear = startDate.Year + actualCount * interval;
        stopDate = DateOnlyHelper.GetDateByDayOfMonth(stopDateYear, monthOfYear, dayOfMonth);
        return (stopDate, actualCount);
    }

    public static (DateOnly stopDate, int count) GetEndDateAndCount(
        DateOnly startDate,
        MonthOfYear monthOfYear,
        DayOfWeek dayOfWeek,
        IndexOfDay indexOfDay,
        Interval interval,
        int count)
    {
        var yearsDiff = DateOnly.MaxValue.Year - startDate.Year;
        var actualCount = Math.Min(yearsDiff / interval, count);

        var stopDateYear = startDate.Year + actualCount * interval;
        var stopDate = DateOnlyHelper.GetDateByDayOfMonth(stopDateYear, monthOfYear, dayOfWeek, indexOfDay);

        return (stopDate, actualCount);
    }

    public static (DateOnly stopDate, int count) GetEndDateAndCount(
        DateOnly startDate,
        MonthOfYear monthOfYear,
        DayOfWeek dayOfWeek,
        IndexOfDay indexOfDay,
        Interval interval,
        DateOnly endDate)
    {
        var yearsDiff = endDate.Year - startDate.Year;
        var actualCount = yearsDiff / interval;

        var stopDateYear = startDate.Year + actualCount * interval;
        var stopDate = DateOnlyHelper.GetDateByDayOfMonth(stopDateYear, monthOfYear, dayOfWeek, indexOfDay);
        if (yearsDiff % interval == 0 && stopDate < endDate )
        {
            actualCount--;
        }

        stopDateYear = startDate.Year + actualCount * interval;
        stopDate = DateOnlyHelper.GetDateByDayOfMonth(stopDateYear, monthOfYear, dayOfWeek, indexOfDay);
        return (stopDate, actualCount);    }

    private static bool DateOutOfRangeByYear(DateOnly beginDate, int addYear)
    {
        return DateOnly.MaxValue.Year - beginDate.Year < addYear;
    }
}