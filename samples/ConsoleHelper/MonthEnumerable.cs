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

    // public DayEnumerator GetDayEnumerator(DayOfWeek firstDayOfWeek)
    // {
    //     return new DayEnumerator(this.Year, in this._current, in this._startDate, in this._endDate, firstDayOfWeek);
    // }

    public WeekEnumerator GetWeekEnumerator()
    {
        return new WeekEnumerator(Year, in _current, in _startDate, in _endDate, _firstDayOfWeek);
    }
}