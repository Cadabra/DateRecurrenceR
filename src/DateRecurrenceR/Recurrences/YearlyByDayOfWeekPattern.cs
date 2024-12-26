using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents the pattern for a yearly recurrence based on a specific day of the week.
/// </summary>
public readonly struct YearlyByDayOfWeekPattern
{
    /// <summary>
    /// Initializes a new instance of <see cref="YearlyByDayOfWeekPattern"/> with the specified parameters.
    /// </summary>
    /// <param name="interval">The interval in years between occurrences.</param>
    /// <param name="dayOfWeek">The day of the week for the recurrence.</param>
    /// <param name="indexOfDay">The index of the day within the month.</param>
    /// <param name="monthOfYear">The month of the year for the recurrence.</param>
    public YearlyByDayOfWeekPattern(
        Interval interval,
        DayOfWeek dayOfWeek,
        IndexOfDay indexOfDay,
        MonthOfYear monthOfYear)
    {
        Interval = interval;
        DayOfWeek = dayOfWeek;
        IndexOfDay = indexOfDay;
        MonthOfYear = monthOfYear;
    }

    /// <summary>Gets the interval in years between occurrences.</summary>
    public Interval Interval { get; }

    /// <summary>Gets the day of the week for the recurrence.</summary>
    public DayOfWeek DayOfWeek { get; }

    /// <summary>Gets the index of the day within the month.</summary>
    public IndexOfDay IndexOfDay { get; }

    /// <summary>Gets the month of the year for the recurrence.</summary>
    public MonthOfYear MonthOfYear { get; }
}