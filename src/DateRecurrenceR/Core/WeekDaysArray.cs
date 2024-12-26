#if NET8_0_OR_GREATER
namespace DateRecurrenceR.Core;

/// <summary>
/// Represents array of week days. The first day of week is Sunday.
/// </summary>
[System.Runtime.CompilerServices.InlineArray(DaysInWeek)]
public struct WeekDaysArray
{
    private bool _item;
    
    public bool this[int i]
    {
        get => this[i];
        internal set => this[i] = value;
    }

    public int Length => DaysInWeek;
    
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