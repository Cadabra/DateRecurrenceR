namespace DateRecurrenceR.Recurrences;

internal static class RecurrenceInfoHelper
{
    public static (DateOnly start, int count) Merge(
        this (DateOnly startDate, DateOnly end, int count) range,
        DateOnly fromDate,
        int takeCount)
    {
        var count = range.count;
        if (takeCount < count)
        {
            count = takeCount;
        }

        var startDate = range.startDate;
        if (fromDate > range.end)
        {
            startDate = range.end;
        }
        else if (fromDate > range.startDate)
        {
            startDate = fromDate;
        }

        return (startDate, count);
    }

    public static (DateOnly start, DateOnly end) Merge(
        this (DateOnly startDate, DateOnly endDate) range,
        DateOnly fromDate,
        DateOnly toDate)
    {
        var start = fromDate < range.startDate ? range.startDate : fromDate;
        var end = toDate > range.endDate ? range.endDate : toDate;

        return (start, end);
    }
}