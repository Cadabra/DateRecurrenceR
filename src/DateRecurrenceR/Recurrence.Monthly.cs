using DateRecurrenceR.Collections;
using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    /// <summary>
    ///     Returns an enumerator for monthly period for first <c>n</c> contiguous dates.<br/>
    ///     <b>By day of month.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="dayOfMonth">
    ///     The day of the month. Takes the last day of the month if <paramref name="dayOfMonth" /> is more than the days
    ///     in the month.
    /// </param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Monthly(DateOnly beginDate, int count, DayOfMonth dayOfMonth, Interval interval)
    {
        return Monthly(beginDate, beginDate, count, dayOfMonth, interval);
    }

    /// <summary>
    ///     Returns an enumerator for monthly period for first <c>n</c> contiguous dates.<br/>
    ///     <b>By day of month.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="dayOfMonth">
    ///     The day of the month. Takes the last day of the month if <paramref name="dayOfMonth" /> is more than the days
    ///     in the month.
    /// </param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Monthly(DateOnly beginDate,
        DateOnly fromDate,
        int count,
        DayOfMonth dayOfMonth,
        Interval interval)
    {
        if (count < 1) return EmptyEnumerator;

        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfMonth,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        return new MonthlyEnumeratorLimitByCount(startDate, count, interval, GetNextDate);

        DateOnly GetNextDate(int year, int month)
        {
            return DateOnlyHelper.GetDateByDayOfMonth(year, month, dayOfMonth);
        }
    }

    /// <summary>
    ///     Returns an enumerator for monthly period for first <c>n</c> contiguous dates.<br/>
    ///     <b>By day of week and number of week.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <param name="indexOfDay">Index of <b>dayOfWeek</b> in the month.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Monthly(DateOnly beginDate, int count, DayOfWeek dayOfWeek,
        IndexOfDay indexOfDay, Interval interval)
    {
        return Monthly(beginDate, beginDate, count, dayOfWeek, indexOfDay, interval);
    }

    /// <summary>
    ///     Returns an enumerator for monthly period for first <c>n</c> contiguous dates.<br/>
    ///     <b>By day of week and number of week.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <param name="indexOfDay">Index of <b>dayOfWeek</b> in the month.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Monthly(DateOnly beginDate,
        DateOnly fromDate,
        int count,
        DayOfWeek dayOfWeek,
        IndexOfDay indexOfDay,
        Interval interval)
    {
        var date = DateOnlyHelper.GetDateByDayOfMonth(beginDate.Year, beginDate.Month, dayOfWeek, indexOfDay);
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.Day,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        return new MonthlyEnumeratorLimitByCount(startDate, count, interval, GetNextDate);

        DateOnly GetNextDate(int year, int month)
        {
            return DateOnlyHelper.GetDateByDayOfMonth(year, month, dayOfWeek, indexOfDay);
        }
    }

    /// <summary>
    ///     Returns an enumerator for monthly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.<br/>
    ///     <b>By day of month.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="endDate">The date when the recurrence ends.</param>
    /// <param name="dayOfMonth">
    ///     The day of the month. Takes the last day of the month if <paramref name="dayOfMonth" /> is more than the days
    ///     in the month.
    /// </param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Monthly(DateOnly beginDate,
        DateOnly endDate,
        DayOfMonth dayOfMonth,
        Interval interval)
    {
        return Monthly(beginDate, endDate, beginDate, endDate, dayOfMonth, interval);
    }

    /// <summary>
    ///     Returns an enumerator for monthly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.<br/>
    ///     <b>By day of month.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="endDate">The date when the recurrence ends.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="toDate">The date when a specific range finishes.</param>
    /// <param name="dayOfMonth">
    ///     The day of the month. Takes the last day of the month if <paramref name="dayOfMonth" /> is more than the days
    ///     in the month.
    /// </param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Monthly(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        DayOfMonth dayOfMonth,
        Interval interval)
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfMonth,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        var stopDate = DateOnlyMin(toDate, endDate);

        return new MonthlyEnumeratorLimitByDate(startDate, stopDate, interval, GetNextDate);

        DateOnly GetNextDate(int year, int month)
        {
            return DateOnlyHelper.GetDateByDayOfMonth(year, month, dayOfMonth);
        }
    }

    /// <summary>
    ///     Returns an enumerator for monthly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.<br/>
    ///     <b>By day of week and number of week.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="endDate">The date when the recurrence ends.</param>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <param name="indexOfDay">Index of <b>dayOfWeek</b> in the month.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Monthly(DateOnly beginDate,
        DateOnly endDate,
        DayOfWeek dayOfWeek,
        IndexOfDay indexOfDay,
        Interval interval)
    {
        return Monthly(beginDate, endDate, beginDate, endDate, dayOfWeek, indexOfDay, interval);
    }

    /// <summary>
    ///     Returns an enumerator for monthly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.<br/>
    ///     <b>By day of week and number of week.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="endDate">The date when the recurrence ends.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="toDate">The date when a specific range finishes.</param>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <param name="indexOfDay">Index of <b>dayOfWeek</b> in the month.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Monthly(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        DayOfWeek dayOfWeek,
        IndexOfDay indexOfDay,
        Interval interval)
    {
        var date = DateOnlyHelper.GetDateByDayOfMonth(beginDate.Year, beginDate.Month, dayOfWeek, indexOfDay);
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.Day,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        var stopDate = DateOnlyMin(toDate, endDate);

        return new MonthlyEnumeratorLimitByDate(
            startDate,
            stopDate,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year, int month)
        {
            return DateOnlyHelper.GetDateByDayOfMonth(year, month, dayOfWeek, indexOfDay);
        }
    }
}