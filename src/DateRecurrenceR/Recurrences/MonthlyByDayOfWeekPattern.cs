using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents the pattern for a monthly recurrence based on a specific day of the week.
/// </summary>
public readonly struct MonthlyByDayOfWeekPattern
{
    /// <summary>
    /// Initializes a new instance of <see cref="MonthlyByDayOfWeekPattern"/> with the specified parameters.
    /// </summary>
    /// <param name="interval">The interval in months between occurrences.</param>
    /// <param name="dayOfWeek">The day of the week for the recurrence.</param>
    /// <param name="indexOfDay">The index of the day within the month.</param>
    public MonthlyByDayOfWeekPattern(Interval interval, DayOfWeek dayOfWeek, IndexOfDay indexOfDay)
    {
        Interval = interval;
        DayOfWeek = dayOfWeek;
        IndexOfDay = indexOfDay;
    }

    /// <summary>Gets the interval in months between occurrences.</summary>
    public Interval Interval { get; }

    /// <summary>Gets the day of the week for the recurrence.</summary>
    public DayOfWeek DayOfWeek { get; }

    /// <summary>Gets the index of the day within the month.</summary>
    public IndexOfDay IndexOfDay { get; }
}