using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Recurrences;

public readonly struct MonthlyPattern
{
    public MonthlyPattern(Interval interval, DayOfMonth dayOfMonth)
    {
        Type = MonthlyRecurrenceType.ByDayOfMonth;

        Interval = interval;
        DayOfMonth = dayOfMonth;

        DayOfWeek = default;
        IndexOfDay = default;
    }

    public MonthlyPattern(Interval interval, DayOfWeek dayOfWeek, IndexOfDay indexOfDay)
    {
        Type = MonthlyRecurrenceType.ByDayOfWeek;

        Interval = interval;
        DayOfWeek = dayOfWeek;
        IndexOfDay = indexOfDay;

        DayOfMonth = default;
    }

    internal MonthlyRecurrenceType Type { get; }

    public Interval Interval { get; }
    public DayOfMonth DayOfMonth { get; }
    public DayOfWeek DayOfWeek { get; }
    public IndexOfDay IndexOfDay { get; }
}

public readonly struct MonthlyRecurrence : IRecurrence
{
    private readonly MonthlyPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    public MonthlyRecurrence(Range range, MonthlyPattern pattern)
    {
        _pattern = pattern;

        bool canStart;
        if (_pattern.Type == MonthlyRecurrenceType.ByDayOfMonth)
        {
            canStart = MonthlyRecurrenceHelper.TryGetStartDate(
                range.BeginDate,
                range.BeginDate,
                _pattern.DayOfMonth,
                _pattern.Interval,
                out _startDate);
        }
        else
        {
            var date = DateHelper.GetDateByDayOfMonth(
                range.BeginDate.Year,
                range.BeginDate.Month,
                pattern.DayOfWeek,
                pattern.IndexOfDay);
            canStart = MonthlyRecurrenceHelper.TryGetStartDate(
                range.BeginDate,
                range.BeginDate,
                date.Day,
                pattern.Interval,
                out _startDate);
        }
    }

    public DateOnly StartDate => _startDate;
    public DateOnly StopDate => _stopDate;
    public int Count => _count;

    public bool Contains(DateOnly date)
    {
        if (_pattern.Type == MonthlyRecurrenceType.ByDayOfMonth)
        {
            if (date < _startDate || _stopDate < date) return false;

            if (date.Day != Math.Min(DateTime.DaysInMonth(date.Year, date.Month), _pattern.DayOfMonth))
                return false;

            if (((date.Year * 12 + date.Month) - (_startDate.Year * 12 + _startDate.Month)) % _pattern.Interval >
                0) return false;

            return true;
        }

        if (date.DayOfWeek != _pattern.DayOfWeek) return false;

        if (date < _startDate || _stopDate < date) return false;

        if (((date.Year * 12 + date.Month) - (_startDate.Year * 12 + _startDate.Month)) % _pattern.Interval >
            0) return false;

        if ((_pattern.Interval - 1) * 7 < date.Day || date.Day < _pattern.Interval * 7) return true;

        return false;
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

internal enum MonthlyRecurrenceType : byte
{
    ByDayOfMonth,
    ByDayOfWeek
}