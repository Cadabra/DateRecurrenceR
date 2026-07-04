using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Helpers;

internal struct YearlyRecurrenceHelper
{
    public static bool TryGetStartDate(DateOnly beginDate,
        DateOnly fromDate,
        YearlyDateResolver resolver,
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

        startDate = resolver.GetDate(beginDate.Year + yearsDiff);

        if (startDate < fromDate)
        {
            yearsDiff += interval;

            if (DateOutOfRangeByYear(beginDate, yearsDiff))
            {
                startDate = default;
                return false;
            }

            startDate = resolver.GetDate(beginDate.Year + yearsDiff);
        }

        return true;
    }

    public static (DateOnly stopDate, int count) GetEndDateAndCount(DateOnly startDate,
        YearlyDateResolver resolver,
        int interval,
        int count)
    {
        if (count < 1) return (startDate, 0);

        var yearsDiff = DateOnly.MaxValue.Year - startDate.Year;
        var actualCount = Math.Min(yearsDiff / interval + 1, count);

        var stopDateYear = startDate.Year + (actualCount - 1) * interval;

        return (resolver.GetDate(stopDateYear), actualCount);
    }

    public static (DateOnly stopDate, int count) GetEndDateAndCount(DateOnly startDate,
        YearlyDateResolver resolver,
        int interval,
        DateOnly endDate)
    {
        var yearsDiff = endDate.Year - startDate.Year;
        var actualCount = yearsDiff / interval + 1;
        var stopDateYear = startDate.Year + (actualCount - 1) * interval;
        var stopDate = resolver.GetDate(stopDateYear);

        if (stopDate > endDate)
        {
            actualCount--;
            stopDateYear -= interval;
            stopDate = resolver.GetDate(stopDateYear);
        }

        return (stopDate, actualCount);
    }

    private static bool DateOutOfRangeByYear(DateOnly beginDate, int addYear)
    {
        return DateOnly.MaxValue.Year - beginDate.Year < addYear;
    }
}
