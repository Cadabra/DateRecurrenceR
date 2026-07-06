using DateRecurrenceR.Core;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Recurrences;

/// <summary>
/// Represents the pattern for a weekly recurrence based on specific days of the week.
/// </summary>
public readonly struct WeeklyByWeekDaysPattern : ISpanParsable<WeeklyByWeekDaysPattern>, IPatternParsable<WeeklyByWeekDaysPattern>
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

    /// <summary>
    /// Returns the compact string representation of this pattern,
    /// for example <c>W1D15S0</c> (weekly, on Monday and Friday, week starts on Sunday).
    /// Days are <see cref="DayOfWeek"/> digits, 0 = Sunday .. 6 = Saturday;
    /// the <c>S</c> part is the first day of the week.
    /// </summary>
    public override string ToString()
    {
        return PatternSerializer.Format(in this);
    }

    /// <summary>
    /// Returns the compact string representation of this pattern together with the specified date range,
    /// for example <c>20260101W1D15S0C10</c>.
    /// The resulting string can be turned back into a recurrence by <see cref="Recurrence.FromString(string)"/>.
    /// </summary>
    /// <param name="dateRange">The date range to include as the leading start date and the trailing count or end-date part.</param>
    public string ToString(DateRange dateRange)
    {
        return PatternSerializer.Format(dateRange, PatternSerializer.Format(in this));
    }

    /// <summary>
    /// Parses a compact pattern string, for example <c>W1D15S0</c>,
    /// into a <see cref="WeeklyByWeekDaysPattern"/>.
    /// When the <c>S</c> part is omitted, Monday is used as the first day of the week.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">Ignored; the format is culture-invariant.</param>
    /// <exception cref="FormatException">Thrown when <paramref name="s"/> is not a valid pattern string for this pattern.</exception>
    public static WeeklyByWeekDaysPattern Parse(string s, IFormatProvider? provider = null)
    {
        return PatternParser.Parse<WeeklyByWeekDaysPattern>(s);
    }

    /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
    public static WeeklyByWeekDaysPattern Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        return PatternParser.Parse<WeeklyByWeekDaysPattern>(s);
    }

    /// <summary>
    /// Tries to parse a compact pattern string into a <see cref="WeeklyByWeekDaysPattern"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">Ignored; the format is culture-invariant.</param>
    /// <param name="result">When this method returns <see langword="true"/>, contains the parsed pattern.</param>
    /// <returns><see langword="true"/> if the string was parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out WeeklyByWeekDaysPattern result)
    {
        return PatternParser.TryParse(s.AsSpan(), out result);
    }

    /// <inheritdoc cref="TryParse(string?, IFormatProvider?, out WeeklyByWeekDaysPattern)"/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out WeeklyByWeekDaysPattern result)
    {
        return PatternParser.TryParse(s, out result);
    }

    static PatternKind IPatternParsable<WeeklyByWeekDaysPattern>.Kind => PatternKind.WeeklyByWeekDays;

    static WeeklyByWeekDaysPattern IPatternParsable<WeeklyByWeekDaysPattern>.FromComponents(in PatternComponents components)
    {
        return new WeeklyByWeekDaysPattern(components.Interval, components.WeekDays, components.FirstDayOfWeek);
    }
}
