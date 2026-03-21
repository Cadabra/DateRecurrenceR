using DateRecurrenceR.Core;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents the pattern for a weekly recurrence based on specific days of the week.
/// </summary>
public readonly struct WeeklyByWeekDaysPattern
{
    /// <summary>
    /// Initializes a new instance of <see cref="WeeklyByWeekDaysPattern"/> with the specified parameters.
    /// </summary>
    /// <param name="interval">The interval in weeks between occurrences.</param>
    /// <param name="weekDays">The days of the week for the recurrence.</param>
    /// <param name="firstDayOfWeek">The first day of the week.</param>
    public WeeklyByWeekDaysPattern(Interval interval, WeekDays weekDays, DayOfWeek firstDayOfWeek)
    {
        Interval = interval;
        WeekDays = weekDays;
        FirstDayOfWeek = firstDayOfWeek;
    }

    /// <summary>Gets the interval in weeks between occurrences.</summary>
    public Interval Interval { get; }

    /// <summary>Gets the days of the week for the recurrence.</summary>
    public WeekDays WeekDays { get; }

    /// <summary>Gets the first day of the week.</summary>
    public DayOfWeek FirstDayOfWeek { get; }
}