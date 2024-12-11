using System.Collections;

namespace ConsoleHelper;

public sealed class WeekEnumerator : IEnumerator<int>
{
    private readonly DayOfWeek _firstDayOfWeek;
    private readonly int _currentYear;
    private readonly int _month;
    private readonly DateOnly _startDate;
    private readonly DateOnly _endDate;
    private readonly DateOnly _fromDate;
    private readonly DateOnly _toDate;
    private int _current = 0;
    private int _startDateDayNumber = int.MinValue;

    public WeekEnumerator(in int currentYear,
        in int month,
        in DateOnly startDate,
        in DateOnly endDate,
        DayOfWeek firstDayOfWeek)
    {
        _currentYear = currentYear;
        _month = month;
        _startDate = startDate;
        _endDate = endDate;
        _firstDayOfWeek = firstDayOfWeek;
        var num1 = DateTime.DaysInMonth(_currentYear, _month);
        _fromDate = new DateOnly(_currentYear, _month, 1);
        _toDate = new DateOnly(_currentYear, _month, num1);
        var num2 = num1 + (7 + (int) _fromDate.DayOfWeek - (int) _firstDayOfWeek) % 7;
        AffectedWeeks = num2 / 7 + (num2 % 7 > 0 ? 1 : 0);
    }

    public int Month => _month;

    public int AffectedWeeks { get; }

    public bool MoveNext()
    {
        if (_startDateDayNumber == int.MinValue)
        {
            _startDateDayNumber = (int) (_fromDate.DayNumber - _fromDate.DayOfWeek + (int) _firstDayOfWeek - 1);
            Current = _current;
            return true;
        }

        ++_current;
        _startDateDayNumber += 7;
        if (_startDateDayNumber > _toDate.DayNumber)
            return false;
        Current = _current;
        return true;
    }

    public void Reset() => throw new NotImplementedException();

    public int Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }

    public DayEnumerator GetDayEnumerator(DayOfWeek firstDayOfWeek)
    {
        return new DayEnumerator(in _currentYear, in _month, in _current, in _startDate, in _endDate, firstDayOfWeek);
    }
}