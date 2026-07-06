using DateRecurrenceR.Core;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents the pattern for a daily recurrence.
/// </summary>
public readonly struct DailyPattern : ISpanParsable<DailyPattern>, IPatternParsable<DailyPattern>
{
    /// <summary>
    /// Initializes a new instance of <see cref="DailyPattern"/> with the specified interval.
    /// </summary>
    /// <param name="interval">The interval between occurrences.</param>
    public DailyPattern(Interval interval)
    {
        Interval = interval;
    }

    /// <summary>Gets the interval between occurrences.</summary>
    public Interval Interval { get; }

    /// <summary>
    /// Returns the compact string representation of this pattern, for example <c>D2</c>.
    /// </summary>
    public override string ToString()
    {
        return PatternSerializer.Format(in this);
    }

    /// <summary>
    /// Returns the compact string representation of this pattern together with the specified date range,
    /// for example <c>20260101D2C10</c>.
    /// The resulting string can be turned back into a recurrence by <see cref="Recurrence.FromString(string)"/>.
    /// </summary>
    /// <param name="dateRange">The date range to include as the leading start date and the trailing count or end-date part.</param>
    public string ToString(DateRange dateRange)
    {
        return PatternSerializer.Format(dateRange, PatternSerializer.Format(in this));
    }

    /// <summary>
    /// Parses a compact pattern string, for example <c>D2</c>, into a <see cref="DailyPattern"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">Ignored; the format is culture-invariant.</param>
    /// <exception cref="FormatException">Thrown when <paramref name="s"/> is not a valid pattern string for this pattern.</exception>
    public static DailyPattern Parse(string s, IFormatProvider? provider = null)
    {
        return PatternParser.Parse<DailyPattern>(s);
    }

    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    public static DailyPattern Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        return PatternParser.Parse<DailyPattern>(s);
    }

    /// <summary>
    /// Tries to parse a compact pattern string into a <see cref="DailyPattern"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">Ignored; the format is culture-invariant.</param>
    /// <param name="result">When this method returns <see langword="true"/>, contains the parsed pattern.</param>
    /// <returns><see langword="true"/> if the string was parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out DailyPattern result)
    {
        return PatternParser.TryParse(s.AsSpan(), out result);
    }

    /// <inheritdoc cref="TryParse(string?, IFormatProvider?, out DailyPattern)"/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DailyPattern result)
    {
        return PatternParser.TryParse(s, out result);
    }

    static PatternKind IPatternParsable<DailyPattern>.Kind => PatternKind.Daily;

    static DailyPattern IPatternParsable<DailyPattern>.FromComponents(in PatternComponents components)
    {
        return new DailyPattern(components.Interval);
    }
}
