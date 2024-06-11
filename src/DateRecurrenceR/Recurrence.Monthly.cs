using System;
using System.Collections.Generic;
using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    public static IEnumerator<DateOnly> MonthlyByDayOfMonth(
        DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        int dayOfMonth,
        int interval = 1)
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfMonth,
            interval,
            out var startDate);
        
        if (!canStart)
        {
            return EmptyEnumerator;
        }

        return new MonthlyEnumeratorLimitByCount(startDate, takeCount, interval, GetNextDate);

        DateOnly GetNextDate(int year, int month) => DateHelper.GetDateByDayOfMonth(year, month, dayOfMonth);
    }

    public static IEnumerator<DateOnly> MonthlyByDayOfWeek(
        DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        DayOfWeek dayOfWeek,
        NumberOfWeek numberOfWeek,
        int interval = 1)
    {
        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, beginDate.Month, dayOfWeek, numberOfWeek);
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.Day,
            interval,
            out var startDate);
        
        if (!canStart)
        {
            return EmptyEnumerator;
        }

        return new MonthlyEnumeratorLimitByCount(startDate, takeCount, interval, GetNextDate);

        DateOnly GetNextDate(int year, int month) =>
            DateHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);
    }

    /// <summary>
    /// Represents a recurrence in which each date falls on a specific day of month every month.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins</param>
    /// <param name="endDate">The date of recurrence ends</param>
    /// <param name="fromDate">The date of specific range starts</param>
    /// <param name="toDate">The date of specific range ends</param>
    /// <param name="dayOfMonth"></param>
    /// <param name="interval">The interval between occurrences, 1 by default</param>
    /// <returns>The <see cref="IEnumerator{T}"/> of type <see cref="DateOnly"/> of intersection ranges <c>[beginDate, endDate]</c> and <c>[fromDate, toDate]</c></returns>
    public static IEnumerator<DateOnly> MonthlyByDayOfMonth(
        DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        int dayOfMonth,
        int interval = 1)
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfMonth,
            interval,
            out var startDate);

        if (!canStart)
        {
            return EmptyEnumerator;
        }

        var stopDate = DateOnlyMin(toDate, endDate);

        return new MonthlyEnumeratorLimitByDate(startDate, stopDate, interval, GetNextDate);

        DateOnly GetNextDate(int year, int month) => DateHelper.GetDateByDayOfMonth(year, month, dayOfMonth);
    }

    /// <summary>
    /// Represents a recurrence in which each date falls on a specific day of week every month.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins</param>
    /// <param name="endDate">The date of recurrence ends</param>
    /// <param name="fromDate">The date of specific range starts</param>
    /// <param name="toDate">The date of specific range ends</param>
    /// <param name="dayOfWeek"></param>
    /// <param name="numberOfWeek"></param>
    /// <param name="interval">The interval between occurrences, 1 by default</param>
    /// <returns>The <see cref="IEnumerator{T}"/> of type <see cref="DateOnly"/> of intersection ranges <c>[beginDate, endDate]</c> and <c>[fromDate, toDate]</c></returns>
    public static IEnumerator<DateOnly> MonthlyByDayOfWeek(
        DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        DayOfWeek dayOfWeek,
        NumberOfWeek numberOfWeek,
        int interval = 1)
    {
        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, beginDate.Month, dayOfWeek, numberOfWeek);
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.Day,
            interval,
            out var startDate);
        
        if (!canStart)
        {
            return EmptyEnumerator;
        }

        var stopDate = DateOnlyMin(toDate, endDate);

        return new MonthlyEnumeratorLimitByDate(
            startDate,
            stopDate,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year, int month) =>
            DateHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);
    }
}