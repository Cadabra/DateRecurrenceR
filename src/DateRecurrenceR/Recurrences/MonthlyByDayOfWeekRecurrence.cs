using DateRecurrenceR.Collections;
using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents a monthly recurrence based on a specific day of the week.
/// </summary>
public readonly struct MonthlyByDayOfWeekRecurrence : IRecurrence<MonthlyByDayOfWeekRecurrence, MonthlyEnumerator>
{
    private readonly MonthlyByDayOfWeekPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    /// <summary>
    /// Creates a new <see cref="MonthlyByDayOfWeekRecurrence"/> from the specified range and pattern.
    /// </summary>
    /// <param name="dateRange">The date range for the recurrence.</param>
    /// <param name="pattern">The monthly recurrence pattern.</param>
    /// <returns>A new <see cref="MonthlyByDayOfWeekRecurrence"/> instance.</returns>
    public static MonthlyByDayOfWeekRecurrence New(DateRange dateRange, MonthlyByDayOfWeekPattern pattern)
    {
        if (dateRange.Count is not null)
        {
            return New(dateRange.BeginDate, dateRange.BeginDate, dateRange.Count.Value, pattern);
        }

        if (dateRange.EndDate is not null)
        {
            return New(dateRange.BeginDate, dateRange.BeginDate, dateRange.EndDate.Value, pattern);
        }

        return new MonthlyByDayOfWeekRecurrence();
    }

    private static MonthlyByDayOfWeekRecurrence New(DateOnly beginDate, DateOnly fromDate, DateOnly toDate,
        MonthlyByDayOfWeekPattern pattern)
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            pattern.DayOfWeek,
            pattern.IndexOfDay,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new MonthlyByDayOfWeekRecurrence();
        }

        return new MonthlyByDayOfWeekRecurrence(startDate, toDate, pattern);
    }

    private static MonthlyByDayOfWeekRecurrence New(DateOnly beginDate, DateOnly fromDate, int count,
        MonthlyByDayOfWeekPattern pattern)
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            pattern.DayOfWeek,
            pattern.IndexOfDay,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new MonthlyByDayOfWeekRecurrence();
        }

        return new MonthlyByDayOfWeekRecurrence(startDate, count, pattern);
    }

    private MonthlyByDayOfWeekRecurrence(DateOnly startDate, int count, MonthlyByDayOfWeekPattern pattern)
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
            _pattern.DayOfWeek,
            _pattern.IndexOfDay,
            _pattern.Interval,
            count);
    }

    private MonthlyByDayOfWeekRecurrence(DateOnly startDate, DateOnly endDate, MonthlyByDayOfWeekPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        (_stopDate, _count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            _pattern.DayOfWeek,
            _pattern.IndexOfDay,
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
        if (dateToCheck.DayOfWeek != _pattern.DayOfWeek) return false;

        if (dateToCheck < _startDate || _stopDate < dateToCheck) return false;

        if (((dateToCheck.Year * 12 + dateToCheck.Month) - (_startDate.Year * 12 + _startDate.Month)) % _pattern.Interval >
            0) return false;

        if (_pattern.IndexOfDay == IndexOfDay.Last)
        {
            return dateToCheck.Day + DaysInWeek > DateTime.DaysInMonth(dateToCheck.Year, dateToCheck.Month);
        }

        return (int)_pattern.IndexOfDay == (dateToCheck.Day - 1) / DaysInWeek;
    }

    /// <inheritdoc />
    public MonthlyByDayOfWeekRecurrence GetSubRange(int takeCount)
    {
        return new MonthlyByDayOfWeekRecurrence(_startDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public MonthlyByDayOfWeekRecurrence GetSubRange(DateOnly fromDate, int takeCount)
    {
        return New(_startDate, fromDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public MonthlyByDayOfWeekRecurrence GetSubRange(DateOnly fromDate, DateOnly toDate)
    {
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
            MonthlyDateResolver.ByDayOfWeek(_pattern.DayOfWeek, _pattern.IndexOfDay));
    }

    IEnumerator<DateOnly> IRecurrence.GetEnumerator()
    {
        return GetEnumerator();
    }
}
