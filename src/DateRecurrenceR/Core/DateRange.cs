using DateRecurrenceR.Helpers;
using DateRecurrenceR.Internals;
using DateRecurrenceR.Recurrences;

namespace DateRecurrenceR.Core;

/// <summary>
/// Represents a date range with a start and end date.
/// </summary>
public readonly struct DateRange
{
    /// <summary>
    /// Initializes a new instance of <see cref="DateRange"/> with the specified start and end dates.
    /// </summary>
    /// <param name="from">The start date of the range.</param>
    /// <param name="to">The end date of the range.</param>
    public DateRange(DateOnly from, DateOnly to)
    {
        From = from;
        To = to;
    }

    /// <summary>Gets the start date of the range.</summary>
    public DateOnly From { get; init; }

    /// <summary>Gets the end date of the range.</summary>
    public DateOnly To { get; init; }
}

internal readonly struct DateRangeHelper
{
    public static DateRange GetDateRange(
        DateOnly beginDate,
        DateOnly fromDate,
        DateOnly endDate,
        WeeklyHash weeklyHash,
        WeeklyByWeekDaysPattern pattern)
    {
        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            weeklyHash,
            pattern.WeekDays,
            pattern.FirstDayOfWeek,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new DateRange();
        }

        var endDayNumber = WeeklyRecurrenceHelper.GetEndDateNumber(
            startDate,
            pattern.WeekDays,
            pattern.FirstDayOfWeek,
            pattern.Interval,
            endDate);

        return new DateRange(startDate, DateOnly.FromDayNumber(endDayNumber));
    }
    
    public static DateRange GetDateRange(
        DateOnly beginDate,
        DateOnly fromDate,
        int count,
        WeeklyHash weeklyHash,
        WeeklyByWeekDaysPattern pattern)
    {
        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            weeklyHash,
            pattern.WeekDays,
            pattern.FirstDayOfWeek,
            pattern.Interval,
            out var startDate);

        if (!canStart)
        {
            return new DateRange();
        }

        var endDayNumber = WeeklyRecurrenceHelper.GetEndDateNumber(
            startDate,
            pattern.WeekDays,
            pattern.Interval,
            count);

        return new DateRange(startDate, DateOnly.FromDayNumber(endDayNumber));
    }
}