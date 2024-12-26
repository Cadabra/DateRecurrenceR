using DateRecurrenceR.Collections;
using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using DateRecurrenceR.Internals;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Recurrences;

public readonly struct WeeklyByWeekDaysRecurrence : IRecurrence<WeeklyByWeekDaysRecurrence, WeeklyEnumerator>
{
    private readonly WeeklyByWeekDaysPattern _pattern;
    private readonly WeeklyHash _weeklyHash;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    public static WeeklyByWeekDaysRecurrence New(Range range, WeeklyByWeekDaysPattern pattern)
    {
        if (range.Count is not null)
        {
            return New(range.BeginDate, range.BeginDate, range.Count.Value, pattern);
        }

        if (range.EndDate is not null)
        {
            return New(range.BeginDate, range.BeginDate, range.EndDate.Value, pattern);
        }

        return new WeeklyByWeekDaysRecurrence();
    }

    private static WeeklyByWeekDaysRecurrence New(DateOnly beginDate, DateOnly fromDate, DateOnly toDate,
        WeeklyByWeekDaysPattern pattern)
    {
        var patternHash =
            WeeklyRecurrenceHelper.GetPatternHash(pattern.WeekDays, pattern.Interval, pattern.FirstDayOfWeek);

        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            patternHash,
            pattern.WeekDays,
            pattern.FirstDayOfWeek,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new WeeklyByWeekDaysRecurrence();
        }

        return new WeeklyByWeekDaysRecurrence(startDate, toDate, patternHash, pattern);
    }

    private static WeeklyByWeekDaysRecurrence New(DateOnly beginDate, DateOnly fromDate, int count,
        WeeklyByWeekDaysPattern pattern)
    {
        var patternHash =
            WeeklyRecurrenceHelper.GetPatternHash(pattern.WeekDays, pattern.Interval, pattern.FirstDayOfWeek);

        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            patternHash,
            pattern.WeekDays,
            pattern.FirstDayOfWeek,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new WeeklyByWeekDaysRecurrence();
        }

        return new WeeklyByWeekDaysRecurrence(startDate, count, patternHash, pattern);
    }

    private WeeklyByWeekDaysRecurrence(DateOnly startDate, int count, WeeklyHash weeklyHash, WeeklyByWeekDaysPattern pattern)
    {
        _startDate = startDate;
        _weeklyHash = weeklyHash;
        _pattern = pattern;

        (_stopDate, _count) = WeeklyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            pattern.WeekDays,
            pattern.FirstDayOfWeek,
            pattern.Interval,
            count);
    }

    private WeeklyByWeekDaysRecurrence(DateOnly startDate, DateOnly endDate, WeeklyHash weeklyHash, WeeklyByWeekDaysPattern pattern)
    {
        _startDate = startDate;
        _weeklyHash = weeklyHash;
        _pattern = pattern;

        (_stopDate, _count) = WeeklyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            pattern.WeekDays,
            pattern.FirstDayOfWeek,
            pattern.Interval,
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
        if (!_pattern.WeekDays[date.DayOfWeek]) return false;

        if (date < _startDate || _stopDate < date) return false;

        var fwdI = WeekDaysHelper.DayToIndex(_startDate.DayOfWeek, _pattern.FirstDayOfWeek);
        if ((date.DayNumber - _startDate.DayNumber + fwdI) % (_pattern.Interval * 7) > 7) return false;

        return true;
    }

    /// <inheritdoc />
    public WeeklyByWeekDaysRecurrence GetSubRange(int takeCount)
    {
        return new WeeklyByWeekDaysRecurrence(_startDate, takeCount, _weeklyHash, _pattern);
    }

    /// <inheritdoc />
    public WeeklyByWeekDaysRecurrence GetSubRange(DateOnly fromDate, int takeCount)
    {
        return New(_startDate, fromDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public WeeklyByWeekDaysRecurrence GetSubRange(DateOnly fromDate, DateOnly toDate)
    {
        return New(_startDate, fromDate, toDate, _pattern);
    }

    /// <inheritdoc />
    public WeeklyEnumerator GetEnumerator()
    {
        return new WeeklyEnumerator(_startDate, _count, _weeklyHash);
    }

    IEnumerator<DateOnly> IRecurrence.GetEnumerator()
    {
        return GetEnumerator();
    }
}