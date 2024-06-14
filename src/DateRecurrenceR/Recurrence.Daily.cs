using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    /// <summary>
    ///     Gets an enumerator for daily period for first <c>n</c> contiguous dates.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="takeCount">The maximum number of contiguous dates.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> Daily(DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        int interval)
    {
        if (interval < 1) throw new ArgumentException($"The '{nameof(interval)}' cannot be less than 1.");

        if (takeCount < 1) return EmptyEnumerator;

        var canStart = DailyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        return new DailyEnumeratorLimitByCount(startDate, takeCount, interval);
    }

    /// <summary>
    ///     Gets an enumerator for daily period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="endDate">The date of recurrence ends.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="toDate">The date of specific range finishes.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> Daily(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        int interval)
    {
        if (interval < 1) throw new ArgumentException($"The '{nameof(interval)}' cannot be less than 1.");

        var canStart = DailyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        var stopDate = DateOnlyMin(toDate, endDate);

        return new DailyEnumeratorLimitByDate(startDate, stopDate, interval);
    }
}