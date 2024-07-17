using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Collections.Val;

public partial struct DateEnumerator
{
    internal DateEnumerator(DateOnly start, DateOnly stop, WeeklyHash hash)
    {
        _iterator = start;
        _stopDate = stop;
        _hash = hash;

        _eType = EType.WeeklyLimitByDate;
    }

    private bool MoveNextWeeklyLimitByDate()
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
}