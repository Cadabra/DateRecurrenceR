using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents a daily recurrence pattern defined by an interval.
/// </summary>
public readonly struct DailyPattern
{
    /// <summary>
    /// Initializes a new instance of <see cref="DailyPattern"/> with the given interval.
    /// </summary>
    /// <param name="interval">The number of days between each recurrence.</param>
    public DailyPattern(Interval interval)
    {
        Interval = interval;
    }

    /// <summary>Gets the interval between each daily recurrence.</summary>
    public Interval Interval { get; }
}