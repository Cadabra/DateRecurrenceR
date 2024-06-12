using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    /// <summary>
    ///     Gets an enumerator for yearly period for first n contiguous dates.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="takeCount">The maximum number of contiguous dates.</param>
    /// <param name="monthOfYear">The number of month.</param>
    /// <param name="dayOfMonth">
    ///     The day of month. Takes the last day of month if <paramref name="dayOfMonth" /> more than days
    ///     in the month.
    /// </param>
    /// <param name="interval">The interval between occurrences, 1 by default.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> YearlyByDayOfMonth(DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        int monthOfYear,
        int dayOfMonth,
        int interval = 1)
    {
        if (interval < 1) throw new ArgumentException($"The '{nameof(interval)}' cannot be less than 1.");

        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, monthOfYear, dayOfMonth);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        return new YearlyEnumeratorLimitByCount(
            startDate.Year,
            takeCount,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year)
        {
            return DateHelper.GetDateByDayOfMonth(year, monthOfYear, dayOfMonth);
        }
    }

    /// <summary>
    ///     Gets an enumerator for yearly period for first n contiguous dates.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="takeCount">The maximum number of contiguous dates.</param>
    /// <param name="monthOfYear">The number of month.</param>
    /// <param name="dayOfWeek">The day of week.</param>
    /// <param name="numberOfWeek">The number of week. First Week of a Month starting from the first day of the month.</param>
    /// <param name="interval">The interval between occurrences, 1 by default.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> YearlyByDayOfWeek(DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        int monthOfYear,
        DayOfWeek dayOfWeek,
        NumberOfWeek numberOfWeek,
        int interval = 1)
    {
        if (interval < 1) throw new ArgumentException($"The '{nameof(interval)}' cannot be less than 1.");

        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, monthOfYear, dayOfWeek, numberOfWeek);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        return new YearlyEnumeratorLimitByCount(
            startDate.Year,
            takeCount,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year)
        {
            return DateHelper.GetDateByDayOfMonth(year, monthOfYear, dayOfWeek, numberOfWeek);
        }
    }

    /// <summary>
    ///     Gets an enumerator for yearly period for first n contiguous dates.
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="takeCount">The maximum number of contiguous dates.</param>
    /// <param name="dayOfYear">The day of year.</param>
    /// <param name="interval">The interval between occurrences, 1 by default.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> YearlyByDayOfYear(DateOnly beginDate,
        DateOnly fromDate,
        int takeCount,
        int dayOfYear,
        int interval = 1)
    {
        if (interval < 1) throw new ArgumentException($"The '{nameof(interval)}' cannot be less than 1.");

        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfYear,
            interval,
            out var startDate);

        if (!canStart) return EmptyEnumerator;

        return new YearlyEnumeratorLimitByCount(
            startDate.Year,
            takeCount,
            interval,
            GetNextDate);

        DateOnly GetNextDate(int year)
        {
            return DateHelper.GetDateByDayOfYear(year, dayOfYear);
        }
    }

    /// <summary>
    ///     Gets an enumerator for yearly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="endDate">The date of recurrence ends.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="toDate">The date of specific range finishes.</param>
    /// <param name="monthOfYear">The number of month.</param>
    /// <param name="dayOfMonth">
    ///     The day of month. Takes the last day of month if <paramref name="dayOfMonth" /> more than days
    ///     in the month.
    /// </param>
    /// <param name="interval">The interval between occurrences, 1 by default.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> YearlyByDayOfMonth(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        int monthOfYear,
        int dayOfMonth,
        int interval = 1)
    {
        if (interval < 1) throw new ArgumentException($"The '{nameof(interval)}' cannot be less than 1.");

        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, monthOfYear, dayOfMonth);
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
            return DateHelper.GetDateByDayOfMonth(year, monthOfYear, dayOfMonth);
        }
    }

    /// <summary>
    ///     Gets an enumerator for yearly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="endDate">The date of recurrence ends.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="toDate">The date of specific range finishes.</param>
    /// <param name="monthOfYear">The number of month.</param>
    /// <param name="dayOfWeek">The day of week.</param>
    /// <param name="numberOfWeek">The number of week. First Week of a Month starting from the first day of the month.</param>
    /// <param name="interval">The interval between occurrences, 1 by default.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> YearlyByDayOfWeek(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        int monthOfYear,
        DayOfWeek dayOfWeek,
        NumberOfWeek numberOfWeek,
        int interval = 1)
    {
        if (interval < 1) throw new ArgumentException($"The '{nameof(interval)}' cannot be less than 1.");

        var date = DateHelper.GetDateByDayOfMonth(beginDate.Year, monthOfYear, dayOfWeek, numberOfWeek);
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
            return DateHelper.GetDateByDayOfMonth(year, monthOfYear, dayOfWeek, numberOfWeek);
        }
    }

    /// <summary>
    ///     Gets an enumerator for yearly period in intersection ranges <c>[beginDate, endDate]</c> and
    ///     <c>[fromDate, toDate]</c>
    /// </summary>
    /// <param name="beginDate">The date of recurrence begins.</param>
    /// <param name="endDate">The date of recurrence ends.</param>
    /// <param name="fromDate">The date of specific range starts.</param>
    /// <param name="toDate">The date of specific range finishes.</param>
    /// <param name="dayOfYear">The day of year.</param>
    /// <param name="interval">The interval between occurrences, 1 by default.</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="interval" /> less than 1.</exception>
    public static IEnumerator<DateOnly> YearlyByDayOfYear(DateOnly beginDate,
        DateOnly endDate,
        DateOnly fromDate,
        DateOnly toDate,
        int dayOfYear,
        int interval = 1)
    {
        if (interval < 1) throw new ArgumentException($"The '{nameof(interval)}' cannot be less than 1.");

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