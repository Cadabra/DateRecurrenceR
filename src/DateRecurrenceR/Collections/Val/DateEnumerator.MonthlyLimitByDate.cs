using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Collections.Val;

public partial struct DateEnumerator
{
    internal DateEnumerator(DateOnly startDate, DateOnly stopDate, int interval, GetNextMonthDateDelegate getNextMonthDate)
    {
        _iterator = startDate;
        _stopDate = stopDate;
        _interval = interval;
        _getNextMonthDate = getNextMonthDate;

        _eType = EType.MonthlyLimitByDate;
    }

    private bool MoveNextMonthlyLimitByDate()
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

        Current = _getNextMonthDate(_iterator.Year, _iterator.Month);

        if (GetMonthNumber(DateOnly.MaxValue) - GetMonthNumber(_iterator) < _interval)
            _canMoveNext = false;
        else
            _iterator = _iterator.AddMonths(_interval);

        return true;
    }
    
    private static int GetMonthNumber(DateOnly date)
    {
        return 12 * date.Year + date.Month;
    }
}