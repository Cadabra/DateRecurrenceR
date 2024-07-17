using DateRecurrenceR.Collections;
using DateRecurrenceR.Collections.Val;

namespace DateRecurrenceR;

public readonly partial struct RecurrenceVal
{
    private static readonly EmptyEnumerator EmptyEnumerator = new();

    public static UnionEnumeratorVal Union(IEnumerator<DateOnly> d1, IEnumerator<DateOnly> d2)
    {
        return new UnionEnumeratorVal(new [] {d1, d2});
    }

    public static UnionEnumeratorVal Union(params IEnumerator<DateOnly>[] enumerators)
    {
        return new UnionEnumeratorVal(enumerators);
    }
    
    private static DateOnly DateOnlyMin(DateOnly val1, DateOnly val2)
    {
        return val1 <= val2 ? val1 : val2;
    }
}