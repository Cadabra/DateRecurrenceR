using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents a yearly recurrence pattern that repeats on a specific day of the year.
/// </summary>
public readonly struct YearlyByDayOfYearPattern
{
    /// <summary>
    /// Initializes a new instance of <see cref="YearlyByDayOfYearPattern"/>.
    /// </summary>
    /// <param name="interval">The number of years between each recurrence.</param>
    /// <param name="dayOfYear">The day of the year on which the recurrence falls.</param>
    public YearlyByDayOfYearPattern(Interval interval, DayOfYear dayOfYear)
    {
        Interval = interval;
        DayOfYear = dayOfYear;
    }

    /// <summary>Gets the interval between each yearly recurrence.</summary>
    public Interval Interval { get; }

    /// <summary>Gets the day of the year on which the recurrence falls.</summary>
    public DayOfYear DayOfYear { get; }
}