namespace DateRecurrenceR.Collections.Val;

public partial struct DateEnumerator
{
    internal DateEnumerator(DateOnly startDate, int takeCount, int interval)
    {
        _iterator = startDate;
        _takeCount = takeCount;
        _interval = interval;
        _count = 0;

        _eType = EType.DailyLimitByCount;
    }

    private bool MoveNextDailyLimitByCount()
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

        if (DateOnly.MaxValue.DayNumber - _iterator.DayNumber < _interval)
            _canMoveNext = false;
        else
            _iterator = _iterator.AddDays(_interval);

        return true;
    }
}