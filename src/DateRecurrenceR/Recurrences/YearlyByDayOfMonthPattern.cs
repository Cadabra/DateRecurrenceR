using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents the pattern for a yearly recurrence based on a specific day of the month within a year.
/// </summary>
public readonly struct YearlyByDayOfMonthPattern
{
    /// <summary>
    /// Initializes a new instance of <see cref="YearlyByDayOfMonthPattern"/> with the specified parameters.
    /// </summary>
    /// <param name="interval">The interval in years between occurrences.</param>
    /// <param name="dayOfMonth">The day of the month for the recurrence.</param>
    /// <param name="monthOfYear">The month of the year for the recurrence.</param>
    public YearlyByDayOfMonthPattern(Interval interval, DayOfMonth dayOfMonth, MonthOfYear monthOfYear)
    {
        Interval = interval;
        DayOfMonth = dayOfMonth;
        MonthOfYear = monthOfYear;
    }

    /// <summary>Gets the interval in years between occurrences.</summary>
    public Interval Interval { get; }

    /// <summary>Gets the day of the month for the recurrence.</summary>
    public DayOfMonth DayOfMonth { get; }

    /// <summary>Gets the month of the year for the recurrence.</summary>
    public MonthOfYear MonthOfYear { get; }
}