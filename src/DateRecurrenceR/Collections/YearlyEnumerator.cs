using System.Collections;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Collections;

/// <summary>
/// Enumerates dates in a yearly recurrence pattern.
/// </summary>
public struct YearlyEnumerator : IEnumerator<DateOnly>
{
    private readonly GetNextDateDelegate _getNextDate;
    private readonly int _interval;
    private readonly int _takeCount;
    private bool _canMoveNext = true;
    private int _count = 0;
    private int _iterator = 0;

    internal YearlyEnumerator(int year, int takeCount, int interval, GetNextDateDelegate getNextDate)
    {
        _iterator = year;
        _takeCount = takeCount;
        _interval = interval;
        _getNextDate = getNextDate;
    }

    /// <inheritdoc />
    public bool MoveNext()
    {
        if (!_canMoveNext || _count >= _takeCount)
        {
            Current = default;
            return false;
        }

        Current = _getNextDate(_iterator);
        _count++;

        if (DateOnly.MaxValue.Year - _iterator < _interval)
            _canMoveNext = false;
        else
            _iterator += _interval;

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