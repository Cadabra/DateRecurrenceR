using System.Collections;

namespace DateRecurrenceR.Collections;

internal sealed class DailyEnumeratorLimitByCount : IEnumerator<DateOnly>
{
    private readonly int _interval;
    private readonly int _takeCount;
    private bool _canMoveNext = true;
    private int _count;
    private DateOnly _iterator;

    public DailyEnumeratorLimitByCount(DateOnly startDate, int takeCount, int interval)
    {
        _iterator = startDate;
        _takeCount = takeCount;
        _interval = interval;
    }

    public bool MoveNext()
    {
        if (!_canMoveNext || _count >= _takeCount)
        {
            Current = default;
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

    public void Reset()
    {
        throw new NotSupportedException();
    }

    public DateOnly Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}