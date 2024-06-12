using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    /// <summary>
    ///     Gets an enumerator for daily period for first n contiguous dates.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="takeCount"></param>
    /// <param name="interval">The interval between occurrences, 1 by default.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval"/> less than 1.</exception>
    public static IEnumerator<DateOnly> GetDailyEnumerator(DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        int interval = 1)
    {
        if (interval < 1) throw new ArgumentException("");

        var canStart = DailyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        return new DailyEnumeratorLimitByCount(startDate, takeCount, interval);
    }

    /// <summary>
    /// Gets an enumerator for daily period in intersection ranges <c>[beginDate, endDate]</c> and <c>[fromDate, toDate]</c>
    /// </summary>
    /// <param name="beginDate"></param>
    /// <param name="endDate"></param>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <param name="interval"></param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> of type <see cref="DateOnly" />
    ///     The <see cref="IEnumerator{T}" /> of type <see cref="DateOnly" /> of 
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval"/> less than 1.</exception>
    public static IEnumerator<DateOnly> GetDailyEnumerator(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        int interval = 1)
    {
        if (interval < 1) throw new ArgumentException("");

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