using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents a yearly recurrence pattern that repeats on a specific occurrence of a day of the week within a month.
/// </summary>
public readonly struct YearlyByDayOfWeekPattern
{
    /// <summary>
    /// Initializes a new instance of <see cref="YearlyByDayOfWeekPattern"/>.
    /// </summary>
    /// <param name="interval">The number of years between each recurrence.</param>
    /// <param name="dayOfWeek">The day of the week on which the recurrence falls.</param>
    /// <param name="indexOfDay">Which occurrence of <paramref name="dayOfWeek"/> within the month (e.g. first, second, last).</param>
    /// <param name="monthOfYear">The month in which the recurrence falls.</param>
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

    /// <summary>Gets the interval between each yearly recurrence.</summary>
    public Interval Interval { get; }

    /// <summary>Gets the day of the week on which the recurrence falls.</summary>
    public DayOfWeek DayOfWeek { get; }

    /// <summary>Gets which occurrence of <see cref="DayOfWeek"/> within the month is targeted.</summary>
    public IndexOfDay IndexOfDay { get; }

    /// <summary>Gets the month of the year in which the recurrence falls.</summary>
    public MonthOfYear MonthOfYear { get; }
}