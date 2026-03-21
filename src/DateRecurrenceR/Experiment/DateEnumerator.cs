using System.Collections;

namespace DateRecurrenceR.Experiment;

/// <summary>
/// Experimental date enumerator that uses <see cref="EnumeratorParams"/> to configure iteration.
/// </summary>
public struct DateEnumerator : IEnumerator<DateOnly>
{
    // private readonly GetNextMonthDateDelegate _getNextDate;
    // private readonly int _interval;
    private readonly int _takeCount;
    private bool _canMoveNext;
    private int _count;
    private DateOnly _iterator;
    private EnumeratorParams _enumeratorParams;

    /// <summary>
    /// Initializes a new instance of <see cref="DateEnumerator"/> with the specified parameters.
    /// </summary>
    /// <param name="enumeratorParams">The parameters that configure the enumerator.</param>
    public DateEnumerator(EnumeratorParams enumeratorParams)
    {
        _count = enumeratorParams.P3;
        _iterator = DateOnly.FromDayNumber(enumeratorParams.P1);
        _enumeratorParams = enumeratorParams;
        _takeCount = enumeratorParams.P2;
        _canMoveNext = _count < _takeCount;
    }

    /// <inheritdoc />
    public bool MoveNext()
    {
        if (!_canMoveNext)
        {
            Current = default;
            return false;
        }

        Current = _enumeratorParams.GetNextDate(_iterator);
        _count++;

        _canMoveNext = _count < _takeCount;

        return true;
    }

    /// <summary>
    /// Resets the enumerator to its initial position.
    /// </summary>
    /// <exception cref="NotSupportedException">Always thrown.</exception>
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