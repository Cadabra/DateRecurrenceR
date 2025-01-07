using System.Diagnostics;
using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Recurrences;

public readonly struct MonthlyByDayOfWeekRecurrence : IRecurrence
{
    private readonly MonthlyByDayOfWeekPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    public MonthlyByDayOfWeekRecurrence(Range range, MonthlyByDayOfWeekPattern pattern)
    {
        _pattern = pattern;

        var date = DateHelper.GetDateByDayOfMonth(
            range.BeginDate.Year,
            range.BeginDate.Month,
            _pattern.DayOfWeek,
            _pattern.IndexOfDay);
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            range.BeginDate,
            range.BeginDate,
            date.Day,
            _pattern.Interval,
            out _startDate);

        Debug.Assert(range.Count is not null || range.EndDate is not null, "The range is not valid.");

        if (canStart)
        {
            if (range.Count is not null)
            {
                (_stopDate, _count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
                    _startDate,
                    _pattern.DayOfWeek,
                    _pattern.IndexOfDay,
                    _pattern.Interval,
                    range.Count.Value);
                return;
            }

            if (range.EndDate is not null)
            {
                (_stopDate, _count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
                    _startDate,
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

        if (date < _startDate || _stopDate < date) return false;

        if (((date.Year * 12 + date.Month) - (_startDate.Year * 12 + _startDate.Month)) % _pattern.Interval >
            0) return false;

        if ((_pattern.Interval - 1) * 7 < date.Day || date.Day < _pattern.Interval * 7) return true;

        return false;
    }

    public IEnumerator<DateOnly> GetEnumerator()
    {
        return Recurrence.Monthly(_startDate, _count, _pattern.DayOfWeek, _pattern.IndexOfDay, _pattern.Interval);
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