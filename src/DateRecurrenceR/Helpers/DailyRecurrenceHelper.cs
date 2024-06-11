using System;

namespace DateRecurrenceR.Helpers;

internal struct DailyRecurrenceHelper
{
    public static bool TryGetStartDate(DateOnly beginDate, DateOnly fromDate, int interval, out DateOnly startDate)
    {
        if (beginDate >= fromDate)
        {
            startDate = beginDate;
            return true;
        }

        var intervalCount = (fromDate.DayNumber - beginDate.DayNumber) / interval;
        var dateDayNumber = beginDate.DayNumber + intervalCount * interval;
        if (dateDayNumber < fromDate.DayNumber)
        {
            dateDayNumber += interval;
        }

        if (dateDayNumber > DateOnly.MaxValue.DayNumber)
        {
            startDate = default;
            return false;
        }

        startDate = DateOnly.FromDayNumber(dateDayNumber);
        return true;
    }
}