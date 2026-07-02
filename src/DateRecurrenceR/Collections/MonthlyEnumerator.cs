using DateRecurrenceR.Internals;
using System.Collections;

namespace DateRecurrenceR.Collections;

/// <summary>
/// Enumerates dates in a monthly recurrence pattern.
/// </summary>
public struct MonthlyEnumerator : IEnumerator<DateOnly>
{
    private readonly DateOnly _start;
    private readonly MonthlyDateResolver _resolver;
    private readonly int _takeCount;
    private readonly int _interval = 0;
    private int _count = 0;

    internal MonthlyEnumerator(DateOnly startDate, int takeCount, int interval, MonthlyDateResolver resolver)
    {
        _start = startDate;
        _takeCount = takeCount;
        _interval = interval;
        _resolver = resolver;
    }

    /// <inheritdoc />
    public bool MoveNext()
    {
        if (_count >= _takeCount)
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
            Current = Current.AddMonths(_interval);
            Current = _resolver.GetDate(Current.Year, Current.Month);
        }

        _count++;

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

    internal static MonthlyEnumerator Empty = new(default, 0, 0, default);

    /// <inheritdoc />
    public void Dispose()
    {
    }
}
