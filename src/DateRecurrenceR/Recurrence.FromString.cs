using System.Diagnostics.CodeAnalysis;
using DateRecurrenceR.Core;
using DateRecurrenceR.Internals;
using DateRecurrenceR.Recurrences;

namespace DateRecurrenceR;

public readonly partial struct Recurrence
{
    /// <summary>
    /// Creates a recurrence from a full recurrence string produced by a pattern's
    /// <c>ToString(DateRange)</c> method, for example <c>20260101D2C10</c>.
    /// The string must start with the start date (<c>yyyyMMdd</c>) and may end with either
    /// a count (<c>C10</c>) or an end date (<c>U20261231</c>).
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <returns>The recurrence as <see cref="IRecurrence"/>. The concrete struct is boxed.</returns>
    /// <exception cref="FormatException">Thrown when <paramref name="s"/> is not a supported recurrence string.</exception>
    public static IRecurrence FromString(string s)
    {
        ArgumentNullException.ThrowIfNull(s);

        return FromString(s.AsSpan());
    }

    /// <inheritdoc cref="FromString(string)"/>
    public static IRecurrence FromString(ReadOnlySpan<char> s)
    {
        if (!TryFromString(s, out var result))
        {
            throw new FormatException("The input is not a supported recurrence string with a leading start date.");
        }

        return result;
    }

    /// <summary>
    /// Creates a recurrence from a pattern-only string produced by a pattern's
    /// <c>ToString()</c> method, for example <c>D2</c>, and the specified date range.
    /// The string must not contain the start date, count, or end-date parts.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="dateRange">The date range for the recurrence.</param>
    /// <returns>The recurrence as <see cref="IRecurrence"/>. The concrete struct is boxed.</returns>
    /// <exception cref="FormatException">Thrown when <paramref name="s"/> is not a supported pattern string.</exception>
    public static IRecurrence FromString(string s, DateRange dateRange)
    {
        ArgumentNullException.ThrowIfNull(s);

        return FromString(s.AsSpan(), dateRange);
    }

    /// <inheritdoc cref="FromString(string, DateRange)"/>
    public static IRecurrence FromString(ReadOnlySpan<char> s, DateRange dateRange)
    {
        if (!TryFromString(s, dateRange, out var result))
        {
            throw new FormatException("The input is not a supported pattern-only string.");
        }

        return result;
    }

    /// <summary>
    /// Tries to create a recurrence from a full recurrence string with a leading start date.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="result">When this method returns <see langword="true"/>, contains the recurrence.</param>
    /// <returns><see langword="true"/> if the string was parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryFromString(string? s, [NotNullWhen(true)] out IRecurrence? result)
    {
        return TryFromString(s.AsSpan(), out result);
    }

    /// <inheritdoc cref="TryFromString(string?, out IRecurrence?)"/>
    public static bool TryFromString(ReadOnlySpan<char> s, [NotNullWhen(true)] out IRecurrence? result)
    {
        result = null;

        if (!PatternSerializer.TryParse(s, allowRangeParts: true, out var components)) return false;
        if (components.DtStart is null) return false;

        var dateRange = components.Count is not null
            ? new DateRange(components.DtStart.Value, components.Count.Value)
            : components.Until is not null
                ? new DateRange(components.DtStart.Value, components.Until.Value)
                : new DateRange(components.DtStart.Value);

        result = Create(in components, dateRange);
        return true;
    }

    /// <summary>
    /// Tries to create a recurrence from a pattern-only string and the specified date range.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="dateRange">The date range for the recurrence.</param>
    /// <param name="result">When this method returns <see langword="true"/>, contains the recurrence.</param>
    /// <returns><see langword="true"/> if the string was parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryFromString(string? s, DateRange dateRange, [NotNullWhen(true)] out IRecurrence? result)
    {
        return TryFromString(s.AsSpan(), dateRange, out result);
    }

    /// <inheritdoc cref="TryFromString(string?, DateRange, out IRecurrence?)"/>
    public static bool TryFromString(ReadOnlySpan<char> s, DateRange dateRange,
        [NotNullWhen(true)] out IRecurrence? result)
    {
        if (!PatternSerializer.TryParse(s, allowRangeParts: false, out var components))
        {
            result = null;
            return false;
        }

        result = Create(in components, dateRange);
        return true;
    }

    private static IRecurrence Create(in PatternComponents components, DateRange dateRange)
    {
        return components.Kind switch
        {
            PatternKind.Daily => DailyRecurrence.New(
                dateRange,
                new DailyPattern(components.Interval)),
            PatternKind.WeeklyByWeekDays => WeeklyByWeekDaysRecurrence.New(
                dateRange,
                new WeeklyByWeekDaysPattern(components.Interval, components.WeekDays, components.FirstDayOfWeek)),
            PatternKind.MonthlyByDayOfMonth => MonthlyByDayOfMonthRecurrence.New(
                dateRange,
                new MonthlyByDayOfMonthPattern(components.Interval, components.DayOfMonth)),
            PatternKind.MonthlyByDayOfWeek => MonthlyByDayOfWeekRecurrence.New(
                dateRange,
                new MonthlyByDayOfWeekPattern(components.Interval, components.DayOfWeek, components.IndexOfDay)),
            PatternKind.YearlyByDayOfMonth => YearlyByDayOfMonthRecurrence.New(
                dateRange,
                new YearlyByDayOfMonthPattern(components.Interval, components.DayOfMonth, components.MonthOfYear)),
            PatternKind.YearlyByDayOfWeek => YearlyByDayOfWeekRecurrence.New(
                dateRange,
                new YearlyByDayOfWeekPattern(components.Interval, components.DayOfWeek, components.IndexOfDay,
                    components.MonthOfYear)),
            PatternKind.YearlyByDayOfYear => YearlyByDayOfYearRecurrence.New(
                dateRange,
                new YearlyByDayOfYearPattern(components.Interval, components.DayOfYear)),
            _ => throw new ArgumentOutOfRangeException(nameof(components))
        };
    }
}
