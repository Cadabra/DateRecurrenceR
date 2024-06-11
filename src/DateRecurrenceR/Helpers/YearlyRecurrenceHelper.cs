using System;

namespace DateRecurrenceR.Helpers;

internal struct YearlyRecurrenceHelper
{
    public static bool TryGetStartDate(
        DateOnly beginDate,
        DateOnly fromDate,
        int dayOfYear,
        int interval,
        out DateOnly startDate)
    {
        var deltaToStartYear = fromDate.Year - beginDate.Year;
        var yearsDiff = deltaToStartYear / interval * interval;

        if (deltaToStartYear % interval > 0 || fromDate.DayOfYear > dayOfYear)
        {
            yearsDiff += interval;
        }

        if (DateOutOfRangeByYear(beginDate, yearsDiff))
        {
            startDate = default;
            return false;
        }

        beginDate = beginDate.AddYears(yearsDiff);

        startDate = DateHelper.GetDateByDayOfYear(beginDate.Year, dayOfYear);

        return true;
    }
    
    private static bool DateOutOfRangeByYear(DateOnly beginDate, int addYear)
    {
        return DateOnly.MaxValue.Year - beginDate.Year < addYear;
    }
}