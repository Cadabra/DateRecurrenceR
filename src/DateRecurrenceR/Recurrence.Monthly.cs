using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    /// <summary>
    ///     Gets an enumerator for monthly period for first <c>n</c> contiguous dates.<br/>
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
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> Monthly(DateOnly beginDate,
        DateOnly fromDate,
        int count,
        int dayOfMonth,
        int interval)
    {
        if (interval < 1) throw new ArgumentException($"The '{nameof(interval)}' cannot be less than 1.");

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
            return DateHelper.GetDateByDayOfMonth(year, month, dayOfMonth);
        }
    }

    /// <summary>
    ///     Gets an enumerator for monthly period for first <c>n</c> contiguous dates.<br/>
    ///     <b>By day of week and number of week.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <param name="numberOfWeek">The number of the week. The first week of a month starts from the first day of the month.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> Monthly(DateOnly beginDate,
        DateOnly fromDate,
        int count,
        DayOfWeek dayOfWeek,
        NumberOfWeek numberOfWeek,
        int interval)
    {
        if (interval < 1) throw new ArgumentException($"The '{nameof(interval)}' cannot be less than 1.");

        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, beginDate.Month, dayOfWeek, numberOfWeek);
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
            return DateHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);
        }
    }

    /// <summary>
    ///     Gets an enumerator for monthly period in intersection ranges <c>[beginDate, endDate]</c> and
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
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> Monthly(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        int dayOfMonth,
        int interval)
    {
        if (interval < 1) throw new ArgumentException($"The '{nameof(interval)}' cannot be less than 1.");

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
            return DateHelper.GetDateByDayOfMonth(year, month, dayOfMonth);
        }
    }

    /// <summary>
    ///     Gets an enumerator for monthly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.<br/>
    ///     <b>By day of week and number of week.</b>
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="endDate">The date when the recurrence ends.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="toDate">The date when a specific range finishes.</param>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <param name="numberOfWeek">The number of the week. The first week of a month starts from the first day of the month.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> Monthly(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        DayOfWeek dayOfWeek,
        NumberOfWeek numberOfWeek,
        int interval)
    {
        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, beginDate.Month, dayOfWeek, numberOfWeek);
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
            return DateHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);
        }
    }
}