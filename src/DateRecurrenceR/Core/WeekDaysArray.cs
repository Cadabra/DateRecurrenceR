#if NET8_0_OR_GREATER
namespace DateRecurrenceR.Core;

/// <summary>
/// Represents array of week days. The first day of week is Sunday.
/// </summary>
[System.Runtime.CompilerServices.InlineArray(DaysInWeek)]
public struct WeekDaysArray
{
    private bool _item;
    
    /// <summary>Gets or sets the element at the specified index.</summary>
    /// <param name="i">The zero-based index of the element.</param>
#pragma warning disable CS9181 // Inline array indexer will not be used for element access expression
    public bool this[int i]
    {
        get => this[i];
        internal set => this[i] = value;
    }
#pragma warning restore CS9181

    /// <summary>Gets the number of elements in the array.</summary>
    public int Length => DaysInWeek;

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
#endif