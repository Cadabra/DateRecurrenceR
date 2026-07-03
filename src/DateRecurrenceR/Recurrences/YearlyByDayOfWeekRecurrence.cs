using DateRecurrenceR.Collections;
using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents a yearly recurrence based on a specific day of the week within a month.
/// </summary>
public readonly struct YearlyByDayOfWeekRecurrence : IRecurrence<YearlyByDayOfWeekRecurrence, YearlyEnumerator>
{
    private readonly YearlyByDayOfWeekPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    /// <summary>
    /// Creates a new <see cref="YearlyByDayOfWeekRecurrence"/> from the specified range and pattern.
    /// </summary>
    /// <param name="dateRange">The date range for the recurrence.</param>
    /// <param name="pattern">The yearly recurrence pattern.</param>
    /// <returns>A new <see cref="YearlyByDayOfWeekRecurrence"/> instance.</returns>
    public static YearlyByDayOfWeekRecurrence New(DateRange dateRange, YearlyByDayOfWeekPattern pattern)
    {
        if (dateRange.Count is not null)
        {
            return New(dateRange.BeginDate, dateRange.BeginDate, dateRange.Count.Value, pattern);
        }

        if (dateRange.EndDate is not null)
        {
            return New(dateRange.BeginDate, dateRange.BeginDate, dateRange.EndDate.Value, pattern);
        }

        return new YearlyByDayOfWeekRecurrence();
    }

    private static YearlyByDayOfWeekRecurrence New(DateOnly beginDate, DateOnly fromDate, DateOnly toDate,
        YearlyByDayOfWeekPattern pattern)
    {
        var date = DateOnlyHelper.GetDateByDayOfMonth(
            beginDate.Year,
            pattern.MonthOfYear,
            pattern.DayOfWeek,
            pattern.IndexOfDay);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new YearlyByDayOfWeekRecurrence();
        }

        return new YearlyByDayOfWeekRecurrence(startDate, toDate, pattern);
    }

    private static YearlyByDayOfWeekRecurrence New(DateOnly beginDate, DateOnly fromDate, int count,
        YearlyByDayOfWeekPattern pattern)
    {
        var date = DateOnlyHelper.GetDateByDayOfMonth(
            beginDate.Year,
            pattern.MonthOfYear,
            pattern.DayOfWeek,
            pattern.IndexOfDay);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new YearlyByDayOfWeekRecurrence();
        }

        return new YearlyByDayOfWeekRecurrence(startDate, count, pattern);
    }

    private YearlyByDayOfWeekRecurrence(DateOnly startDate, int count, YearlyByDayOfWeekPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        (_stopDate, _count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            _pattern.MonthOfYear,
            _pattern.DayOfWeek,
            _pattern.IndexOfDay,
            _pattern.Interval,
            count);
    }

    private YearlyByDayOfWeekRecurrence(DateOnly startDate, DateOnly endDate, YearlyByDayOfWeekPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        (_stopDate, _count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            _pattern.MonthOfYear,
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

        if (dateToCheck.Month != _pattern.MonthOfYear) return false;

        if (dateToCheck < _startDate || _stopDate < dateToCheck) return false;

        if ((dateToCheck.Year - _startDate.Year) % _pattern.Interval != 0) return false;

        if (_pattern.IndexOfDay == IndexOfDay.Last)
        {
            return dateToCheck.Day + DaysInWeek > DateTime.DaysInMonth(dateToCheck.Year, dateToCheck.Month);
        }

        return (int)_pattern.IndexOfDay == (dateToCheck.Day - 1) / DaysInWeek;
    }

    /// <inheritdoc />
    public YearlyByDayOfWeekRecurrence GetSubRange(int takeCount)
    {
        return new YearlyByDayOfWeekRecurrence(_startDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public YearlyByDayOfWeekRecurrence GetSubRange(DateOnly fromDate, int takeCount)
    {
        return New(_startDate, fromDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public YearlyByDayOfWeekRecurrence GetSubRange(DateOnly fromDate, DateOnly toDate)
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
    public YearlyEnumerator GetEnumerator()
    {
        return new YearlyEnumerator(
            _startDate.Year,
            _count,
            _pattern.Interval,
            YearlyDateResolver.ByDayOfWeek(_pattern.MonthOfYear, _pattern.DayOfWeek, _pattern.IndexOfDay));
    }

    IEnumerator<DateOnly> IRecurrence.GetEnumerator()
    {
        return GetEnumerator();
    }
}