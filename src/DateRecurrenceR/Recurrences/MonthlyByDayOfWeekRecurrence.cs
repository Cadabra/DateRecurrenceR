using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Recurrences;

public readonly struct MonthlyByDayOfWeekRecurrence : IRecurrence<MonthlyByDayOfWeekRecurrence, MonthlyEnumerator>
{
    private readonly MonthlyByDayOfWeekPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    public static MonthlyByDayOfWeekRecurrence New(Range range, MonthlyByDayOfWeekPattern pattern)
    {
        if (range.Count is not null)
        {
            return New(range.BeginDate, range.BeginDate, range.Count.Value, pattern);
        }

        if (range.EndDate is not null)
        {
            return New(range.BeginDate, range.BeginDate, range.EndDate.Value, pattern);
        }

        return new MonthlyByDayOfWeekRecurrence();
    }

    private static MonthlyByDayOfWeekRecurrence New(DateOnly beginDate, DateOnly fromDate, DateOnly toDate,
        MonthlyByDayOfWeekPattern pattern)
    {
        var date = DateOnlyHelper.GetDateByDayOfMonth(
            beginDate.Year,
            beginDate.Month,
            pattern.DayOfWeek,
            pattern.IndexOfDay);
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.Day,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new MonthlyByDayOfWeekRecurrence();
        }

        return new MonthlyByDayOfWeekRecurrence(startDate, toDate, pattern);
    }

    private static MonthlyByDayOfWeekRecurrence New(DateOnly beginDate, DateOnly fromDate, int count,
        MonthlyByDayOfWeekPattern pattern)
    {
        var date = DateOnlyHelper.GetDateByDayOfMonth(
            beginDate.Year,
            beginDate.Month,
            pattern.DayOfWeek,
            pattern.IndexOfDay);
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.Day,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new MonthlyByDayOfWeekRecurrence();
        }

        return new MonthlyByDayOfWeekRecurrence(startDate, count, pattern);
    }

    private MonthlyByDayOfWeekRecurrence(DateOnly startDate, int count, MonthlyByDayOfWeekPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        (_stopDate, _count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            _pattern.DayOfWeek,
            _pattern.IndexOfDay,
            _pattern.Interval,
            count);
    }

    private MonthlyByDayOfWeekRecurrence(DateOnly startDate, DateOnly endDate, MonthlyByDayOfWeekPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        (_stopDate, _count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            _pattern.DayOfWeek,
            _pattern.IndexOfDay,
            _pattern.Interval,
            endDate);
    }

    private MonthlyByDayOfWeekRecurrence(MonthlyByDayOfWeekPattern pattern, DateOnly startDate, DateOnly stopDate,
        int count)
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
        if (date.DayOfWeek != _pattern.DayOfWeek) return false;

        if (date < _startDate || _stopDate < date) return false;

        if (((date.Year * 12 + date.Month) - (_startDate.Year * 12 + _startDate.Month)) % _pattern.Interval >
            0) return false;

        if ((_pattern.Interval - 1) * 7 < date.Day || date.Day < _pattern.Interval * 7) return true;

        return false;
    }

    /// <inheritdoc />
    public MonthlyByDayOfWeekRecurrence GetSubRange(int takeCount)
    {
        return new MonthlyByDayOfWeekRecurrence(_startDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public MonthlyByDayOfWeekRecurrence GetSubRange(DateOnly fromDate, int takeCount)
    {
        return New(_startDate, fromDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public MonthlyByDayOfWeekRecurrence GetSubRange(DateOnly fromDate, DateOnly toDate)
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
        var dow = _pattern.DayOfWeek;
        var iod = _pattern.IndexOfDay;
        return new MonthlyEnumerator(_startDate, _count, _pattern.Interval, GetNextDate);

        DateOnly GetNextDate(int year, int month)
        {
            return DateOnlyHelper.GetDateByDayOfMonth(year, month, dow, iod);
        }
    }

    IEnumerator<DateOnly> IRecurrence.GetEnumerator()
    {
        return GetEnumerator();
    }
}