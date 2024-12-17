using System.Collections;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Collections;

internal sealed class MonthlyEnumeratorLimitByDate : IEnumerator<DateOnly>
{
    private readonly GetNextMonthDateDelegate _getNextDate;
    private readonly int _interval;
    private readonly DateOnly _stopDate;
    private bool _canMoveNext = true;
    private DateOnly _iterator;

    public MonthlyEnumeratorLimitByDate(DateOnly startDate, DateOnly stopDate, int interval,
        GetNextMonthDateDelegate getNextDate)
    {
        _iterator = startDate;
        _stopDate = stopDate;
        _interval = interval;
        _getNextDate = getNextDate;
    }

    public bool MoveNext()
    {
        if (!_canMoveNext || _iterator > _stopDate)
        {
            Current = default;
            return false;
        }

        Current = _getNextDate(_iterator.Year, _iterator.Month);

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
        return MonthsInYear * date.Year + date.Month;
    }
}