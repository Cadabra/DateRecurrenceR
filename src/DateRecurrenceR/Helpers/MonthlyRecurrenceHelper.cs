using DateRecurrenceR.Core;

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

        startDate = DateOnlyHelper.GetDateByDayOfMonth(beginDate.Year, beginDate.Month, dayOfMonth);

        return true;
    }

    public static (DateOnly, int) GetEndDateAndCount(DateOnly startDate, int dayOfMonth, int interval, int count)
    {
        var startMonthNumber = 12 * startDate.Year + startDate.Month;
        var maxMonthNumber = 12 * DateOnly.MaxValue.Year + DateOnly.MaxValue.Month;

        var safeCount = Math.Min((maxMonthNumber - startMonthNumber) / interval, count);
        var endMonthNumber = startMonthNumber + (safeCount - 1) * interval;

        var endYear = endMonthNumber / 12;
        var endMonth = endMonthNumber % 12;

        return (DateOnlyHelper.GetDateByDayOfMonth(endYear, endMonth, dayOfMonth), safeCount);
    }

    public static (DateOnly, int) GetEndDateAndCount(DateOnly startDate, int dayOfMonth, int interval, DateOnly endDate)
    {
        var startMonthNumber = 12 * (startDate.Year - 1) + startDate.Month;
        var endMonthNumber = 12 * (endDate.Year - 1) + endDate.Month;
        var count = (endMonthNumber - startMonthNumber) / interval + 1;
        var actualEndMonthNumber = startMonthNumber + (count - 1) * interval;

        // if ((endMonthNumber - startMonthNumber) % interval == 0)
        // {
        //     var saveDay = dayOfMonth > 28
        //         ? Math.Min(dayOfMonth, DateTime.DaysInMonth(endDate.Year, endDate.Month))
        //         : dayOfMonth;
        //     if (endDate.Day > saveDay)
        //     {
        //         // actualEndMonthNumber -= interval;
        //     }
        // }

        var endYear = actualEndMonthNumber / 12 + 1;
        var endMonth = actualEndMonthNumber % 12;

        return (DateOnlyHelper.GetDateByDayOfMonth(endYear, endMonth, dayOfMonth), count);
    }

    public static (DateOnly, int) GetEndDateAndCount(
        DateOnly startDate,
        DayOfWeek dayOfWeek,
        IndexOfDay indexOfDay,
        int interval,
        int count)
    {
        var startMonthNumber = 12 * startDate.Year + startDate.Month;
        var maxMonthNumber = 12 * DateOnly.MaxValue.Year + DateOnly.MaxValue.Month;

        var safeCount = Math.Min((maxMonthNumber - startMonthNumber) / interval, count);
        var endMonthNumber = startMonthNumber + (safeCount - 1) * interval;

        var endYear = endMonthNumber / 12;
        var endMonth = endMonthNumber % 12;

        var date = DateOnlyHelper.GetDateByDayOfMonth(endYear, endMonth, dayOfWeek, indexOfDay);

        return (date, safeCount);
    }

    public static (DateOnly, int) GetEndDateAndCount(
        DateOnly startDate,
        DayOfWeek dayOfWeek,
        IndexOfDay indexOfDay,
        int interval,
        DateOnly endDate)
    {
        var startMonthNumber = 12 * (startDate.Year - 1) + startDate.Month;
        var endMonthNumber = 12 * (endDate.Year - 1) + endDate.Month;
        var count = (endMonthNumber - startMonthNumber) / interval + 1;
        var actualEndMonthNumber = startMonthNumber + (count - 1) * interval;

        var endYear = actualEndMonthNumber / 12 + 1;
        var endMonth = actualEndMonthNumber % 12;

        var date = DateOnlyHelper.GetDateByDayOfMonth(endYear, endMonth, dayOfWeek, indexOfDay);

        return (date, count);
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