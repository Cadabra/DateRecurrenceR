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
        if (fromDate < beginDate) fromDate = beginDate;

        var deltaToStartYear = fromDate.Year - beginDate.Year;
        var yearsDiff = deltaToStartYear / interval * interval;
        if (deltaToStartYear % interval > 0) yearsDiff += interval;

        if (DateOutOfRangeByYear(beginDate, yearsDiff))
        {
            startDate = default;
            return false;
        }

        startDate = DateOnlyHelper.GetDateByDayOfYear(beginDate.Year + yearsDiff, dayOfYear);

        if (startDate < fromDate)
        {
            yearsDiff += interval;

            if (DateOutOfRangeByYear(beginDate, yearsDiff))
            {
                startDate = default;
                return false;
            }

            startDate = DateOnlyHelper.GetDateByDayOfYear(beginDate.Year + yearsDiff, dayOfYear);
        }

        return true;
    }

    public static bool TryGetStartDate(DateOnly beginDate,
        DateOnly fromDate,
        MonthOfYear monthOfYear,
        DayOfMonth dayOfMonth,
        int interval,
        out DateOnly startDate)
    {
        if (fromDate < beginDate) fromDate = beginDate;

        var deltaToStartYear = fromDate.Year - beginDate.Year;
        var yearsDiff = deltaToStartYear / interval * interval;
        if (deltaToStartYear % interval > 0) yearsDiff += interval;

        if (DateOutOfRangeByYear(beginDate, yearsDiff))
        {
            startDate = default;
            return false;
        }

        startDate = DateOnlyHelper.GetDateByDayOfMonth(beginDate.Year + yearsDiff, monthOfYear, dayOfMonth);

        if (startDate < fromDate)
        {
            yearsDiff += interval;

            if (DateOutOfRangeByYear(beginDate, yearsDiff))
            {
                startDate = default;
                return false;
            }

            startDate = DateOnlyHelper.GetDateByDayOfMonth(beginDate.Year + yearsDiff, monthOfYear, dayOfMonth);
        }

        return true;
    }

    public static bool TryGetStartDate(DateOnly beginDate,
        DateOnly fromDate,
        MonthOfYear monthOfYear,
        DayOfWeek dayOfWeek,
        IndexOfDay indexOfDay,
        int interval,
        out DateOnly startDate)
    {
        if (fromDate < beginDate) fromDate = beginDate;

        var deltaToStartYear = fromDate.Year - beginDate.Year;
        var yearsDiff = deltaToStartYear / interval * interval;
        if (deltaToStartYear % interval > 0) yearsDiff += interval;

        if (DateOutOfRangeByYear(beginDate, yearsDiff))
        {
            startDate = default;
            return false;
        }

        startDate = DateOnlyHelper.GetDateByDayOfMonth(beginDate.Year + yearsDiff, monthOfYear, dayOfWeek, indexOfDay);

        if (startDate < fromDate)
        {
            yearsDiff += interval;

            if (DateOutOfRangeByYear(beginDate, yearsDiff))
            {
                startDate = default;
                return false;
            }

            startDate = DateOnlyHelper.GetDateByDayOfMonth(beginDate.Year + yearsDiff, monthOfYear, dayOfWeek, indexOfDay);
        }

        return true;
    }

    public static (DateOnly stopDate, int count) GetEndDateAndCount(
        DateOnly startDate,
        int dayOfYear,
        Interval interval,
        int count)
    {
        if (count < 1) return (startDate, 0);

        var yearsDiff = DateOnly.MaxValue.Year - startDate.Year;

        var actualCount = Math.Min(yearsDiff / interval + 1, count);

        var stopDateYear = startDate.Year + (actualCount - 1) * interval;

        return (new DateOnly(stopDateYear, 1, 1).AddDays(dayOfYear - 1), actualCount);
    }

    public static (DateOnly stopDate, int count) GetEndDateAndCount(
        DateOnly startDate,
        int dayOfYear,
        Interval interval,
        DateOnly endDate)
    {
        var yearsDiff = endDate.Year - startDate.Year;
        var actualCount = yearsDiff / interval + 1;
        var stopDateYear = startDate.Year + (actualCount - 1) * interval;
        var stopDate = new DateOnly(stopDateYear, 1, 1).AddDays(dayOfYear - 1);

        if (stopDate > endDate)
        {
            actualCount--;
            stopDateYear -= interval;
            stopDate = new DateOnly(stopDateYear, 1, 1).AddDays(dayOfYear - 1);
        }

        return (stopDate, actualCount);
    }

    public static (DateOnly stopDate, int count) GetEndDateAndCount(
        DateOnly startDate,
        MonthOfYear monthOfYear,
        DayOfMonth dayOfMonth,
        Interval interval,
        int count)
    {
        if (count < 1) return (startDate, 0);

        var yearsDiff = DateOnly.MaxValue.Year - startDate.Year;
        var actualCount = Math.Min(yearsDiff / interval + 1, count);

        var stopDateYear = startDate.Year + (actualCount - 1) * interval;
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
        var actualCount = yearsDiff / interval + 1;
        var stopDateYear = startDate.Year + (actualCount - 1) * interval;
        var stopDate = DateOnlyHelper.GetDateByDayOfMonth(stopDateYear, monthOfYear, dayOfMonth);

        if (stopDate > endDate)
        {
            actualCount--;
            stopDateYear -= interval;
            stopDate = DateOnlyHelper.GetDateByDayOfMonth(stopDateYear, monthOfYear, dayOfMonth);
        }

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
        if (count < 1) return (startDate, 0);

        var yearsDiff = DateOnly.MaxValue.Year - startDate.Year;
        var actualCount = Math.Min(yearsDiff / interval + 1, count);

        var stopDateYear = startDate.Year + (actualCount - 1) * interval;
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
        var actualCount = yearsDiff / interval + 1;
        var stopDateYear = startDate.Year + (actualCount - 1) * interval;
        var stopDate = DateOnlyHelper.GetDateByDayOfMonth(stopDateYear, monthOfYear, dayOfWeek, indexOfDay);

        if (stopDate > endDate)
        {
            actualCount--;
            stopDateYear -= interval;
            stopDate = DateOnlyHelper.GetDateByDayOfMonth(stopDateYear, monthOfYear, dayOfWeek, indexOfDay);
        }

        return (stopDate, actualCount);
    }

    private static bool DateOutOfRangeByYear(DateOnly beginDate, int addYear)
    {
        return DateOnly.MaxValue.Year - beginDate.Year < addYear;
    }
}
