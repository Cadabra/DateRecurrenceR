namespace DateRecurrenceR.Collections.Val;

public partial struct DateEnumerator
{
    internal DateEnumerator(DateOnly startDate, DateOnly stopDate, int interval)
    {
        _iterator = startDate;
        _stopDate = stopDate;
        _interval = interval;

        _eType = EType.DailyLimitByDate;
    }

    private bool MoveNextDailyLimitByDate()
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

        if (DateOnly.MaxValue.DayNumber - _iterator.DayNumber < _interval)
            _canMoveNext = false;
        else
            _iterator = _iterator.AddDays(_interval);

        return true;
    }
}