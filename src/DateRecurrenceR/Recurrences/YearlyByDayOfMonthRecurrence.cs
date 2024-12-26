using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Recurrences;

public readonly struct YearlyByDayOfMonthRecurrence : IRecurrence<YearlyByDayOfMonthRecurrence, YearlyEnumerator>
{
    private readonly YearlyByDayOfMonthPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    public static YearlyByDayOfMonthRecurrence New(Range range, YearlyByDayOfMonthPattern pattern)
    {
        if (range.Count is not null)
        {
            return New(range.BeginDate, range.BeginDate, range.Count.Value, pattern);
        }

        if (range.EndDate is not null)
        {
            return New(range.BeginDate, range.BeginDate, range.EndDate.Value, pattern);
        }

        return new YearlyByDayOfMonthRecurrence();
    }

    private static YearlyByDayOfMonthRecurrence New(DateOnly beginDate, DateOnly fromDate, DateOnly toDate,
        YearlyByDayOfMonthPattern pattern)
    {
        var date = DateOnlyHelper.GetDateByDayOfMonth(beginDate.Year, pattern.MonthOfYear, pattern.DayOfMonth);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new YearlyByDayOfMonthRecurrence();
        }

        return new YearlyByDayOfMonthRecurrence(startDate, toDate, pattern);
    }

    private static YearlyByDayOfMonthRecurrence New(DateOnly beginDate, DateOnly fromDate, int count,
        YearlyByDayOfMonthPattern pattern)
    {
        var date = DateOnlyHelper.GetDateByDayOfMonth(beginDate.Year, pattern.MonthOfYear, pattern.DayOfMonth);
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            date.DayOfYear,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new YearlyByDayOfMonthRecurrence();
        }

        return new YearlyByDayOfMonthRecurrence(startDate, count, pattern);
    }

    private YearlyByDayOfMonthRecurrence(DateOnly startDate, int count, YearlyByDayOfMonthPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        (_stopDate, _count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            _pattern.MonthOfYear,
            _pattern.DayOfMonth,
            _pattern.Interval,
            count);
    }

    private YearlyByDayOfMonthRecurrence(DateOnly startDate, DateOnly endDate, YearlyByDayOfMonthPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        (_stopDate, _count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            _pattern.MonthOfYear,
            _pattern.DayOfMonth,
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
        if (date.Month != _pattern.MonthOfYear) return false;

        if (date < _startDate || _stopDate < date) return false;

        if (date.Day != Math.Min(DateTime.DaysInMonth(date.Year, date.Month), _pattern.DayOfMonth)) return false;

        return (date.Year - _startDate.Year) % _pattern.Interval == 0;
    }

    /// <inheritdoc />
    public YearlyByDayOfMonthRecurrence GetSubRange(int takeCount)
    {
        return new YearlyByDayOfMonthRecurrence(_startDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public YearlyByDayOfMonthRecurrence GetSubRange(DateOnly fromDate, int takeCount)
    {
        return New(_startDate, fromDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public YearlyByDayOfMonthRecurrence GetSubRange(DateOnly fromDate, DateOnly toDate)
    {
        return New(_startDate, fromDate, toDate, _pattern);
    }

    /// <inheritdoc />
    public YearlyEnumerator GetEnumerator()
    {
        var numberOfMonth = _pattern.MonthOfYear;
        var dayOfMonth = _pattern.DayOfMonth;
        return new YearlyEnumerator(
            _startDate.Year,
            _count,
            _pattern.Interval,
            GetNextDate);

        DateOnly GetNextDate(int year)
        {
            return DateOnlyHelper.GetDateByDayOfMonth(year, numberOfMonth, dayOfMonth);
        }
    }

    IEnumerator<DateOnly> IRecurrence.GetEnumerator()
    {
        return GetEnumerator();
    }
}