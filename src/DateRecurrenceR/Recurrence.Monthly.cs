using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    /// <summary>
    ///     Gets an enumerator for monthly period for first n contiguous dates.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="takeCount">The maximum number of contiguous dates.</param>
    /// <param name="dayOfMonth">The day of month. Takes the last day of month if <paramref name="dayOfMonth" /> more than days in the month.</param>
    /// <param name="interval">The interval between occurrences, 1 by default.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> MonthlyByDayOfMonth(DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        int dayOfMonth,
        int interval = 1)
    {
        if (interval < 1) throw new ArgumentException($"The '{nameof(interval)}' cannot be less than 1.");

        if (takeCount < 1) return EmptyEnumerator;

        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfMonth,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        return new MonthlyEnumeratorLimitByCount(startDate, takeCount, interval, GetNextDate);

        DateOnly GetNextDate(int year, int month)
        {
            return DateHelper.GetDateByDayOfMonth(year, month, dayOfMonth);
        }
    }

    /// <summary>
    ///     Gets an enumerator for monthly period for first n contiguous dates.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="takeCount">The maximum number of contiguous dates.</param>
    /// <param name="dayOfWeek">The day of week.</param>
    /// <param name="numberOfWeek">The number of week. First Week of a Month starting from the first day of the month.</param>
    /// <param name="interval">The interval between occurrences, 1 by default.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> MonthlyByDayOfWeek(DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        DayOfWeek dayOfWeek,
        NumberOfWeek numberOfWeek,
        int interval = 1)
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

        return new MonthlyEnumeratorLimitByCount(startDate, takeCount, interval, GetNextDate);

        DateOnly GetNextDate(int year, int month)
        {
            return DateHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);
        }
    }

    /// <summary>
    ///     Gets an enumerator for monthly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="endDate">The date of recurrence ends.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="toDate">The date of specific range finishes.</param>
    /// <param name="dayOfMonth">The day of month. Takes the last day of month if <paramref name="dayOfMonth" /> more than days in the month.</param>
    /// <param name="interval">The interval between occurrences, 1 by default</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> MonthlyByDayOfMonth(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        int dayOfMonth,
        int interval = 1)
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
    ///     <c>[fromDate, toDate]</c>
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="endDate">The date of recurrence ends.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="toDate">The date of specific range finishes.</param>
    /// <param name="dayOfWeek">The day of week.</param>
    /// <param name="numberOfWeek">The number of week. First Week of a Month starting from the first day of the month.</param>
    /// <param name="interval">The interval between occurrences, 1 by default</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> MonthlyByDayOfWeek(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        DayOfWeek dayOfWeek,
        NumberOfWeek numberOfWeek,
        int interval = 1)
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