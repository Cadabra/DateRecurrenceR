namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Strongly-typed recurrence interface that provides type-safe subrange and enumerator access.
/// </summary>
/// <typeparam name="T">The concrete recurrence type returned by subrange operations.</typeparam>
/// <typeparam name="TEnum">The enumerator type returned by <see cref="GetEnumerator"/>.</typeparam>
public interface IRecurrence<T, TEnum> : IRecurrence
    where T : struct, IRecurrence
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