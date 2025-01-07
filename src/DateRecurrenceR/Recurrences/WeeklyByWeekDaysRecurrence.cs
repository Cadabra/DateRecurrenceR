using DateRecurrenceR.Helpers;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Recurrences;

public readonly struct WeeklyByWeekDaysRecurrence : IRecurrence
{
    private readonly WeeklyByWeekDaysPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    public WeeklyByWeekDaysRecurrence(Range range, WeeklyByWeekDaysPattern pattern)
    {
        _pattern = pattern;

        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(
            _pattern.WeekDays,
            _pattern.Interval,
            _pattern.FirstDayOfWeek);

        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            range.BeginDate,
            range.BeginDate,
            patternHash,
            _pattern.WeekDays,
            _pattern.FirstDayOfWeek,
            _pattern.Interval,
            out _startDate);

        if (canStart)
        {
            if (range.Count is not null)
            {
                (_stopDate, _count) = WeeklyRecurrenceHelper.GetEndDateAndCount(
                    _startDate,
                    _pattern.WeekDays,
                    _pattern.FirstDayOfWeek,
                    _pattern.Interval,
                    range.Count.Value);
                return;
            }

            if (range.EndDate is not null)
            {
                (_stopDate, _count) = WeeklyRecurrenceHelper.GetEndDateAndCount(
                    _startDate,
                    _pattern.WeekDays,
                    _pattern.FirstDayOfWeek,
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
        if (!_pattern.WeekDays[date.DayOfWeek]) return false;

        if (date < _startDate || _stopDate < date) return false;

        if ((date.DayNumber - _startDate.DayNumber) % (_pattern.Interval * 7) > 7) return false;

        return true;
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