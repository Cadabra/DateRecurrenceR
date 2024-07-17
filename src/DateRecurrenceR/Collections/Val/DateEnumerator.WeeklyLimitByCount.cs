using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Collections.Val;

public partial struct DateEnumerator
{
    internal DateEnumerator(DateOnly start, int takeCount, WeeklyHash hash)
    {
        _iterator = start;
        _takeCount = takeCount;
        _hash = hash;
        _count = 0;
        
        _eType = EType.WeeklyLimitByCount;
    }

    private bool MoveNextWeeklyLimitByCount()
    {
        if (!_canMoveNext)
        {
            return false;
        }

        if (_count >= _takeCount)
        {
            Current = default;
            _canMoveNext = false;
            return false;
        }

        Current = _iterator;
        _count++;

        if (DateOnly.MaxValue.DayNumber - _iterator.DayNumber < _hash[_iterator.DayOfWeek])
            _canMoveNext = false;
        else
            _iterator = _iterator.AddDays(_hash[_iterator.DayOfWeek]);

        return true;
    }
}