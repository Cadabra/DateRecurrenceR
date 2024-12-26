using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents the pattern for a monthly recurrence based on a specific day of the month.
/// </summary>
public readonly struct MonthlyByDayOfMonthPattern
{
    /// <summary>
    /// Initializes a new instance of <see cref="MonthlyByDayOfMonthPattern"/> with the specified interval and day of month.
    /// </summary>
    /// <param name="interval">The interval in months between occurrences.</param>
    /// <param name="dayOfMonth">The day of the month for the recurrence.</param>
    public MonthlyByDayOfMonthPattern(Interval interval, DayOfMonth dayOfMonth)
    {
        Interval = interval;
        DayOfMonth = dayOfMonth;
    }

    /// <summary>Gets the interval in months between occurrences.</summary>
    public Interval Interval { get; }

    /// <summary>Gets the day of the month for the recurrence.</summary>
    public DayOfMonth DayOfMonth { get; }
}