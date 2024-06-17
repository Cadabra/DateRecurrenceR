using DateRecurrenceR.Collections;

namespace DateRecurrenceR;

public readonly partial struct RecurrenceRef
{
    private static readonly EmptyEnumerator EmptyEnumerator = new();

    public static IEnumerator<DateOnly> Union(params IEnumerator<DateOnly>[] enumerators)
    {
        return new UnionEnumerator(enumerators);
    }

    private static DateOnly DateOnlyMin(DateOnly val1, DateOnly val2)
    {
        return val1 <= val2 ? val1 : val2;
    }
}