using System.Collections;

namespace DateRecurrenceR.Collections;

public struct DailyEnumerator : IEnumerator<DateOnly>
{
    private readonly int _interval;
    private readonly int _takeCount;
    private bool _canMoveNext = false;
    private int _count = 0;
    private DateOnly _iterator;

    internal DailyEnumerator(DateOnly startDate, int takeCount, int interval)
    {
        _iterator = startDate;
        _takeCount = takeCount;
        _interval = interval;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void Reset()
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public DateOnly Current { get; private set; }

    object IEnumerator.Current => Current;

    /// <inheritdoc />
    public void Dispose()
    {
    }
}