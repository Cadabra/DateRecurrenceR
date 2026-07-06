using DateRecurrenceR.Internals;
using System.Collections;

namespace DateRecurrenceR.Collections;

/// <summary>
/// Enumerates dates in a monthly recurrence pattern.
/// </summary>
public struct MonthlyEnumerator : IEnumerator<DateOnly>
{
    private static readonly int MaxMonthNumber =
        MonthsInYear * (DateOnly.MaxValue.Year - 1) + DateOnly.MaxValue.Month;

    private readonly DateOnly _start;
    private readonly MonthlyDateResolver _resolver;
    private readonly int _takeCount;
    private readonly int _interval = 0;
    private bool _canMoveNext = true;
    private int _count = 0;
    private int _monthNumber;

    internal MonthlyEnumerator(DateOnly startDate, int takeCount, int interval, MonthlyDateResolver resolver)
    {
        _start = startDate;
        _takeCount = takeCount;
        _interval = interval;
        _resolver = resolver;
        _monthNumber = MonthsInYear * (startDate.Year - 1) + startDate.Month;
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
            Current = _resolver.GetDate(
                (_monthNumber - 1) / MonthsInYear + 1,
                (_monthNumber - 1) % MonthsInYear + 1);
        }

        _count++;

        if (MaxMonthNumber - _monthNumber < _interval)
            _canMoveNext = false;
        else
            _monthNumber += _interval;

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

    internal static readonly MonthlyEnumerator Empty = new(default, 0, 0, default);

    /// <inheritdoc />
    public void Dispose()
    {
    }
}
