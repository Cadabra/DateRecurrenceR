using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Collections.Val;

public partial struct DateEnumerator
{
    internal DateEnumerator(DateOnly startDate, int takeCount, int interval, GetNextMonthDateDelegate getNextMonthDate)
    {
        _iterator = startDate;
        _takeCount = takeCount;
        _interval = interval;
        _getNextMonthDate = getNextMonthDate;
        _count = 0;

        _eType = EType.MonthlyLimitByCount;
    }

    private bool MoveNextMonthlyLimitByCount()
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

        Current = _getNextMonthDate(_iterator.Year, _iterator.Month);
        _count++;

        if (GetMonthNumber(DateOnly.MaxValue) - GetMonthNumber(_iterator) < _interval)
            _canMoveNext = false;
        else
            _iterator = _iterator.AddMonths(_interval);

        return true;
    }
}