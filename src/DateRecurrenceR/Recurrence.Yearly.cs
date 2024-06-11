using System;
using System.Collections.Generic;
using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    public static IEnumerator<DateOnly> YearlyByDayOfMonth(
        DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        int month,
        int day,
        int interval = 1)
    {
        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, month, day);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            interval,
            out var startDate);

        if (!canStart)
        {
            return EmptyEnumerator;
        }

        return new YearlyEnumeratorLimitByCount(
            startDate.Year,
            takeCount,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year) => DateHelper.GetDateByDayOfMonth(year, month, day);
    }

    public static IEnumerator<DateOnly> YearlyByDayOfWeek(
        DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        int month,
        DayOfWeek dayOfWeek,
        NumberOfWeek numberOfWeek,
        int interval = 1)
    {
        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, month, dayOfWeek, numberOfWeek);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            interval,
            out var startDate);

        if (!canStart)
        {
            return EmptyEnumerator;
        }

        return new YearlyEnumeratorLimitByCount(
            startDate.Year,
            takeCount,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year) => DateHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);
    }

    public static IEnumerator<DateOnly> YearlyByDayOfYear(
        DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        int dayOfYear,
        int interval = 1)
    {
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfYear,
            interval,
            out var startDate);

        if (!canStart)
        {
            return EmptyEnumerator;
        }

        return new YearlyEnumeratorLimitByCount(
            startDate.Year,
            takeCount,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year) => DateHelper.GetDateByDayOfYear(year, dayOfYear);
    }

    /// <summary>
    /// Represents a recurrence in which each date falls on a specific day of the month every year.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins</param>
    /// <param name="endDate">The date of recurrence ends</param>
    /// <param name="fromDate">The date of specific range starts</param>
    /// <param name="toDate">The date of specific range ends</param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    /// <param name="interval">The interval between occurrences, 1 by default</param>
    /// <returns>The <see cref="IEnumerable{T}"/> of type <see cref="DateOnly"/> of intersection ranges <c>[beginDate, endDate]</c> and <c>[fromDate, toDate]</c></returns>
    public static IEnumerator<DateOnly> YearlyByDayOfMonth(
        DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        int month,
        int day,
        int interval = 1)
    {
        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, month, day);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            interval,
            out var startDate);

        if (!canStart)
        {
            return EmptyEnumerator;
        }

        var stopDate = DateOnlyMin(toDate, endDate);

        return new YearlyEnumeratorLimitByDate(
            startDate.Year,
            stopDate,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year) => DateHelper.GetDateByDayOfMonth(year, month, day);
    }

    /// <summary>
    /// Represents a recurrence in which each date falls on a specific week day of month every year.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins</param>
    /// <param name="endDate">The date of recurrence ends</param>
    /// <param name="fromDate">The date of specific range starts</param>
    /// <param name="toDate">The date of specific range ends</param>
    /// <param name="month"></param>
    /// <param name="dayOfWeek"></param>
    /// <param name="numberOfWeek"></param>
    /// <param name="interval">The interval between occurrences, 1 by default</param>
    /// <returns>The <see cref="IEnumerator{T}"/> of type <see cref="DateOnly"/> of intersection ranges <c>[beginDate, endDate]</c> and <c>[fromDate, toDate]</c></returns>
    public static IEnumerator<DateOnly> YearlyByDayOfWeek(
        DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        int month,
        DayOfWeek dayOfWeek,
        NumberOfWeek numberOfWeek,
        int interval = 1)
    {
        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, month, dayOfWeek, numberOfWeek);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            interval,
            out var startDate);

        if (!canStart)
        {
            return EmptyEnumerator;
        }

        var stopDate = DateOnlyMin(toDate, endDate);

        return new YearlyEnumeratorLimitByDate(
            startDate.Year,
            stopDate,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year) => DateHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);
    }

    /// <summary>
    /// Represents a recurrence in which each date falls on a specific day of year every year.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins</param>
    /// <param name="endDate">The date of recurrence ends</param>
    /// <param name="fromDate">The date of specific range starts</param>
    /// <param name="toDate">The date of specific range ends</param>
    /// <param name="dayOfYear">The day of the year when each occurrence happens</param>
    /// <param name="interval">The interval between occurrences, 1 by default</param>
    /// <returns>The <see cref="IEnumerator{T}"/> of type <see cref="DateOnly"/> of intersection ranges <c>[beginDate, endDate]</c> and <c>[fromDate, toDate]</c></returns>
    public static IEnumerator<DateOnly> YearlyByDayOfYear(
        DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        int dayOfYear,
        int interval = 1)
    {
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfYear,
            interval,
            out var startDate);

        if (!canStart)
        {
            return EmptyEnumerator;
        }

        var stopDate = DateOnlyMin(toDate, endDate);

        return new YearlyEnumeratorLimitByDate(
            startDate.Year,
            stopDate,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year) => DateHelper.GetDateByDayOfYear(year, dayOfYear);
    }
}