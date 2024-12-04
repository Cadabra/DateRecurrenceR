using DateRecurrenceR.Collections;
using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    /// <summary>
    ///     Returns an enumerator for yearly period for first <c>n</c> contiguous dates.<br/>
    ///     <b>By day of month and month of year.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="dayOfMonth">
    ///     The day of the month. Takes the last day of the month if <paramref name="dayOfMonth" /> is more than the days
    ///     in the month.
    /// </param>
    /// <param name="numberOfMonth">The number of the month.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Yearly(DateOnly beginDate,
        int count,
        DayOfMonth dayOfMonth,
        MonthOfYear numberOfMonth,
        Interval interval)
    {
        return Yearly(beginDate, beginDate, count, dayOfMonth, numberOfMonth, interval);
    }

    /// <summary>
    ///     Returns an enumerator for yearly period for first <c>n</c> contiguous dates.<br/>
    ///     <b>By day of month and month of year.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="dayOfMonth">
    ///     The day of the month. Takes the last day of the month if <paramref name="dayOfMonth" /> is more than the days
    ///     in the month.
    /// </param>
    /// <param name="numberOfMonth">The number of the month.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Yearly(DateOnly beginDate,
        DateOnly fromDate,
        int count,
        DayOfMonth dayOfMonth,
        MonthOfYear numberOfMonth,
        Interval interval)
    {
        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, numberOfMonth, dayOfMonth);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        return new YearlyEnumeratorLimitByCount(
            startDate.Year,
            count,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year)
        {
            return DateHelper.GetDateByDayOfMonth(year, numberOfMonth, dayOfMonth);
        }
    }

    /// <summary>
    ///     Returns an enumerator for yearly period for first <c>n</c> contiguous dates.<br/>
    ///     <b>By day of week, number of week and month of year.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <param name="numberOfWeek">The number of the week. The first week of a month starts from the first day of the month.</param>
    /// <param name="numberOfMonth">The number of the month.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Yearly(DateOnly beginDate,
        int count,
        DayOfWeek dayOfWeek,
        NumberOfWeek numberOfWeek,
        MonthOfYear numberOfMonth,
        Interval interval)
    {
        return Yearly(beginDate, beginDate, count, dayOfWeek, numberOfWeek, numberOfMonth, interval);
    }

    /// <summary>
    ///     Returns an enumerator for yearly period for first <c>n</c> contiguous dates.<br/>
    ///     <b>By day of week, number of week and month of year.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <param name="numberOfWeek">The number of the week. The first week of a month starts from the first day of the month.</param>
    /// <param name="numberOfMonth">The number of the month.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Yearly(DateOnly beginDate,
        DateOnly fromDate,
        int count,
        DayOfWeek dayOfWeek,
        NumberOfWeek numberOfWeek,
        MonthOfYear numberOfMonth,
        Interval interval)
    {
        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, numberOfMonth, dayOfWeek, numberOfWeek);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        return new YearlyEnumeratorLimitByCount(
            startDate.Year,
            count,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year)
        {
            return DateHelper.GetDateByDayOfMonth(year, numberOfMonth, dayOfWeek, numberOfWeek);
        }
    }

    /// <summary>
    ///     Returns an enumerator for yearly period for first <c>n</c> contiguous dates.<br/>
    ///     <b>By day of year.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="dayOfYear">The day of the year.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Yearly(DateOnly beginDate, int count, DayOfYear dayOfYear, Interval interval)
    {
        return Yearly(beginDate, beginDate, count, dayOfYear, interval);
    }

    /// <summary>
    ///     Returns an enumerator for yearly period for first <c>n</c> contiguous dates.<br/>
    ///     <b>By day of year.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="dayOfYear">The day of the year.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Yearly(DateOnly beginDate,
        DateOnly fromDate,
        int count,
        DayOfYear dayOfYear,
        Interval interval)
    {
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfYear,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        return new YearlyEnumeratorLimitByCount(
            startDate.Year,
            count,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year)
        {
            return DateHelper.GetDateByDayOfYear(year, dayOfYear);
        }
    }

    /// <summary>
    ///     Returns an enumerator for yearly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.<br/>
    ///     <b>By day of month and month of year.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="endDate">The date when the recurrence ends.</param>
    /// <param name="dayOfMonth">
    ///     The day of the month. Takes the last day of the month if <paramref name="dayOfMonth" /> is more than the days
    ///     in the month.
    /// </param>
    /// <param name="numberOfMonth">The number of the month.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Yearly(DateOnly beginDate,
        DateOnly endDate,
        DayOfMonth dayOfMonth,
        MonthOfYear numberOfMonth,
        Interval interval)
    {
        return Yearly(beginDate, endDate, beginDate, endDate, dayOfMonth, numberOfMonth, interval);
    }

    /// <summary>
    ///     Returns an enumerator for yearly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.<br/>
    ///     <b>By day of month and month of year.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="endDate">The date when the recurrence ends.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="toDate">The date when a specific range finishes.</param>
    /// <param name="dayOfMonth">
    ///     The day of the month. Takes the last day of the month if <paramref name="dayOfMonth" /> is more than the days
    ///     in the month.
    /// </param>
    /// <param name="numberOfMonth">The number of the month.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Yearly(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        DayOfMonth dayOfMonth,
        MonthOfYear numberOfMonth,
        Interval interval)
    {
        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, numberOfMonth, dayOfMonth);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        var stopDate = DateOnlyMin(toDate, endDate);

        return new YearlyEnumeratorLimitByDate(
            startDate.Year,
            stopDate,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year)
        {
            return DateHelper.GetDateByDayOfMonth(year, numberOfMonth, dayOfMonth);
        }
    }

    /// <summary>
    ///     Returns an enumerator for yearly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.<br/>
    ///     <b>By day of week, number of week and month of year.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="endDate">The date when the recurrence ends.</param>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <param name="numberOfWeek">The number of the week. The first week of a month starts from the first day of the month.</param>
    /// <param name="numberOfMonth">The number of the month.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Yearly(DateOnly beginDate,
        DateOnly endDate,
        DayOfWeek dayOfWeek,
        NumberOfWeek numberOfWeek,
        MonthOfYear numberOfMonth,
        Interval interval)
    {
        return Yearly(beginDate, endDate, beginDate, endDate, dayOfWeek, numberOfWeek, numberOfMonth, interval);
    }

    /// <summary>
    ///     Returns an enumerator for yearly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.<br/>
    ///     <b>By day of week, number of week and month of year.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="endDate">The date when the recurrence ends.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="toDate">The date when a specific range finishes.</param>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <param name="numberOfWeek">The number of the week. The first week of a month starts from the first day of the month.</param>
    /// <param name="numberOfMonth">The number of the month.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Yearly(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        DayOfWeek dayOfWeek,
        NumberOfWeek numberOfWeek,
        MonthOfYear numberOfMonth,
        Interval interval)
    {
        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, numberOfMonth, dayOfWeek, numberOfWeek);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        var stopDate = DateOnlyMin(toDate, endDate);

        return new YearlyEnumeratorLimitByDate(
            startDate.Year,
            stopDate,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year)
        {
            return DateHelper.GetDateByDayOfMonth(year, numberOfMonth, dayOfWeek, numberOfWeek);
        }
    }

    /// <summary>
    ///     Returns an enumerator for yearly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.<br/>
    ///     <b>By day of year.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="endDate">The date when the recurrence ends.</param>
    /// <param name="dayOfYear">The day of the year.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Yearly(DateOnly beginDate,
        DateOnly endDate,
        DayOfYear dayOfYear,
        Interval interval)
    {
        return Yearly(beginDate, endDate, beginDate, endDate, dayOfYear, interval);
    }

    /// <summary>
    ///     Returns an enumerator for yearly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.<br/>
    ///     <b>By day of year.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="endDate">The date when the recurrence ends.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="toDate">The date when a specific range finishes.</param>
    /// <param name="dayOfYear">The day of the year.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Yearly(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        DayOfYear dayOfYear,
        Interval interval)
    {
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfYear,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        var stopDate = DateOnlyMin(toDate, endDate);

        return new YearlyEnumeratorLimitByDate(
            startDate.Year,
            stopDate,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year)
        {
            return DateHelper.GetDateByDayOfYear(year, dayOfYear);
        }
    }
}