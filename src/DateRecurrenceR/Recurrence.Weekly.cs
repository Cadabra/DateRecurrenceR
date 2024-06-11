using System;
using System.Collections.Generic;
using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    /// <summary>
    /// Represents a recurrence in which each date falls on a specific day or days of week every week.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins</param>
    /// <param name="fromDate">The date of specific range starts</param>
    /// <param name="takeCount"></param>
    /// <param name="weekDays"></param>
    /// <param name="firstDayOfWeek"></param>
    /// <param name="interval">The interval between occurrences, 1 by default</param>
    /// <returns>The <see cref="IEnumerator{T}"/> of type <see cref="DateOnly"/> of intersection ranges <c>[beginDate, endDate]</c> and <c>[fromDate, toDate]</c></returns>
    public static IEnumerator<DateOnly> GetWeeklyEnumerator(
        DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        WeekDays weekDays,
        DayOfWeek firstDayOfWeek,
        int interval = 1)
    {
        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            weekDays,
            firstDayOfWeek,
            interval,
            out var startDate);
        
        if (!canStart)
        {
            return EmptyEnumerator;
        }

        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval);

        return new WeeklyEnumeratorLimitByCount(startDate, takeCount, patternHash);
    }

    /// <summary>
    /// Represents a recurrence in which each date falls on a specific day or days of week every week.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins</param>
    /// <param name="endDate">The date of recurrence ends</param>
    /// <param name="fromDate">The date of specific range starts</param>
    /// <param name="toDate">The date of specific range ends</param>
    /// <param name="weekDays"></param>
    /// <param name="firstDayOfWeek"></param>
    /// <param name="interval">The interval between occurrences, 1 by default</param>
    /// <returns>The <see cref="IEnumerator{T}"/> of type <see cref="DateOnly"/> of intersection ranges <c>[beginDate, endDate]</c> and <c>[fromDate, toDate]</c></returns>
    public static IEnumerator<DateOnly> GetWeeklyEnumerator(
        DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        WeekDays weekDays,
        DayOfWeek firstDayOfWeek,
        int interval = 1)
    {
        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            weekDays,
            firstDayOfWeek,
            interval,
            out var startDate);
        
        if (!canStart)
        {
            return EmptyEnumerator;
        }

        var stopDate = DateOnlyMin(toDate, endDate);

        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval);

        return new WeeklyEnumeratorLimitByDate(startDate, stopDate, patternHash);
    }
}