using System.Collections;

namespace DateRecurrenceR.Collections;

internal struct EmptyEnumerator : IEnumerator<DateOnly>
{
    public bool MoveNext()
    {
        return false;
    }

    public void Reset()
    {
        throw new NotSupportedException();
    }

    public DateOnly Current => default;

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}