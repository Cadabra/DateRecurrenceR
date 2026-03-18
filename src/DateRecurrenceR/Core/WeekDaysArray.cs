#if NET8_0_OR_GREATER
namespace DateRecurrenceR.Core;

/// <summary>
/// Represents array of week days. The first day of week is Sunday.
/// </summary>
[System.Runtime.CompilerServices.InlineArray(DaysInWeek)]
public struct WeekDaysArray
{
    private bool _item;

    /// <summary>Gets the total number of elements in the array.</summary>
    public int Length => DaysInWeek;

#pragma warning disable CS9181 // Inline array indexer will not be used for element access expression
    // This write-only indexer is needed for internal object-initializer syntax used in tests.
    // The getter delegates to the compiler-generated InlineArray element accessor.
    internal bool this[int i]
    {
        get => this[i];
        set => this[i] = value;
    }
#pragma warning restore CS9181

    /// <summary>
    /// Returns the number of selected (true) days in the array.
    /// </summary>
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
#endif