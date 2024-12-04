using DateRecurrenceR.Collections;

namespace DateRecurrenceR;

/// <summary>
/// Provides static methods for recurrence dates
/// </summary>
public readonly partial struct Recurrence
{
    private static readonly EmptyEnumerator EmptyEnumerator = new();

    public static IEnumerator<DateOnly> Union(IEnumerator<DateOnly> d1, IEnumerator<DateOnly> d2)
    {
        return new UnionEnumerator(new[] {d1, d2});
    }

    public static IEnumerator<DateOnly> Union(params IEnumerator<DateOnly>[] enumerators)
    {
        return new UnionEnumerator(enumerators);
    }

    private static DateOnly DateOnlyMin(DateOnly val1, DateOnly val2)
    {
        return val1 <= val2 ? val1 : val2;
    }
}