using DateRecurrenceR.Collections;
using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents a monthly recurrence based on a specific day of the month.
/// </summary>
public readonly struct MonthlyByDayOfMonthRecurrence : IRecurrence<MonthlyByDayOfMonthRecurrence, MonthlyEnumerator>
{
    private readonly MonthlyByDayOfMonthPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    /// <summary>
    /// Creates a new <see cref="MonthlyByDayOfMonthRecurrence"/> from the specified range and pattern.
    /// </summary>
    /// <param name="dateRange">The date range for the recurrence.</param>
    /// <param name="pattern">The monthly recurrence pattern.</param>
    /// <returns>A new <see cref="MonthlyByDayOfMonthRecurrence"/> instance.</returns>
    public static MonthlyByDayOfMonthRecurrence New(DateRange dateRange, MonthlyByDayOfMonthPattern pattern)
    {
        if (dateRange.Count is not null)
        {
            return New(dateRange.BeginDate, dateRange.BeginDate, dateRange.Count.Value, pattern);
        }

        if (dateRange.EndDate is not null)
        {
            return New(dateRange.BeginDate, dateRange.BeginDate, dateRange.EndDate.Value, pattern);
        }

        return new MonthlyByDayOfMonthRecurrence(dateRange.BeginDate, 0, pattern);
    }

    private static MonthlyByDayOfMonthRecurrence New(DateOnly beginDate, DateOnly fromDate, DateOnly toDate,
        MonthlyByDayOfMonthPattern pattern)
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            MonthlyDateResolver.ByDayOfMonth(pattern.DayOfMonth),
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new MonthlyByDayOfMonthRecurrence(beginDate, 0, pattern);
        }

        return new MonthlyByDayOfMonthRecurrence(startDate, toDate, pattern);
    }

    private static MonthlyByDayOfMonthRecurrence New(DateOnly beginDate, DateOnly fromDate, int count,
        MonthlyByDayOfMonthPattern pattern)
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            MonthlyDateResolver.ByDayOfMonth(pattern.DayOfMonth),
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new MonthlyByDayOfMonthRecurrence(beginDate, 0, pattern);
        }

        return new MonthlyByDayOfMonthRecurrence(startDate, count, pattern);
    }

    private MonthlyByDayOfMonthRecurrence(DateOnly startDate, int count, MonthlyByDayOfMonthPattern pattern)
    {
        _pattern = pattern;

        if (count < 1)
        {
            _startDate = DateOnly.MaxValue;
            _stopDate = DateOnly.MinValue;
            _count = 0;
            return;
        }

        _startDate = startDate;

        (_stopDate, _count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            MonthlyDateResolver.ByDayOfMonth(_pattern.DayOfMonth),
            _pattern.Interval,
            count);
    }

    private MonthlyByDayOfMonthRecurrence(DateOnly startDate, DateOnly endDate, MonthlyByDayOfMonthPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        (_stopDate, _count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            MonthlyDateResolver.ByDayOfMonth(_pattern.DayOfMonth),
            _pattern.Interval,
            endDate);
    }

    /// <inheritdoc />
    public DateOnly StartDate => _startDate;

    /// <inheritdoc />
    public DateOnly StopDate => _stopDate;

    /// <inheritdoc />
    public int Count => _count;

    /// <inheritdoc />
    public bool Contains(DateOnly dateToCheck)
    {
        if (dateToCheck < _startDate || _stopDate < dateToCheck) return false;

        if (dateToCheck.Day != Math.Min(DateTime.DaysInMonth(dateToCheck.Year, dateToCheck.Month), _pattern.DayOfMonth))
            return false;

        return (((dateToCheck.Year * 12 + dateToCheck.Month) - (_startDate.Year * 12 + _startDate.Month))
            % _pattern.Interval) == 0;
    }

    /// <inheritdoc />
    public MonthlyByDayOfMonthRecurrence GetSubRange(int takeCount)
    {
        if (_count == 0) return this;

        return new MonthlyByDayOfMonthRecurrence(_startDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public MonthlyByDayOfMonthRecurrence GetSubRange(DateOnly fromDate, int takeCount)
    {
        if (_count == 0) return this;

        return New(_startDate, fromDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public MonthlyByDayOfMonthRecurrence GetSubRange(DateOnly fromDate, DateOnly toDate)
    {
        if (_count == 0) return this;

        return New(_startDate, fromDate, toDate, _pattern);
    }

    IRecurrence IRecurrence.GetSubRange(int takeCount)
    {
        return GetSubRange(takeCount);
    }

    IRecurrence IRecurrence.GetSubRange(DateOnly fromDate, int takeCount)
    {
        return GetSubRange(fromDate, takeCount);
    }

    IRecurrence IRecurrence.GetSubRange(DateOnly fromDate, DateOnly toDate)
    {
        return GetSubRange(fromDate, toDate);
    }

    /// <inheritdoc />
    public MonthlyEnumerator GetEnumerator()
    {
        return new MonthlyEnumerator(_startDate, _count, _pattern.Interval,
            MonthlyDateResolver.ByDayOfMonth(_pattern.DayOfMonth));
    }

    IEnumerator<DateOnly> IRecurrence.GetEnumerator()
    {
        return GetEnumerator();
    }
}
