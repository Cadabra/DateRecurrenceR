using DateRecurrenceR.Collections;

namespace DateRecurrenceR;

/// <summary>
/// Provides static methods for recurrence dates
/// </summary>
public readonly partial struct Recurrence
{
    private static readonly EmptyEnumerator EmptyEnumerator = new();

    private static DateOnly DateOnlyMin(DateOnly val1, DateOnly val2)
    {
        return val1 <= val2 ? val1 : val2;
    }
}