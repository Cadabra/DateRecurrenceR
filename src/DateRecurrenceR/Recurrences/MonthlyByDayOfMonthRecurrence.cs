using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;
using Range = DateRecurrenceR.Core.Range;

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
    /// <param name="range">The date range for the recurrence.</param>
    /// <param name="pattern">The monthly recurrence pattern.</param>
    /// <returns>A new <see cref="MonthlyByDayOfMonthRecurrence"/> instance.</returns>
    public static MonthlyByDayOfMonthRecurrence New(Range range, MonthlyByDayOfMonthPattern pattern)
    {
        if (range.Count is not null)
        {
            return New(range.BeginDate, range.BeginDate, range.Count.Value, pattern);
        }

        if (range.EndDate is not null)
        {
            return New(range.BeginDate, range.BeginDate, range.EndDate.Value, pattern);
        }

        return new MonthlyByDayOfMonthRecurrence();
    }

    private static MonthlyByDayOfMonthRecurrence New(DateOnly beginDate, DateOnly fromDate, DateOnly toDate,
        MonthlyByDayOfMonthPattern pattern)
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            pattern.DayOfMonth,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new MonthlyByDayOfMonthRecurrence();
        }

        return new MonthlyByDayOfMonthRecurrence(startDate, toDate, pattern);
    }

    private static MonthlyByDayOfMonthRecurrence New(DateOnly beginDate, DateOnly fromDate, int count,
        MonthlyByDayOfMonthPattern pattern)
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            pattern.DayOfMonth,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new MonthlyByDayOfMonthRecurrence();
        }

        return new MonthlyByDayOfMonthRecurrence(startDate, count, pattern);
    }

    private MonthlyByDayOfMonthRecurrence(DateOnly startDate, int count, MonthlyByDayOfMonthPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        (_stopDate, _count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            _pattern.DayOfMonth,
            _pattern.Interval,
            count);
    }

    private MonthlyByDayOfMonthRecurrence(DateOnly startDate, DateOnly endDate, MonthlyByDayOfMonthPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        (_stopDate, _count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            _pattern.DayOfMonth,
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
    public bool Contains(DateOnly date)
    {
        if (date < _startDate || _stopDate < date) return false;

        if (date.Day != Math.Min(DateTime.DaysInMonth(date.Year, date.Month), _pattern.DayOfMonth))
            return false;

        if (((date.Year * 12 + date.Month) - (_startDate.Year * 12 + _startDate.Month)) % _pattern.Interval <=
            0) return true;
        return false;
    }

    /// <inheritdoc />
    public MonthlyByDayOfMonthRecurrence GetSubRange(int takeCount)
    {
        return new MonthlyByDayOfMonthRecurrence(_startDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public MonthlyByDayOfMonthRecurrence GetSubRange(DateOnly fromDate, int takeCount)
    {
        return New(_startDate, fromDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public MonthlyByDayOfMonthRecurrence GetSubRange(DateOnly fromDate, DateOnly toDate)
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
        var dom = _pattern.DayOfMonth;
        return new MonthlyEnumerator(_startDate, _count, _pattern.Interval, GetNextDate);

        DateOnly GetNextDate(int year, int month)
        {
            return DateOnlyHelper.GetDateByDayOfMonth(year, month, dom);
        }
    }

    IEnumerator<DateOnly> IRecurrence.GetEnumerator()
    {
        return GetEnumerator();
    }
}
