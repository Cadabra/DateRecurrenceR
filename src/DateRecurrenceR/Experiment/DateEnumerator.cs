using System.Collections;

namespace DateRecurrenceR.Experiment;

public struct DateEnumerator : IEnumerator<DateOnly>
{
    // private readonly GetNextMonthDateDelegate _getNextDate;
    // private readonly int _interval;
    private readonly int _takeCount;
    private bool _canMoveNext;
    private int _count;
    private DateOnly _iterator;
    private EnumeratorParams _enumeratorParams;

    public DateEnumerator(EnumeratorParams enumeratorParams)
    {
        _count = enumeratorParams.P3;
        _iterator = DateOnly.FromDayNumber(enumeratorParams.P1);
        _enumeratorParams = enumeratorParams;
        _takeCount = enumeratorParams.P2;
        _canMoveNext = _count < _takeCount;
    }

    private DateOnly _current;

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

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public DateOnly Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}