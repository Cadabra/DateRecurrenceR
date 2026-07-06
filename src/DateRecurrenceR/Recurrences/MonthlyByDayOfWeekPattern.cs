using DateRecurrenceR.Core;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents the pattern for a monthly recurrence based on a specific day of the week.
/// </summary>
public readonly struct MonthlyByDayOfWeekPattern : ISpanParsable<MonthlyByDayOfWeekPattern>, IPatternParsable<MonthlyByDayOfWeekPattern>
{
    /// <summary>
    /// Initializes a new instance of <see cref="MonthlyByDayOfWeekPattern"/> with the specified parameters.
    /// </summary>
    /// <param name="interval">The interval in months between occurrences.</param>
    /// <param name="dayOfWeek">The day of the week for the recurrence.</param>
    /// <param name="indexOfDay">The index of the day within the month.</param>
    public MonthlyByDayOfWeekPattern(Interval interval, DayOfWeek dayOfWeek, IndexOfDay indexOfDay)
    {
        Interval = interval;
        DayOfWeek = dayOfWeek;
        IndexOfDay = indexOfDay;
    }

    /// <summary>Gets the interval in months between occurrences.</summary>
    public Interval Interval { get; }

    /// <summary>Gets the day of the week for the recurrence.</summary>
    public DayOfWeek DayOfWeek { get; }

    /// <summary>Gets the index of the day within the month.</summary>
    public IndexOfDay IndexOfDay { get; }

    /// <summary>
    /// Returns the compact string representation of this pattern,
    /// for example <c>M1I22</c>.
    /// The day of the week is a <see cref="DayOfWeek"/> digit and <see cref="Core.IndexOfDay.Last"/> is encoded as <c>L</c>.
    /// </summary>
    public override string ToString()
    {
        return PatternSerializer.Format(in this);
    }

    /// <summary>
    /// Returns the compact string representation of this pattern together with the specified date range,
    /// for example <c>20260101M1I22C10</c>.
    /// The resulting string can be turned back into a recurrence by <see cref="Recurrence.FromString(string)"/>.
    /// </summary>
    /// <param name="dateRange">The date range to include as the leading start date and the trailing count or end-date part.</param>
    public string ToString(DateRange dateRange)
    {
        return PatternSerializer.Format(dateRange, PatternSerializer.Format(in this));
    }

    /// <summary>
    /// Parses a compact pattern string, for example <c>M1I22</c>,
    /// into a <see cref="MonthlyByDayOfWeekPattern"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">Ignored; the format is culture-invariant.</param>
    /// <exception cref="FormatException">Thrown when <paramref name="s"/> is not a valid pattern string for this pattern.</exception>
    public static MonthlyByDayOfWeekPattern Parse(string s, IFormatProvider? provider = null)
    {
        return PatternParser.Parse<MonthlyByDayOfWeekPattern>(s);
    }

    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    public static MonthlyByDayOfWeekPattern Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        return PatternParser.Parse<MonthlyByDayOfWeekPattern>(s);
    }

    /// <summary>
    /// Tries to parse a compact pattern string into a <see cref="MonthlyByDayOfWeekPattern"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">Ignored; the format is culture-invariant.</param>
    /// <param name="result">When this method returns <see langword="true"/>, contains the parsed pattern.</param>
    /// <returns><see langword="true"/> if the string was parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out MonthlyByDayOfWeekPattern result)
    {
        return PatternParser.TryParse(s.AsSpan(), out result);
    }

    /// <inheritdoc cref="TryParse(string?, IFormatProvider?, out MonthlyByDayOfWeekPattern)"/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out MonthlyByDayOfWeekPattern result)
    {
        return PatternParser.TryParse(s, out result);
    }

    static PatternKind IPatternParsable<MonthlyByDayOfWeekPattern>.Kind => PatternKind.MonthlyByDayOfWeek;

    static MonthlyByDayOfWeekPattern IPatternParsable<MonthlyByDayOfWeekPattern>.FromComponents(in PatternComponents components)
    {
        return new MonthlyByDayOfWeekPattern(components.Interval, components.DayOfWeek, components.IndexOfDay);
    }
}
