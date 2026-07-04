using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Helpers;

internal struct MonthlyRecurrenceHelper
{
    public static bool TryGetStartDate(DateOnly beginDate,
        DateOnly fromDate,
        MonthlyDateResolver resolver,
        int interval,
        out DateOnly startDate)
    {
        if (fromDate < beginDate) fromDate = beginDate;

        var deltaToStartMonth = SubtractMonth(fromDate, beginDate);

        var monthsDiff = deltaToStartMonth / interval * interval;
        if (deltaToStartMonth % interval > 0) monthsDiff += interval;

        if (DateOutOfRangeByMonth(beginDate, monthsDiff))
        {
            startDate = default;
            return false;
        }

        var monthDate = beginDate.AddMonths(monthsDiff);
        startDate = resolver.GetDate(monthDate.Year, monthDate.Month);

        if (startDate < fromDate)
        {
            monthsDiff += interval;

            if (DateOutOfRangeByMonth(beginDate, monthsDiff))
            {
                startDate = default;
                return false;
            }

            monthDate = beginDate.AddMonths(monthsDiff);
            startDate = resolver.GetDate(monthDate.Year, monthDate.Month);
        }

        return true;
    }

    public static (DateOnly, int) GetEndDateAndCount(DateOnly startDate,
        MonthlyDateResolver resolver,
        int interval,
        int count)
    {
        if (count < 1) return (startDate, 0);

        var startMonthNumber = MonthsInYear * (startDate.Year - 1) + startDate.Month;
        var maxMonthNumber = MonthsInYear * (DateOnly.MaxValue.Year - 1) + DateOnly.MaxValue.Month;

        var safeCount = Math.Min((maxMonthNumber - startMonthNumber) / interval + 1, count);
        var endMonthNumber = startMonthNumber + (safeCount - 1) * interval;

        var endYear = (endMonthNumber - 1) / MonthsInYear + 1;
        var endMonth = (endMonthNumber - 1) % MonthsInYear + 1;

        return (resolver.GetDate(endYear, endMonth), safeCount);
    }

    public static (DateOnly, int) GetEndDateAndCount(DateOnly startDate,
        MonthlyDateResolver resolver,
        int interval,
        DateOnly endDate)
    {
        var startMonthNumber = MonthsInYear * (startDate.Year - 1) + startDate.Month;
        var endMonthNumber = MonthsInYear * (endDate.Year - 1) + endDate.Month;
        var count = (endMonthNumber - startMonthNumber) / interval + 1;
        var actualEndMonthNumber = startMonthNumber + (count - 1) * interval;

        var endYear = (actualEndMonthNumber - 1) / MonthsInYear + 1;
        var endMonth = (actualEndMonthNumber - 1) % MonthsInYear + 1;

        var actualDate = resolver.GetDate(endYear, endMonth);

        if (actualDate > endDate)
        {
            count--;
            actualEndMonthNumber -= interval;
            endYear = (actualEndMonthNumber - 1) / MonthsInYear + 1;
            endMonth = (actualEndMonthNumber - 1) % MonthsInYear + 1;
            actualDate = resolver.GetDate(endYear, endMonth);
        }

        return (actualDate, count);
    }

    private static bool DateOutOfRangeByMonth(DateOnly beginDate, int addMonth)
    {
        return SubtractMonth(DateOnly.MaxValue, beginDate) < addMonth;
    }

    private static int SubtractMonth(DateOnly minuendDate, DateOnly subtractedDate)
    {
        var minuend = minuendDate.Year * MonthsInYear + minuendDate.Month;
        var subtracted = subtractedDate.Year * MonthsInYear + subtractedDate.Month;

        return minuend - subtracted;
    }
}
