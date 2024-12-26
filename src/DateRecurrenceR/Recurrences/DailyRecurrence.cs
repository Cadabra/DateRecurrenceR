using DateRecurrenceR.Collections;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Recurrences;

/// <inheritdoc />
public readonly struct DailyRecurrence : IRecurrence<DailyRecurrence, DailyEnumerator>
{
    private readonly DailyPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    public static DailyRecurrence New(Range range, DailyPattern pattern)
    {
        if (range.Count is not null)
        {
            return new DailyRecurrence(range.BeginDate, range.Count.Value, pattern);
        }

        if (range.EndDate is not null)
        {
            return new DailyRecurrence(range.BeginDate, range.EndDate.Value, pattern);
        }

        return new DailyRecurrence();
    }

    private DailyRecurrence(DateOnly startDate, DateOnly endDate, DailyPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        var intervalCount = (endDate.DayNumber - startDate.DayNumber) / pattern.Interval;

        _stopDate = DateOnly.FromDayNumber(startDate.DayNumber + intervalCount * pattern.Interval);
        _count = intervalCount + 1;
    }

    private DailyRecurrence(DateOnly startDate, int count, DailyPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        var maxCount = (DateOnly.MaxValue.DayNumber - _startDate.DayNumber) / pattern.Interval + 1;

        _count = Math.Min(count, maxCount);
        _stopDate = DateOnly.FromDayNumber(_startDate.DayNumber + (count - 1) * pattern.Interval);
    }

    private DailyRecurrence(DailyPattern pattern, DateOnly startDate, DateOnly stopDate, int count)
    {
        _pattern = pattern;
        _startDate = startDate;
        _stopDate = stopDate;
        _count = count;
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

        return (date.DayNumber - _startDate.DayNumber) % _pattern.Interval == 0;
    }

    /// <inheritdoc />
    /// <inheritdoc />
    public DailyRecurrence GetSubRange(int takeCount)
    {
        return new DailyRecurrence(_startDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public DailyRecurrence GetSubRange(DateOnly fromDate, int takeCount)
    {
        return new DailyRecurrence(fromDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public DailyRecurrence GetSubRange(DateOnly fromDate, DateOnly toDate)
    {
        return new DailyRecurrence(_startDate, toDate, _pattern);
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