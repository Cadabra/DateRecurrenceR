using System.Diagnostics;
using DateRecurrenceR.Helpers;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Recurrences;

public readonly struct MonthlyByDayOfMonthRecurrence : IRecurrence
{
    private readonly MonthlyByDayOfMonthPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    public MonthlyByDayOfMonthRecurrence(Range range, MonthlyByDayOfMonthPattern pattern)
    {
        _pattern = pattern;

        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            range.BeginDate,
            range.BeginDate,
            _pattern.DayOfMonth,
            _pattern.Interval,
            out _startDate);

        Debug.Assert(range.Count is not null || range.EndDate is not null, "The range is not valid.");

        if (canStart)
        {
            if (range.Count is not null)
            {
                (_stopDate, _count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
                    _startDate,
                    _pattern.DayOfMonth,
                    _pattern.Interval,
                    range.Count.Value);
                return;
            }

            if (range.EndDate is not null)
            {
                (_stopDate, _count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
                    _startDate,
                    _pattern.DayOfMonth,
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
        if (date < _startDate || _stopDate < date) return false;

        if (date.Day != Math.Min(DateTime.DaysInMonth(date.Year, date.Month), _pattern.DayOfMonth))
            return false;

        if (((date.Year * 12 + date.Month) - (_startDate.Year * 12 + _startDate.Month)) % _pattern.Interval >
            0) return false;

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