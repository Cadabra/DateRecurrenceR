using System.Collections;

namespace ConsoleHelper;

public sealed class MonthEnumerable : IEnumerator<WeekEnumerator>
{
    private readonly DayOfWeek _firstDayOfWeek;
    private readonly DateOnly _startDate;
    private readonly DateOnly _endDate;
    private readonly int _startMonth;
    private readonly int _endMonth;
    private int _current = 0;

    public MonthEnumerable(in int currentYear, in DateOnly startDate, in DateOnly endDate, DayOfWeek firstDayOfWeek)
    {
        Year = currentYear;
        _startDate = startDate;
        _endDate = endDate;
        _startMonth = currentYear > startDate.Year ? 1 : startDate.Month;
        _endMonth = currentYear < endDate.Year ? 12 : endDate.Month;
        _firstDayOfWeek = firstDayOfWeek;
    }

    public int Year { get; }

    public bool MoveNext()
    {
        if (_current == 0)
        {
            _current = _startMonth;
            Current = new WeekEnumerator(Year, in _current, in _startDate, in _endDate, _firstDayOfWeek);
            return true;
        }

        if (_current >= _endMonth)
            return false;
        ++_current;
        Current = new WeekEnumerator(Year, in _current, in _startDate, in _endDate, _firstDayOfWeek);
        return true;
    }

    public void Reset() => throw new NotImplementedException();

    public WeekEnumerator Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }

    public WeekEnumerator GetWeekEnumerator()
    {
        return new WeekEnumerator(Year, in _current, in _startDate, in _endDate, _firstDayOfWeek);
    }
}

public sealed class IntMonthEnumerator : IEnumerator<int>
{
    private readonly int _startMonth;
    private readonly int _endMonth;
    private int _current;

    public IntMonthEnumerator(in int currentYear, in DateOnly startDate, in DateOnly endDate)
    {
        // Year = currentYear;
        _startMonth = currentYear > startDate.Year ? 1 : startDate.Month;
        _endMonth = currentYear < endDate.Year ? 12 : endDate.Month;
    }

    // public int Year { get; }

    public bool MoveNext()
    {
        if (_current == 0)
        {
            _current = _startMonth;
            return true;
        }

        if (_current >= _endMonth)
            return false;

        ++_current;
        return true;
    }

    public void Reset() => throw new NotImplementedException();

    public int Current => _current;

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}