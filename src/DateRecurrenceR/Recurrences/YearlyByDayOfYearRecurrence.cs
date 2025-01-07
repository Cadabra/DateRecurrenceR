using DateRecurrenceR.Helpers;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Recurrences;

public readonly struct YearlyByDayOfYearRecurrence : IRecurrence
{
    private readonly YearlyByDayOfYearPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    public YearlyByDayOfYearRecurrence(Range range, YearlyByDayOfYearPattern pattern)
    {
        _pattern = pattern;

        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            range.BeginDate,
            range.BeginDate,
            _pattern.DayOfYear,
            _pattern.Interval,
            out _startDate);

        if (canStart)
        {
            if (range.Count is not null)
            {
                (_stopDate, _count) = YearlyRecurrenceHelper.GetEndDateAndCount(
                    _startDate,
                    _pattern.DayOfYear,
                    _pattern.Interval,
                    range.Count.Value);
                return;
            }

            if (range.EndDate is not null)
            {
                (_stopDate, _count) = YearlyRecurrenceHelper.GetEndDateAndCount(
                    _startDate,
                    _pattern.DayOfYear,
                    _pattern.Interval,
                    range.EndDate.Value);
                return;
            }
        }

        _stopDate = default;
        _count = default;
    }

    public DateOnly StartDate => _startDate;
    public DateOnly StopDate => _stopDate;
    public int Count => _count;

    public bool Contains(DateOnly date)
    {
        if (date.DayOfYear != _pattern.DayOfYear) return false;

        if (date < _startDate || _stopDate < date) return false;

        return (date.Year - _startDate.Year) % _pattern.Interval == 0;
    }

    public IEnumerator<DateOnly> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public IEnumerator<DateOnly> GetEnumerator(int takeCount)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<DateOnly> GetEnumerator(DateOnly fromDate, int takeCount)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<DateOnly> GetEnumerator(DateOnly fromDate, DateOnly toDate)
    {
        throw new NotImplementedException();
    }
}