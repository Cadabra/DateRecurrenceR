using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents the pattern for a yearly recurrence based on a specific day of the year.
/// </summary>
public readonly struct YearlyByDayOfYearPattern
{
    /// <summary>
    /// Initializes a new instance of <see cref="YearlyByDayOfYearPattern"/> with the specified parameters.
    /// </summary>
    /// <param name="interval">The interval in years between occurrences.</param>
    /// <param name="dayOfYear">The day of the year for the recurrence.</param>
    public YearlyByDayOfYearPattern(Interval interval, DayOfYear dayOfYear)
    {
        Interval = interval;
        DayOfYear = dayOfYear;
    }

    /// <summary>Gets the interval in years between occurrences.</summary>
    public Interval Interval { get; }

    /// <summary>Gets the day of the year for the recurrence.</summary>
    public DayOfYear DayOfYear { get; }
}