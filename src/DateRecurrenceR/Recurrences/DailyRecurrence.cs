using DateRecurrenceR.Core;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Recurrences;

public readonly struct DailyPattern
{
    public DailyPattern(Interval interval)
    {
        Interval = interval;
    }

    public Interval Interval { get; }
}

public readonly struct DailyRecurrence : IRecurrence
{
    private readonly DailyPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;
    
    public DailyRecurrence(Range range, DailyPattern pattern)
    {
        _pattern = pattern;

        _startDate = range.BeginDate;
        
        if (range.EndDate is not null)
        {
            var intervalCount = (range.EndDate!.Value.DayNumber - _startDate.DayNumber) / _pattern.Interval;

            _stopDate = DateOnly.FromDayNumber(_startDate.DayNumber + intervalCount * _pattern.Interval);
            _count = intervalCount + 1;
        }
        else
        {
            var expectedCount = range.Count!.Value;
            var maxCount = (DateOnly.MaxValue.DayNumber - _startDate.DayNumber) / _pattern.Interval + 1;

            _count = Math.Min(expectedCount, maxCount);
            _stopDate = DateOnly.FromDayNumber(_startDate.DayNumber + (_count - 1) * _pattern.Interval);
        }
    }
    
    public DateOnly StartDate => _startDate;
    public DateOnly StopDate => _stopDate;
    public int Count => _count;

    public bool Contains(DateOnly date)
    {
        if (date < _startDate || _stopDate < date) return false;

        return (date.DayNumber - _startDate.DayNumber) % _pattern.Interval == 0;
    }

    public IEnumerator<DateOnly> GetEnumerator()
    {
        return Recurrence.Daily(_startDate, _count, _pattern.Interval);
    }

    public IEnumerator<DateOnly> GetEnumerator(int takeCount)
    {
        return Recurrence.Daily(_startDate, takeCount, _pattern.Interval);
    }

    public IEnumerator<DateOnly> GetEnumerator(DateOnly fromDate, int takeCount)
    {
        return Recurrence.Daily(_startDate, fromDate, takeCount, _pattern.Interval);
    }

    public IEnumerator<DateOnly> GetEnumerator(DateOnly fromDate, DateOnly toDate)
    {
        return Recurrence.Daily(_startDate, _stopDate, fromDate, toDate, _pattern.Interval);
    }
}