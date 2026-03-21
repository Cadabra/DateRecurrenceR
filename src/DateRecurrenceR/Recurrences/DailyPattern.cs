using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents the pattern for a daily recurrence.
/// </summary>
public readonly struct DailyPattern
{
    /// <summary>
    /// Initializes a new instance of <see cref="DailyPattern"/> with the specified interval.
    /// </summary>
    /// <param name="interval">The interval between occurrences.</param>
    public DailyPattern(Interval interval)
    {
        Interval = interval;
    }

    /// <summary>Gets the interval between occurrences.</summary>
    public Interval Interval { get; }
}