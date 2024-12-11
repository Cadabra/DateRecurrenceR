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
        if (this._current == 0)
        {
            this._current = this._startDate.Year;
            this.Current = new MonthEnumerable(in _current, in _startDate, in _endDate, _firstDayOfWeek);
            return true;
        }

        if (this._current >= this._endDate.Year)
            return false;
        ++this._current;
        this.Current = new MonthEnumerable(in _current, in _startDate, in _endDate, _firstDayOfWeek);
        return true;
    }

    public void Reset() => throw new NotImplementedException();

    public MonthEnumerable Current { get; private set; }

    object IEnumerator.Current => (object) this.Current;

    public void Dispose()
    {
    }
}