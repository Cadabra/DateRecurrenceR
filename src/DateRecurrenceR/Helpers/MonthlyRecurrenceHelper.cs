namespace DateRecurrenceR.Helpers;

internal struct MonthlyRecurrenceHelper
{
    public static bool TryGetStartDate(DateOnly beginDate,
        DateOnly fromDate,
        int dayOfMonth,
        int interval,
        out DateOnly startDate)
    {
        var deltaToStartMonth = SubtractMonth(fromDate, beginDate);

        var monthsDiff = Math.Max(deltaToStartMonth / interval * interval, 0);
        if (deltaToStartMonth % interval > 0 || fromDate.Day > dayOfMonth) monthsDiff += interval;

        if (DateOutOfRangeByMonth(beginDate, monthsDiff))
        {
            startDate = default;
            return false;
        }

        beginDate = beginDate.AddMonths(monthsDiff);

        startDate = DateHelper.GetDateByDayOfMonth(beginDate.Year, beginDate.Month, dayOfMonth);

        return true;
    }

    private static bool DateOutOfRangeByMonth(DateOnly beginDate, int addMonth)
    {
        return SubtractMonth(DateOnly.MaxValue, beginDate) < addMonth;
    }

    private static int SubtractMonth(DateOnly minuendDate, DateOnly subtractedDate)
    {
        var minuend = minuendDate.Year * 12 + minuendDate.Month;
        var subtracted = subtractedDate.Year * 12 + subtractedDate.Month;

        return minuend - subtracted;
    }
}