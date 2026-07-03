using DateRecurrenceR.Collections;
using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

/// <inheritdoc />
public readonly struct DailyRecurrence : IRecurrence<DailyRecurrence, DailyEnumerator>
{
    private readonly DailyPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    /// <summary>
    /// Creates a new <see cref="DailyRecurrence"/> from the specified range and pattern.
    /// </summary>
    /// <param name="dateRange">The date range for the recurrence.</param>
    /// <param name="pattern">The daily recurrence pattern.</param>
    /// <returns>A new <see cref="DailyRecurrence"/> instance.</returns>
    public static DailyRecurrence New(DateRange dateRange, DailyPattern pattern)
    {
        if (dateRange.Count is not null)
        {
            return new DailyRecurrence(dateRange.BeginDate, dateRange.Count.Value, pattern);
        }

        if (dateRange.EndDate is not null)
        {
            return new DailyRecurrence(dateRange.BeginDate, dateRange.EndDate.Value, pattern);
        }

        return new DailyRecurrence();
    }

    private DailyRecurrence(DateOnly startDate, DateOnly endDate, DailyPattern pattern)
    {
        _pattern = pattern;

        if (endDate < startDate)
        {
            _startDate = DateOnly.MaxValue;
            _stopDate = DateOnly.MinValue;
            _count = 0;
            return;
        }

        _startDate = startDate;

        var intervalCount = (endDate.DayNumber - startDate.DayNumber) / pattern.Interval;

        _stopDate = DateOnly.FromDayNumber(startDate.DayNumber + intervalCount * pattern.Interval);
        _count = intervalCount + 1;
    }

    private DailyRecurrence(DateOnly startDate, int count, DailyPattern pattern)
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

        var maxCount = (DateOnly.MaxValue.DayNumber - _startDate.DayNumber) / pattern.Interval + 1;

        _count = Math.Min(count, maxCount);
        _stopDate = DateOnly.FromDayNumber(_startDate.DayNumber + (_count - 1) * pattern.Interval);
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

        return (dateToCheck.DayNumber - _startDate.DayNumber) % _pattern.Interval == 0;
    }

    /// <inheritdoc />
    public DailyRecurrence GetSubRange(int takeCount)
    {
        return new DailyRecurrence(_startDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public DailyRecurrence GetSubRange(DateOnly fromDate, int takeCount)
    {
        if (!TryAlignToGrid(fromDate, out var startDate))
        {
            return new DailyRecurrence(DateOnly.MaxValue, 0, _pattern);
        }

        return new DailyRecurrence(startDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public DailyRecurrence GetSubRange(DateOnly fromDate, DateOnly toDate)
    {
        if (!TryAlignToGrid(fromDate, out var startDate))
        {
            return new DailyRecurrence(DateOnly.MaxValue, 0, _pattern);
        }

        return new DailyRecurrence(startDate, toDate, _pattern);
    }

    private bool TryAlignToGrid(DateOnly fromDate, out DateOnly alignedDate)
    {
        if (fromDate <= _startDate)
        {
            alignedDate = _startDate;
            return true;
        }

        var offset = fromDate.DayNumber - _startDate.DayNumber;
        var remainder = offset % _pattern.Interval;
        if (remainder != 0) offset += _pattern.Interval - remainder;

        if (offset > DateOnly.MaxValue.DayNumber - _startDate.DayNumber)
        {
            alignedDate = default;
            return false;
        }

        alignedDate = DateOnly.FromDayNumber(_startDate.DayNumber + offset);
        return true;
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
    public DailyEnumerator GetEnumerator()
    {
        return new DailyEnumerator(_startDate, _count, _pattern.Interval);
    }

    IEnumerator<DateOnly> IRecurrence.GetEnumerator()
    {
        return GetEnumerator();
    }
}
