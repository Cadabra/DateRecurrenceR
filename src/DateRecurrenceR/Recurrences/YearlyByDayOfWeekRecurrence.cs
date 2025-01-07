using DateRecurrenceR.Helpers;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Recurrences;

public readonly struct YearlyByDayOfWeekRecurrence : IRecurrence
{
    private readonly YearlyByDayOfWeekPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    public YearlyByDayOfWeekRecurrence(Range range, YearlyByDayOfWeekPattern pattern)
    {
        _pattern = pattern;

        var date = DateHelper.GetDateByDayOfMonth(
            range.BeginDate.Year,
            _pattern.MonthOfYear,
            _pattern.DayOfWeek,
            _pattern.IndexOfDay);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            range.BeginDate,
            range.BeginDate,
            date.DayOfYear,
            _pattern.Interval,
            out _startDate);
        
        // var canStart = YearlyRecurrenceHelper.TryGetStartDate(
        //     range.BeginDate,
        //     range.BeginDate,
        //     _pattern.DayOfYear,
        //     _pattern.Interval,
        //     out _startDate);

        if (canStart)
        {
            if (range.Count is not null)
            {
                (_stopDate, _count) = YearlyRecurrenceHelper.GetEndDateAndCount(
                    _startDate,
                    _pattern.MonthOfYear,
                    _pattern.DayOfWeek,
                    _pattern.IndexOfDay,
                    _pattern.Interval,
                    range.Count.Value);
                return;
            }

            if (range.EndDate is not null)
            {
                (_stopDate, _count) = YearlyRecurrenceHelper.GetEndDateAndCount(
                    _startDate,
                    _pattern.MonthOfYear,
                    _pattern.DayOfWeek,
                    _pattern.IndexOfDay,
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
        if (date.DayOfWeek != _pattern.DayOfWeek) return false;
        
        if (date.Month != _pattern.MonthOfYear) return false;

        if (date < _startDate || _stopDate < date) return false;

        if ((_pattern.Interval - 1) * 7 < date.Day || date.Day < _pattern.Interval * 7) return true;

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