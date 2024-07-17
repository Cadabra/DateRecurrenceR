using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Collections.Val;

public partial struct DateEnumerator
{
    internal DateEnumerator(int year, DateOnly stopDate, int interval, GetNextYearDateDelegate getNextYearDate)
    {
        _intIterator = year;
        _stopDate = stopDate;
        _interval = interval;
        _getNextYearDate = getNextYearDate;
        
        _eType = EType.YearlyLimitByDate;
    }

    private bool MoveNextYearlyLimitByDate()
    {
        Current = _getNextYearDate(_intIterator);

        if (!_canMoveNext)
        {
            return false;
        }

        if (Current > _stopDate)
        {
            Current = default;
            _canMoveNext = false;
            return false;
        }
        
        // if (!_canMoveNext || Current > _stopDate)
        // {
        //     Current = default;
        //     return false;
        // }

        if (DateOnly.MaxValue.Year - _intIterator < _interval)
            _canMoveNext = false;
        else
            _intIterator += _intIterator;

        return true;
    }
}