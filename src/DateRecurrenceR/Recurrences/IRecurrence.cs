namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Defines the base contract for a date recurrence pattern.
/// </summary>
public interface IRecurrence
{
    /// <summary>Gets the first date of the recurrence.</summary>
    DateOnly StartDate { get; }

    /// <summary>Gets the last date of the recurrence.</summary>
    DateOnly StopDate { get; }

    /// <summary>Gets the total count of occurrences in the recurrence.</summary>
    int Count { get; }

    /// <summary>Determines whether the specified date falls within the recurrence.</summary>
    /// <param name="date">The date to check.</param>
    /// <returns><see langword="true"/> if the date is part of the recurrence; otherwise, <see langword="false"/>.</returns>
    bool Contains(DateOnly date);

    /// <summary>Returns a sub-range of the recurrence limited to the specified count from the start.</summary>
    /// <param name="takeCount">The maximum number of occurrences.</param>
    IRecurrence GetSubRange(int takeCount);

    /// <summary>Returns a sub-range of the recurrence starting from the specified date, limited to the specified count.</summary>
    /// <param name="fromDate">The date from which to start the sub-range.</param>
    /// <param name="takeCount">The maximum number of occurrences.</param>
    IRecurrence GetSubRange(DateOnly fromDate, int takeCount);

    /// <summary>Returns a sub-range of the recurrence within the specified date bounds.</summary>
    /// <param name="fromDate">The start date of the sub-range.</param>
    /// <param name="toDate">The end date of the sub-range.</param>
    IRecurrence GetSubRange(DateOnly fromDate, DateOnly toDate);

    /// <summary>Returns an enumerator that iterates through the recurrence dates.</summary>
    IEnumerator<DateOnly> GetEnumerator();
}