namespace DateRecurrenceR.Core;

/// <summary>
/// Represents a recurrence range defined by a start date and either an end date or a count of occurrences.
/// </summary>
public readonly struct DateRange
{
    /// <summary>
    /// Initializes a new instance of <see cref="DateRange"/> with the specified start date and no defined end.
    /// </summary>
    /// <param name="beginDate">The start date of the range.</param>
    public DateRange(DateOnly beginDate)
    {
        BeginDate = beginDate;
        EndDate = DateOnly.MaxValue;
        Count = null;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="DateRange"/> with the specified start date and optional end date.
    /// </summary>
    /// <param name="beginDate">The start date of the range.</param>
    /// <param name="endDate">The optional end date of the range.</param>
    public DateRange(DateOnly beginDate, DateOnly? endDate)
    {
        BeginDate = beginDate;
        EndDate = endDate;
        Count = null;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="DateRange"/> with the specified start date and occurrence count.
    /// </summary>
    /// <param name="beginDate">The start date of the range.</param>
    /// <param name="count">The number of occurrences.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is negative.</exception>
    public DateRange(DateOnly beginDate, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        BeginDate = beginDate;
        EndDate = null;
        Count = count;
    }

    /// <summary>Gets the start date of the range.</summary>
    public DateOnly BeginDate { get; }

    /// <summary>Gets the optional end date of the range.</summary>
    public DateOnly? EndDate { get; }

    /// <summary>Gets the optional occurrence count.</summary>
    public int? Count { get; }
}