using DateRecurrenceR.Core;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents the pattern for a yearly recurrence based on a specific day of the year.
/// </summary>
public readonly struct YearlyByDayOfYearPattern : ISpanParsable<YearlyByDayOfYearPattern>
{
    /// <summary>
    /// Initializes a new instance of <see cref="YearlyByDayOfYearPattern"/> with the specified parameters.
    /// </summary>
    /// <param name="interval">The interval in years between occurrences.</param>
    /// <param name="dayOfYear">The day of the year for the recurrence.</param>
    public YearlyByDayOfYearPattern(Interval interval, DayOfYear dayOfYear)
    {
        Interval = interval;
        DayOfYear = dayOfYear;
    }

    /// <summary>Gets the interval in years between occurrences.</summary>
    public Interval Interval { get; }

    /// <summary>Gets the day of the year for the recurrence.</summary>
    public DayOfYear DayOfYear { get; }

    /// <summary>
    /// Returns the compact string representation of this pattern,
    /// for example <c>Y1D100</c>.
    /// </summary>
    public override string ToString()
    {
        return PatternSerializer.Format(in this);
    }

    /// <summary>
    /// Returns the compact string representation of this pattern together with the specified date range,
    /// for example <c>20260101Y1D100C10</c>.
    /// The resulting string can be turned back into a recurrence by <see cref="Recurrence.FromString(string)"/>.
    /// </summary>
    /// <param name="dateRange">The date range to include as the leading start date and the trailing count or end-date part.</param>
    public string ToString(DateRange dateRange)
    {
        return PatternSerializer.Format(dateRange, PatternSerializer.Format(in this));
    }

    /// <summary>
    /// Parses a compact pattern string, for example <c>Y1D100</c>,
    /// into a <see cref="YearlyByDayOfYearPattern"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">Ignored; the format is culture-invariant.</param>
    /// <exception cref="FormatException">Thrown when <paramref name="s"/> is not a valid pattern string for this pattern.</exception>
    public static YearlyByDayOfYearPattern Parse(string s, IFormatProvider? provider = null)
    {
        ArgumentNullException.ThrowIfNull(s);

        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    public static YearlyByDayOfYearPattern Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        if (!TryParse(s, provider, out var result))
        {
            throw new FormatException($"The input is not a valid pattern string for {nameof(YearlyByDayOfYearPattern)}.");
        }

        return result;
    }

    /// <summary>
    /// Tries to parse a compact pattern string into a <see cref="YearlyByDayOfYearPattern"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">Ignored; the format is culture-invariant.</param>
    /// <param name="result">When this method returns <see langword="true"/>, contains the parsed pattern.</param>
    /// <returns><see langword="true"/> if the string was parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out YearlyByDayOfYearPattern result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <inheritdoc cref="TryParse(string?, IFormatProvider?, out YearlyByDayOfYearPattern)"/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out YearlyByDayOfYearPattern result)
    {
        if (PatternSerializer.TryParse(s, allowRangeParts: false, out var components) &&
            components.Kind == PatternKind.YearlyByDayOfYear)
        {
            result = new YearlyByDayOfYearPattern(components.Interval, components.DayOfYear);
            return true;
        }

        result = default;
        return false;
    }
}