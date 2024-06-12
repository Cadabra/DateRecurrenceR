using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    /// <summary>
    ///     Gets an enumerator for weekly period for first n contiguous dates.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="takeCount">The maximum number of contiguous dates.</param>
    /// <param name="weekDays">Days of week.</param>
    /// <param name="firstDayOfWeek">The first day of week.</param>
    /// <param name="interval">The interval between occurrences, 1 by default.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> GetWeeklyEnumerator(DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        WeekDays weekDays,
        DayOfWeek firstDayOfWeek,
        int interval = 1)
    {
        if (interval < 1) throw new ArgumentException($"The '{nameof(interval)}' cannot be less than 1.");

        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            weekDays,
            firstDayOfWeek,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval);

        return new WeeklyEnumeratorLimitByCount(startDate, takeCount, patternHash);
    }

    /// <summary>
    ///     Gets an enumerator for monthly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="endDate">The date of recurrence ends.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="toDate">The date of specific range finishes.</param>
    /// <param name="weekDays">Days of week.</param>
    /// <param name="firstDayOfWeek">The first day of week.</param>
    /// <param name="interval">The interval between occurrences, 1 by default.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> GetWeeklyEnumerator(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        WeekDays weekDays,
        DayOfWeek firstDayOfWeek,
        int interval = 1)
    {
        if (interval < 1) throw new ArgumentException($"The '{nameof(interval)}' cannot be less than 1.");

        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            weekDays,
            firstDayOfWeek,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        var stopDate = DateOnlyMin(toDate, endDate);

        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval);

        return new WeeklyEnumeratorLimitByDate(startDate, stopDate, patternHash);
    }
}