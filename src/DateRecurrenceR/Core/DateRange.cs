using DateRecurrenceR.Helpers;
using DateRecurrenceR.Internals;
using DateRecurrenceR.Recurrences;

namespace DateRecurrenceR.Core;

public readonly struct DateRange
{
    public DateRange(DateOnly from, DateOnly to)
    {
        From = from;
        To = to;
    }

    public DateOnly From { get; init; }
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