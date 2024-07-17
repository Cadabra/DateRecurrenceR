using System.Collections;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Collections;

internal struct WeeklyEnumeratorLimitByDate : IEnumerator<DateOnly>
{
    private readonly WeeklyHash _hash;
    private readonly DateOnly _stopDate;
    private bool _canMoveNext = true;
    private DateOnly _iterator;

    public WeeklyEnumeratorLimitByDate(DateOnly start,
        DateOnly stop,
        WeeklyHash hash)
    {
        _iterator = start;
        _stopDate = stop;
        _hash = hash;
    }

    public bool MoveNext()
    {
        if (!_canMoveNext)
        {
            return false;
        }

        if (_iterator > _stopDate)
        {
            Current = default;
            _canMoveNext = false;
            return false;
        }

        Current = _iterator;

        if (DateOnly.MaxValue.DayNumber - _iterator.DayNumber < _hash[_iterator.DayOfWeek])
            _canMoveNext = false;
        else
            _iterator = _iterator.AddDays(_hash[_iterator.DayOfWeek]);

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