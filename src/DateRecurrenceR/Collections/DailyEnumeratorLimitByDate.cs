using System;
using System.Collections;

namespace DateRecurrenceR.Collections;

internal sealed class DailyEnumeratorLimitByDate : IEnumerator<DateOnly>
{
    private readonly DateOnly _stopDate;
    private readonly int _interval;
    private bool _canMoveNext = true;
    private DateOnly _iterator;

    public DailyEnumeratorLimitByDate(DateOnly startDate, DateOnly stopDate, int interval)
    {
        _iterator = startDate;
        _stopDate = stopDate;
        _interval = interval;
    }

    public bool MoveNext()
    {
        if (!_canMoveNext || _iterator > _stopDate)
        {
            Current = default;
            return false;
        }

        Current = _iterator;
        
        if (DateOnly.MaxValue.DayNumber - _iterator.DayNumber < _interval)
        {
            _canMoveNext = false;
        }
        else
        {
            _iterator = _iterator.AddDays(_interval);
        }

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