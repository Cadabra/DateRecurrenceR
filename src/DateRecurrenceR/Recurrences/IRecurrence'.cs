namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Defines a strongly-typed contract for a date recurrence pattern with a specific enumerator type.
/// </summary>
/// <typeparam name="T">The concrete recurrence type.</typeparam>
/// <typeparam name="TEnum">The enumerator type for iterating dates.</typeparam>
public interface IRecurrence<T, TEnum> : IRecurrence
    where T :  struct, IRecurrence
    where TEnum : struct, IEnumerator<DateOnly>
{
    /// <inheritdoc cref="IRecurrence.GetSubRange(int)"/>
    new T GetSubRange(int takeCount);

    /// <inheritdoc cref="IRecurrence.GetSubRange(DateOnly, int)"/>
    new T GetSubRange(DateOnly fromDate, int takeCount);

    /// <inheritdoc cref="IRecurrence.GetSubRange(DateOnly, DateOnly)"/>
    new T GetSubRange(DateOnly fromDate, DateOnly toDate);

    /// <inheritdoc cref="IRecurrence.GetEnumerator"/>
    new TEnum GetEnumerator();
}