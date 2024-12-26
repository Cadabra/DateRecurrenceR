using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Recurrences;

public readonly struct YearlyByDayOfWeekRecurrence : IRecurrence<YearlyByDayOfWeekRecurrence, YearlyEnumerator>
{
    private readonly YearlyByDayOfWeekPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    public static YearlyByDayOfWeekRecurrence New(Range range, YearlyByDayOfWeekPattern pattern)
    {
        if (range.Count is not null)
        {
            return New(range.BeginDate, range.BeginDate, range.Count.Value, pattern);
        }

        if (range.EndDate is not null)
        {
            return New(range.BeginDate, range.BeginDate, range.EndDate.Value, pattern);
        }

        return new YearlyByDayOfWeekRecurrence();
    }

    private static YearlyByDayOfWeekRecurrence New(DateOnly beginDate, DateOnly fromDate, DateOnly toDate,
        YearlyByDayOfWeekPattern pattern)
    {
        var date = DateOnlyHelper.GetDateByDayOfMonth(
            beginDate.Year,
            pattern.MonthOfYear,
            pattern.DayOfWeek,
            pattern.IndexOfDay);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new YearlyByDayOfWeekRecurrence();
        }

        return new YearlyByDayOfWeekRecurrence(startDate, toDate, pattern);
    }

    private static YearlyByDayOfWeekRecurrence New(DateOnly beginDate, DateOnly fromDate, int count,
        YearlyByDayOfWeekPattern pattern)
    {
        var date = DateOnlyHelper.GetDateByDayOfMonth(
            beginDate.Year,
            pattern.MonthOfYear,
            pattern.DayOfWeek,
            pattern.IndexOfDay);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new YearlyByDayOfWeekRecurrence();
        }

        return new YearlyByDayOfWeekRecurrence(startDate, count, pattern);
    }

    private YearlyByDayOfWeekRecurrence(DateOnly startDate, int count, YearlyByDayOfWeekPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        (_stopDate, _count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            _pattern.MonthOfYear,
            _pattern.DayOfWeek,
            _pattern.IndexOfDay,
            _pattern.Interval,
            count);
    }

    private YearlyByDayOfWeekRecurrence(DateOnly startDate, DateOnly endDate, YearlyByDayOfWeekPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        (_stopDate, _count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            _pattern.MonthOfYear,
            _pattern.DayOfWeek,
            _pattern.IndexOfDay,
            _pattern.Interval,
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
        if (date.DayOfWeek != _pattern.DayOfWeek) return false;

        if (date.Month != _pattern.MonthOfYear) return false;

        if (date < _startDate || _stopDate < date) return false;

        if ((_pattern.Interval - 1) * 7 < date.Day || date.Day < _pattern.Interval * 7) return true;

        return (date.Year - _startDate.Year) % _pattern.Interval == 0;
    }

    /// <inheritdoc />
    public YearlyByDayOfWeekRecurrence GetSubRange(int takeCount)
    {
        return new YearlyByDayOfWeekRecurrence(_startDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public YearlyByDayOfWeekRecurrence GetSubRange(DateOnly fromDate, int takeCount)
    {
        return New(_startDate, fromDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public YearlyByDayOfWeekRecurrence GetSubRange(DateOnly fromDate, DateOnly toDate)
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
    public YearlyEnumerator GetEnumerator()
    {
        var numberOfMonth = _pattern.MonthOfYear;
        var dayOfWeek = _pattern.DayOfWeek;
        var indexOfDay = _pattern.IndexOfDay;

        return new YearlyEnumerator(
            _startDate.Year,
            _count,
            _pattern.Interval,
            GetNextDate);

        DateOnly GetNextDate(int year)
        {
            return DateOnlyHelper.GetDateByDayOfMonth(year, numberOfMonth, dayOfWeek, indexOfDay);
        }
    }

    IEnumerator<DateOnly> IRecurrence.GetEnumerator()
    {
        return GetEnumerator();
    }
}