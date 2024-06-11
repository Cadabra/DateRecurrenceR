using System;
using System.Collections;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Collections;

internal sealed class WeeklyEnumeratorLimitByCount : IEnumerator<DateOnly>
{
    private readonly int _takeCount;
    private readonly WeeklyHash _hash;
    private int _count;
    private bool _canMoveNext = true;
    private DateOnly _iterator;

    public WeeklyEnumeratorLimitByCount(DateOnly start, int takeCount, WeeklyHash hash)
    {
        _iterator = start;
        _takeCount = takeCount;
        _hash = hash;
    }
    
    public bool MoveNext()
    {
        if (!_canMoveNext || _count >= _takeCount)
        {
            Current = default;
            return false;
        }

        Current = _iterator;
        _count++;

        if (DateOnly.MaxValue.DayNumber - _iterator.DayNumber < _hash[_iterator.DayOfWeek])
        {
            _canMoveNext = false;
        }
        else
        {
            _iterator = _iterator.AddDays(_hash[_iterator.DayOfWeek]);
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