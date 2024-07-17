using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Collections.Val;

public partial struct DateEnumerator
{
    internal DateEnumerator(int year, int takeCount, int interval, GetNextYearDateDelegate getNextYearDate)
    {
        _intIterator = year;
        _takeCount = takeCount;
        _interval = interval;
        _getNextYearDate = getNextYearDate;
        _count = 0;
        
        _eType = EType.YearlyLimitByCount;
    }

    private bool MoveNextYearlyLimitByCount()
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

        Current = _getNextYearDate(_intIterator);
        _count++;

        if (DateOnly.MaxValue.Year - _intIterator < _interval)
            _canMoveNext = false;
        else
            _intIterator += _intIterator;

        return true;
    }
}