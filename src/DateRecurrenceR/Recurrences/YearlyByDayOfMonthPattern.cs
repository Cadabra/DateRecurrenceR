using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents a yearly recurrence pattern that repeats on a specific day of a specific month.
/// </summary>
public readonly struct YearlyByDayOfMonthPattern
{
    /// <summary>
    /// Initializes a new instance of <see cref="YearlyByDayOfMonthPattern"/>.
    /// </summary>
    /// <param name="interval">The number of years between each recurrence.</param>
    /// <param name="dayOfMonth">The day of the month on which the recurrence falls.</param>
    /// <param name="monthOfYear">The month in which the recurrence falls.</param>
    public YearlyByDayOfMonthPattern(Interval interval, DayOfMonth dayOfMonth, MonthOfYear monthOfYear)
    {
        Interval = interval;
        DayOfMonth = dayOfMonth;
        MonthOfYear = monthOfYear;
    }

    /// <summary>Gets the interval between each yearly recurrence.</summary>
    public Interval Interval { get; }

    /// <summary>Gets the day of the month on which the recurrence falls.</summary>
    public DayOfMonth DayOfMonth { get; }

    /// <summary>Gets the month of the year in which the recurrence falls.</summary>
    public MonthOfYear MonthOfYear { get; }
}