using DateRecurrenceR.Collections.Val;
using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct RecurrenceVal
{
    /// <summary>
    ///     Gets an enumerator for daily period for first <c>n</c> contiguous dates.
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static DateEnumerator Daily(DateOnly beginDate,
        DateOnly fromDate,
        int count,
        Interval interval)
    {
        if (count < 1) return default;

        var canStart = DailyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            interval,
            out var startDate);

        if (!canStart) return default;

        return new DateEnumerator(startDate, count, interval);
    }

    /// <summary>
    ///     Gets an enumerator for daily period in intersection ranges <c>[beginDate, endDate]</c> and
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
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static DateEnumerator Daily(DateOnly beginDate,
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

        if (!canStart) return default;

        var stopDate = DateOnlyMin(toDate, endDate);

        return new DateEnumerator(startDate, stopDate, interval);
    }
}