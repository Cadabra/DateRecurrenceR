using DateRecurrenceR.Collections;
using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    /// <summary>
    ///     Returns an enumerator for daily period for first <c>n</c> contiguous dates.
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static DailyEnumerator Daily(DateOnly beginDate, int count, Interval interval)
    {
        return Daily(beginDate, beginDate, count, interval);
    }

    /// <summary>
    ///     Returns an enumerator for daily period for first <c>n</c> contiguous dates.
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static DailyEnumerator Daily(DateOnly beginDate,
        DateOnly fromDate,
        int count,
        Interval interval)
    {
        if (count < 1) return DailyEnumerator.Empty;

        var canStart = DailyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            interval,
            out var startDate);

        if (!canStart) return DailyEnumerator.Empty;

        return new DailyEnumerator(startDate, count, interval);
        // return new DailyEnumeratorLimitByCount(startDate, count, interval);
    }

    /// <summary>
    ///     Returns an enumerator for daily period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="endDate">The date when the recurrence ends.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static DailyEnumerator Daily(DateOnly beginDate, DateOnly endDate, Interval interval)
    {
        return Daily(beginDate, endDate, beginDate, endDate, interval);
    }

    /// <summary>
    ///     Returns an enumerator for daily period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="endDate">The date when the recurrence ends.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="toDate">The date when a specific range finishes.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static DailyEnumerator Daily(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        Interval interval)
    {
        var canStart = DailyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            interval,
            out var startDate);

        if (!canStart) return DailyEnumerator.Empty;

        var stopDate = DateOnlyMin(toDate, endDate);
        var safeCount = DailyRecurrenceHelper.GetCount(startDate, stopDate, interval);

        return new DailyEnumerator(startDate, safeCount, interval);
    }
}
