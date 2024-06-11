using System;
using System.Collections.Generic;
using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    public static IEnumerator<DateOnly> GetDailyEnumerator(
        DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        int interval = 1)
    {
        var canStart = DailyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            interval,
            out var startDate);
        
        if (!canStart)
        {
            return EmptyEnumerator;
        }

        return new DailyEnumeratorLimitByCount(startDate, takeCount, interval);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="beginDate"></param>
    /// <param name="endDate"></param>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <param name="interval"></param>
    /// <returns></returns>
    public static IEnumerator<DateOnly> GetDailyEnumerator(
        DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        int interval = 1)
    {
        var canStart = DailyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            interval,
            out var startDate);
        
        if (!canStart)
        {
            return EmptyEnumerator;
        }

        var stopDate = DateOnlyMin(toDate, endDate);

        return new DailyEnumeratorLimitByDate(startDate, stopDate, interval);
    }
}