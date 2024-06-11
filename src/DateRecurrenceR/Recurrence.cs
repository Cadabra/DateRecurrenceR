using System;
using DateRecurrenceR.Collections;

namespace DateRecurrenceR;

public readonly partial struct Recurrence
{
    private static readonly EmptyEnumerator EmptyEnumerator = new EmptyEnumerator();
    
    private static DateOnly DateOnlyMin(DateOnly val1, DateOnly val2)
    {
        return val1 <= val2 ? val1 : val2;
    }
}