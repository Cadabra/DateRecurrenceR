using DateRecurrenceR.Collections;
using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    /// <summary>
    ///     Gets an enumerator for weekly period for first <c>n</c> contiguous dates.
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="count">The maximum number of contiguous dates.</param>
    /// <param name="weekDays">Days of the week.</param>
    /// <param name="firstDayOfWeek">The first day of the week.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> Weekly(DateOnly beginDate,
        DateOnly fromDate,
        int count,
        WeekDays weekDays,
        DayOfWeek firstDayOfWeek,
        Interval interval)
    {
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval);

        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            patternHash,
            weekDays,
            firstDayOfWeek,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        return new WeeklyEnumeratorLimitByCount(startDate, count, patternHash);
    }

    /// <summary>
    ///     Gets an enumerator for weekly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>.
    /// </summary>
    /// <param name="beginDate">The date when the recurrence begins.</param>
    /// <param name="endDate">The date when the recurrence ends.</param>
    /// <param name="fromDate">The date when a specific range starts.</param>
    /// <param name="toDate">The date when a specific range finishes.</param>
    /// <param name="weekDays">Days of the week.</param>
    /// <param name="firstDayOfWeek">The first day of the week.</param>
    /// <param name="interval">The interval between occurrences.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> Weekly(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        WeekDays weekDays,
        DayOfWeek firstDayOfWeek,
        Interval interval)
    {
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval);

        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            patternHash,
            weekDays,
            firstDayOfWeek,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        var stopDate = DateOnlyMin(toDate, endDate);

        return new WeeklyEnumeratorLimitByDate(startDate, stopDate, patternHash);
    }
}