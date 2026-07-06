using System.Runtime.CompilerServices;

namespace DateRecurrenceR.Core;

/// <summary>
/// Represents array of week days. The first day of week is Sunday.
/// </summary>
[InlineArray(DaysInWeek)]
public struct WeekDaysArray
{
    // Element access (daysArray[i]) is provided by the compiler for [InlineArray] types;
    // a declared indexer would be ignored for element access expressions (CS9181).
    private bool _item;

    /// <summary>Gets the number of elements in the array.</summary>
    public static int Length => DaysInWeek;

    /// <summary>
    /// Returns the count of days that are set to <see langword="true"/>.
    /// </summary>
    /// <returns>The number of selected days.</returns>
    public int GetCountOfSelected()
    {
        return (this[0] ? 1 : 0)
               + (this[1] ? 1 : 0)
               + (this[2] ? 1 : 0)
               + (this[3] ? 1 : 0)
               + (this[4] ? 1 : 0)
               + (this[5] ? 1 : 0)
               + (this[6] ? 1 : 0);
    }
}
