using DateRecurrenceR.Core;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents the pattern for a yearly recurrence based on a specific day of the week.
/// </summary>
public readonly struct YearlyByDayOfWeekPattern : ISpanParsable<YearlyByDayOfWeekPattern>, IPatternParsable<YearlyByDayOfWeekPattern>
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

    /// <summary>
    /// Returns the compact string representation of this pattern,
    /// for example <c>Y1M3I22</c>.
    /// The day of the week is a <see cref="DayOfWeek"/> digit and <see cref="Core.IndexOfDay.Last"/> is encoded as <c>L</c>.
    /// </summary>
    public override string ToString()
    {
        return PatternSerializer.Format(in this);
    }

    /// <summary>
    /// Returns the compact string representation of this pattern together with the specified date range,
    /// for example <c>20260101Y1M3I22C10</c>.
    /// The resulting string can be turned back into a recurrence by <see cref="Recurrence.FromString(string)"/>.
    /// </summary>
    /// <param name="dateRange">The date range to include as the leading start date and the trailing count or end-date part.</param>
    public string ToString(DateRange dateRange)
    {
        return PatternSerializer.Format(dateRange, PatternSerializer.Format(in this));
    }

    /// <summary>
    /// Parses a compact pattern string, for example <c>Y1M3I22</c>,
    /// into a <see cref="YearlyByDayOfWeekPattern"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">Ignored; the format is culture-invariant.</param>
    /// <exception cref="FormatException">Thrown when <paramref name="s"/> is not a valid pattern string for this pattern.</exception>
    public static YearlyByDayOfWeekPattern Parse(string s, IFormatProvider? provider = null)
    {
        return PatternParser.Parse<YearlyByDayOfWeekPattern>(s);
    }

    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    public static YearlyByDayOfWeekPattern Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        return PatternParser.Parse<YearlyByDayOfWeekPattern>(s);
    }

    /// <summary>
    /// Tries to parse a compact pattern string into a <see cref="YearlyByDayOfWeekPattern"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">Ignored; the format is culture-invariant.</param>
    /// <param name="result">When this method returns <see langword="true"/>, contains the parsed pattern.</param>
    /// <returns><see langword="true"/> if the string was parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out YearlyByDayOfWeekPattern result)
    {
        return PatternParser.TryParse(s.AsSpan(), out result);
    }

    /// <inheritdoc cref="TryParse(string?, IFormatProvider?, out YearlyByDayOfWeekPattern)"/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out YearlyByDayOfWeekPattern result)
    {
        return PatternParser.TryParse(s, out result);
    }

    static PatternKind IPatternParsable<YearlyByDayOfWeekPattern>.Kind => PatternKind.YearlyByDayOfWeek;

    static YearlyByDayOfWeekPattern IPatternParsable<YearlyByDayOfWeekPattern>.FromComponents(in PatternComponents components)
    {
        return new YearlyByDayOfWeekPattern(components.Interval, components.DayOfWeek, components.IndexOfDay,
            components.MonthOfYear);
    }
}
