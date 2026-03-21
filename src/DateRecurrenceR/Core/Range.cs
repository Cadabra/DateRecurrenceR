namespace DateRecurrenceR.Core;

/// <summary>
/// Represents a recurrence range defined by a start date and either an end date or a count of occurrences.
/// </summary>
public readonly struct Range
{
    /// <summary>
    /// Initializes a new instance of <see cref="Range"/> with the specified start date and no defined end.
    /// </summary>
    /// <param name="beginDate">The start date of the range.</param>
    public Range(DateOnly beginDate)
    {
        BeginDate = beginDate;
        EndDate = DateOnly.MaxValue;
        Count = null;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Range"/> with the specified start date and optional end date.
    /// </summary>
    /// <param name="beginDate">The start date of the range.</param>
    /// <param name="endDate">The optional end date of the range.</param>
    public Range(DateOnly beginDate, DateOnly? endDate)
    {
        BeginDate = beginDate;
        EndDate = endDate;
        Count = null;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Range"/> with the specified start date and occurrence count.
    /// </summary>
    /// <param name="beginDate">The start date of the range.</param>
    /// <param name="count">The number of occurrences.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="count"/> is negative.</exception>
    public Range(DateOnly beginDate, int count)
    {
        if (count < 0)
        {
            throw new ArgumentException();
        }

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