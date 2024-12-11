using System.Collections;

namespace ConsoleHelper;

public sealed class YearEnumerator : IEnumerator<MonthEnumerable>
{
    private readonly DateOnly _startDate;
    private readonly DateOnly _endDate;
    private int _current = 0;
    private DayOfWeek _firstDayOfWeek;

    public YearEnumerator(DateOnly startDate, DateOnly endDate, DayOfWeek firstDayOfWeek)
    {
        _startDate = startDate;
        _endDate = endDate;
        _firstDayOfWeek = firstDayOfWeek;
    }

    public bool MoveNext()
    {
        if (_current == 0)
        {
            _current = _startDate.Year;
            Current = new MonthEnumerable(in _current, in _startDate, in _endDate, _firstDayOfWeek);
            return true;
        }

        if (_current >= _endDate.Year)
            return false;
        ++_current;
        Current = new MonthEnumerable(in _current, in _startDate, in _endDate, _firstDayOfWeek);
        return true;
    }

    public void Reset() => throw new NotImplementedException();

    public MonthEnumerable Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}
public sealed class IntEnumerator : IEnumerator<int>
{
    private readonly int _startYear;
    private readonly int _endYear;
    private int _current = 0;

    public IntEnumerator(DateOnly startDate, DateOnly endDate)
    {
        _startYear = startDate.Year;
        _endYear = endDate.Year;
    }

    public bool MoveNext()
    {
        if (_current == 0)
        {
            _current = _startYear;
            // Current = new MonthEnumerable(in _current, in _startDate, in _endDate, _firstDayOfWeek);
            return true;
        }

        if (_current >= _endYear)
            return false;
        ++_current;
        // Current = new MonthEnumerable(in _current, in _startDate, in _endDate, _firstDayOfWeek);
        return true;
    }

    public void Reset() => throw new NotImplementedException();

    public int Current => _current;

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}