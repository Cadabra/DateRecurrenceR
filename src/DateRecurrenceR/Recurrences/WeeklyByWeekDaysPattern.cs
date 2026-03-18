using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents a weekly recurrence pattern defined by an interval, a set of week days, and a first day of the week.
/// </summary>
public readonly struct WeeklyByWeekDaysPattern
{
    /// <summary>
    /// Initializes a new instance of <see cref="WeeklyByWeekDaysPattern"/>.
    /// </summary>
    /// <param name="interval">The number of weeks between each recurrence cycle.</param>
    /// <param name="weekDays">The days of the week on which the recurrence falls.</param>
    /// <param name="firstDayOfWeek">The first day of the week used for ordering.</param>
    public WeeklyByWeekDaysPattern(Interval interval, WeekDays weekDays, DayOfWeek firstDayOfWeek)
    {
        Interval = interval;
        WeekDays = weekDays;
        FirstDayOfWeek = firstDayOfWeek;
    }

    /// <summary>Gets the interval between each weekly recurrence cycle.</summary>
    public Interval Interval { get; }

    /// <summary>Gets the days of the week on which the recurrence occurs.</summary>
    public WeekDays WeekDays { get; }

    /// <summary>Gets the first day of the week used to order recurrence days.</summary>
    public DayOfWeek FirstDayOfWeek { get; }
}