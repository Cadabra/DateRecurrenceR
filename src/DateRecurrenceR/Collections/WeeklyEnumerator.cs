using DateRecurrenceR.Internals;
using System.Collections;

namespace DateRecurrenceR.Collections;

/// <summary>
/// Enumerates dates in a weekly recurrence pattern.
/// </summary>
public struct WeeklyEnumerator : IEnumerator<DateOnly>
{
    private readonly DateOnly _start;
    private readonly int _takeCount;
    private readonly WeeklyHash _hash;
    private bool _canMoveNext = true;
    private int _count = 0;
    private int _nextIncrement = 0;

    internal WeeklyEnumerator(DateOnly start, int takeCount, WeeklyHash hash)
    {
        _start = start;
        _takeCount = takeCount;
        _hash = hash;
    }

    /// <inheritdoc />
    public bool MoveNext()
    {
        if (!_canMoveNext || _count >= _takeCount)
        {
            Current = default;
            return false;
        }

        if (_count == 0)
        {
            Current = _start;
        }
        else
        {
            Current = Current.AddDays(_nextIncrement);
        }

        _count++;

        _nextIncrement = _hash[Current.DayOfWeek];

        if (DateOnly.MaxValue.DayNumber - Current.DayNumber < _nextIncrement)
        {
            _canMoveNext = false;
        }

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

    internal static readonly WeeklyEnumerator Empty = new(default, 0, default);

    /// <inheritdoc />
    public void Dispose()
    {
    }
}
