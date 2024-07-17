using DateRecurrenceR.Collections;

namespace DateRecurrenceR;

public readonly partial struct RecurrenceVal
{
    private static readonly EmptyEnumerator EmptyEnumerator = new();
    
    private static DateOnly DateOnlyMin(DateOnly val1, DateOnly val2)
    {
        return val1 <= val2 ? val1 : val2;
    }
}