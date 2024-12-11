using System.Collections;

namespace ConsoleHelper;

public sealed class DayEnumerator : IEnumerator<DayInfo>
{
    private readonly DayOfWeek _firstDayOfWeek;
    private readonly int _currentYear;
    private readonly int _currentMonth;
    private readonly DateOnly _startDate;
    private readonly DateOnly _endDate;
    private readonly int _startDay;
    private readonly int _endDay;
    private int _currentDayNumber;
    private int _current = 0;

    public DayEnumerator(in int currentYear,
        in int currentMonth,
        in int weekNumber,
        in DateOnly startDate,
        in DateOnly endDate,
        DayOfWeek firstDayOfWeek)
    {
        _currentYear = currentYear;
        _currentMonth = currentMonth;
        _startDate = startDate;
        _endDate = endDate;
        _firstDayOfWeek = firstDayOfWeek;
        var dateOnly = new DateOnly(_currentYear, _currentMonth, 1);
        _startDay = dateOnly.DayNumber - (7 + (int) dateOnly.DayOfWeek - (int) _firstDayOfWeek) % 7 + 7 * weekNumber;
        _endDay = _startDay + 7;
        _currentDayNumber = int.MinValue;
    }

    public bool MoveNext()
    {
        if (_currentDayNumber == int.MinValue)
        {
            _currentDayNumber = _startDay;
            Current = new DayInfo(_currentDayNumber, _currentMonth);
            return true;
        }

        ++_currentDayNumber;
        if (_currentDayNumber >= _endDay)
            return false;
        Current = new DayInfo(_currentDayNumber, _currentMonth);
        return true;
    }

    public void Reset() => throw new NotImplementedException();

    public DayInfo Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}