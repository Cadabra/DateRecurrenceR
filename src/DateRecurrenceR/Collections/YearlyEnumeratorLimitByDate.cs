using System.Collections;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Collections;

internal sealed class YearlyEnumeratorLimitByDate : IEnumerator<DateOnly>
{
    private readonly GetNextDateDelegate _getNextDate;
    private readonly int _interval;
    private readonly DateOnly _stopDate;
    private bool _canMoveNext = true;
    private int _iterator;

    public YearlyEnumeratorLimitByDate(int year, DateOnly stopDate, int interval, GetNextDateDelegate getNextDate)
    {
        _iterator = year;
        _stopDate = stopDate;
        _interval = interval;
        _getNextDate = getNextDate;
    }

    public bool MoveNext()
    {
        Current = _getNextDate(_iterator);

        if (!_canMoveNext || Current > _stopDate)
        {
            Current = default;
            return false;
        }

        if (DateOnly.MaxValue.Year - _iterator < _interval)
            _canMoveNext = false;
        else
            _iterator += _interval;

        return true;
    }

    public void Reset()
    {
        throw new NotSupportedException();
    }

    public DateOnly Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}