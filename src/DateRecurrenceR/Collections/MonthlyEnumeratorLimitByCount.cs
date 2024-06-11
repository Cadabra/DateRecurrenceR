using System.Collections;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Collections;

internal sealed class MonthlyEnumeratorLimitByCount : IEnumerator<DateOnly>
{
    private readonly GetNextMonthDateDelegate _getNextDate;
    private readonly int _interval;
    private readonly int _takeCount;
    private bool _canMoveNext = true;
    private int _count;
    private DateOnly _iterator;

    public MonthlyEnumeratorLimitByCount(DateOnly startDate, int takeCount, int interval,
        GetNextMonthDateDelegate getNextDate)
    {
        _iterator = startDate;
        _takeCount = takeCount;
        _interval = interval;
        _getNextDate = getNextDate;
    }

    public bool MoveNext()
    {
        if (!_canMoveNext || _count >= _takeCount)
        {
            Current = default;
            return false;
        }

        Current = _getNextDate(_iterator.Year, _iterator.Month);
        _count++;

        if (GetMonthNumber(DateOnly.MaxValue) - GetMonthNumber(_iterator) < _interval)
            _canMoveNext = false;
        else
            _iterator = _iterator.AddMonths(_interval);

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

    private static int GetMonthNumber(DateOnly date)
    {
        return 12 * date.Year + date.Month;
    }
}