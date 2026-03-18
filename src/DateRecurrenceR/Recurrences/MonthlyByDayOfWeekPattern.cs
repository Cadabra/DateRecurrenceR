using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents a monthly recurrence pattern that repeats on a specific occurrence of a day of the week within a month.
/// </summary>
public readonly struct MonthlyByDayOfWeekPattern
{
    /// <summary>
    /// Initializes a new instance of <see cref="MonthlyByDayOfWeekPattern"/>.
    /// </summary>
    /// <param name="interval">The number of months between each recurrence.</param>
    /// <param name="dayOfWeek">The day of the week on which the recurrence falls.</param>
    /// <param name="indexOfDay">Which occurrence of <paramref name="dayOfWeek"/> within the month (e.g. first, second, last).</param>
    public MonthlyByDayOfWeekPattern(Interval interval, DayOfWeek dayOfWeek, IndexOfDay indexOfDay)
    {
        Interval = interval;
        DayOfWeek = dayOfWeek;
        IndexOfDay = indexOfDay;
    }

    /// <summary>Gets the interval between each monthly recurrence.</summary>
    public Interval Interval { get; }

    /// <summary>Gets the day of the week on which the recurrence falls.</summary>
    public DayOfWeek DayOfWeek { get; }

    /// <summary>Gets which occurrence of <see cref="DayOfWeek"/> within the month is targeted.</summary>
    public IndexOfDay IndexOfDay { get; }
}