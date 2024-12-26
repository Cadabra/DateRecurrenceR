using DateRecurrenceR.Collections;
using DateRecurrenceR.Helpers;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Recurrences;

public readonly struct YearlyByDayOfYearRecurrence : IRecurrence<YearlyByDayOfYearRecurrence, YearlyEnumerator>
{
    private readonly YearlyByDayOfYearPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    public static YearlyByDayOfYearRecurrence New(Range range, YearlyByDayOfYearPattern pattern)
    {
        if (range.Count is not null)
        {
            return New(range.BeginDate, range.BeginDate, range.Count.Value, pattern);
        }

        if (range.EndDate is not null)
        {
            return New(range.BeginDate, range.BeginDate, range.EndDate.Value, pattern);
        }

        return new YearlyByDayOfYearRecurrence();
    }

    private static YearlyByDayOfYearRecurrence New(DateOnly beginDate, DateOnly fromDate, DateOnly toDate,
        YearlyByDayOfYearPattern pattern)
    {
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            pattern.DayOfYear,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new YearlyByDayOfYearRecurrence();
        }

        return new YearlyByDayOfYearRecurrence(startDate, toDate, pattern);
    }

    private static YearlyByDayOfYearRecurrence New(DateOnly beginDate, DateOnly fromDate, int count,
        YearlyByDayOfYearPattern pattern)
    {
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            pattern.DayOfYear,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new YearlyByDayOfYearRecurrence();
        }

        return new YearlyByDayOfYearRecurrence(startDate, count, pattern);
    }

    private YearlyByDayOfYearRecurrence(DateOnly startDate, int count, YearlyByDayOfYearPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        (_stopDate, _count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            _pattern.DayOfYear,
            _pattern.Interval,
            count);
    }

    private YearlyByDayOfYearRecurrence(DateOnly startDate, DateOnly endDate, YearlyByDayOfYearPattern pattern)
    {
        _startDate = startDate;
        _pattern = pattern;

        (_stopDate, _count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            _startDate,
            _pattern.DayOfYear,
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
        if (date.DayOfYear != _pattern.DayOfYear) return false;

        if (date < _startDate || _stopDate < date) return false;

        return (date.Year - _startDate.Year) % _pattern.Interval == 0;
    }

    /// <inheritdoc />
    public YearlyByDayOfYearRecurrence GetSubRange(int takeCount)
    {
        return new YearlyByDayOfYearRecurrence(_startDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public YearlyByDayOfYearRecurrence GetSubRange(DateOnly fromDate, int takeCount)
    {
        return New(_startDate, fromDate, takeCount, _pattern);
    }

    /// <inheritdoc />
    public YearlyByDayOfYearRecurrence GetSubRange(DateOnly fromDate, DateOnly toDate)
    {
        return New(_startDate, fromDate, toDate, _pattern);
    }

    /// <inheritdoc />
    public YearlyEnumerator GetEnumerator()
    {
        var dayOfYear = _pattern.DayOfYear;

        return new YearlyEnumerator(
            _startDate.Year,
            _count,
            _pattern.Interval,
            GetNextDate);

        DateOnly GetNextDate(int year)
        {
            return DateOnlyHelper.GetDateByDayOfYear(year, dayOfYear);
        }
    }

    IEnumerator<DateOnly> IRecurrence.GetEnumerator()
    {
        return GetEnumerator();
    }
}