using System.Globalization;
using System.Text;
using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;

namespace DateRecurrenceR.Internals;

/// <summary>
/// Identifies the recurrence pattern type encoded in a pattern string.
/// </summary>
internal enum PatternKind
{
    Daily,
    WeeklyByWeekDays,
    MonthlyByDayOfMonth,
    MonthlyByDayOfWeek,
    YearlyByDayOfMonth,
    YearlyByDayOfWeek,
    YearlyByDayOfYear
}

/// <summary>
/// The parsed components of a pattern string. Only the fields relevant to <see cref="Kind"/> are set.
/// </summary>
internal struct PatternComponents
{
    public PatternKind Kind;
    public Interval Interval;
    public WeekDays WeekDays;
    public DayOfWeek FirstDayOfWeek;
    public DayOfWeek DayOfWeek;
    public IndexOfDay IndexOfDay;
    public DayOfMonth DayOfMonth;
    public MonthOfYear MonthOfYear;
    public DayOfYear DayOfYear;
    public DateOnly? DtStart;
    public DateOnly? Until;
    public int? Count;
}

/// <summary>
/// Writes and parses the compact, datetime-like pattern string format.
/// <para>
/// Pattern grammar (days of the week are <see cref="DayOfWeek"/> digits, 0 = Sunday .. 6 = Saturday;
/// the index of a day is <c>1</c>..<c>4</c> or <c>L</c> for the last one):
/// <code>
/// D[interval]                        daily                   D2
/// W[interval]D[days](S[firstDay])    weekly                  W2D15S0
/// M[interval]D[dayOfMonth]           monthly by day          M1D15
/// M[interval]I[index][dayOfWeek]     monthly by week day     M1I22, M1IL5
/// Y[interval]M[month]D[dayOfMonth]   yearly by day           Y1M3D10
/// Y[interval]M[month]I[index][dayOfWeek] yearly by week day  Y1M3I22
/// Y[interval]D[dayOfYear]            yearly by day of year   Y1D100
/// </code>
/// A full recurrence string prefixes the start date and appends an optional limit:
/// <c>yyyyMMdd</c> + pattern + (<c>C[count]</c> | <c>U[yyyyMMdd]</c>), for example <c>20260101D2C10</c>.
/// </para>
/// </summary>
internal static class PatternSerializer
{
    private const string DateFormat = "yyyyMMdd";

    public static string Format(in DailyPattern pattern)
    {
        return $"D{(int)pattern.Interval}";
    }

    public static string Format(in WeeklyByWeekDaysPattern pattern)
    {
        var sb = new StringBuilder(16);
        sb.Append('W').Append(pattern.Interval).Append('D');

        for (var i = 0; i < DaysInWeek; i++)
        {
            if (pattern.WeekDays[(DayOfWeek)i]) sb.Append((char)('0' + i));
        }

        sb.Append('S').Append((char)('0' + (int)pattern.FirstDayOfWeek));

        return sb.ToString();
    }

    public static string Format(in MonthlyByDayOfMonthPattern pattern)
    {
        return $"M{(int)pattern.Interval}D{(int)pattern.DayOfMonth}";
    }

    public static string Format(in MonthlyByDayOfWeekPattern pattern)
    {
        return $"M{(int)pattern.Interval}I{GetIndexChar(pattern.IndexOfDay)}{(int)pattern.DayOfWeek}";
    }

    public static string Format(in YearlyByDayOfMonthPattern pattern)
    {
        return $"Y{(int)pattern.Interval}M{(int)pattern.MonthOfYear}D{(int)pattern.DayOfMonth}";
    }

    public static string Format(in YearlyByDayOfWeekPattern pattern)
    {
        return $"Y{(int)pattern.Interval}M{(int)pattern.MonthOfYear}" +
               $"I{GetIndexChar(pattern.IndexOfDay)}{(int)pattern.DayOfWeek}";
    }

    public static string Format(in YearlyByDayOfYearPattern pattern)
    {
        return $"Y{(int)pattern.Interval}D{(int)pattern.DayOfYear}";
    }

    public static string Format(DateRange dateRange, string patternBody)
    {
        var sb = new StringBuilder(patternBody.Length + 20);
        sb.Append(dateRange.BeginDate.ToString(DateFormat, CultureInfo.InvariantCulture));
        sb.Append(patternBody);

        if (dateRange.Count is not null)
        {
            sb.Append('C').Append(dateRange.Count.Value);
        }
        else if (dateRange.EndDate is { } endDate && endDate != DateOnly.MaxValue)
        {
            sb.Append('U').Append(endDate.ToString(DateFormat, CultureInfo.InvariantCulture));
        }

        return sb.ToString();
    }

    public static bool TryParse(ReadOnlySpan<char> s, bool allowRangeParts, out PatternComponents result)
    {
        result = default;

        var remaining = s.Trim();
        if (remaining.IsEmpty) return false;

        DateOnly? dtStart = null;
        if (char.IsAsciiDigit(remaining[0]))
        {
            if (!allowRangeParts) return false;
            if (!TryReadDate(ref remaining, out var startDate)) return false;
            dtStart = startDate;
        }

        if (!TryReadPattern(ref remaining, out result)) return false;
        result.DtStart = dtStart;

        if (remaining.IsEmpty) return true;
        if (!allowRangeParts) return false;

        var tag = char.ToUpperInvariant(remaining[0]);
        remaining = remaining[1..];
        switch (tag)
        {
            case 'C':
                if (!TryReadInt(ref remaining, out var count)) return false;
                result.Count = count;
                break;
            case 'U':
                if (!TryReadDate(ref remaining, out var until)) return false;
                result.Until = until;
                break;
            default:
                return false;
        }

        return remaining.IsEmpty;
    }

    private static bool TryReadPattern(ref ReadOnlySpan<char> s, out PatternComponents result)
    {
        result = default;
        if (s.IsEmpty) return false;

        var kindChar = char.ToUpperInvariant(s[0]);
        s = s[1..];

        if (!TryReadInt(ref s, out var interval) || interval < 1) return false;
        result.Interval = new Interval(interval);

        switch (kindChar)
        {
            case 'D':
                result.Kind = PatternKind.Daily;
                return true;

            case 'W':
            {
                if (!TryReadTag(ref s, 'D')) return false;

                Span<bool> days = stackalloc bool[DaysInWeek];
                var dayCount = 0;
                while (!s.IsEmpty && char.IsAsciiDigit(s[0]))
                {
                    var day = s[0] - '0';
                    if (day >= DaysInWeek) return false;
                    if (days[day]) return false;
                    days[day] = true;
                    dayCount++;
                    s = s[1..];
                }

                if (dayCount == 0) return false;

                var firstDayOfWeek = DayOfWeek.Monday;
                if (!s.IsEmpty && char.ToUpperInvariant(s[0]) == 'S')
                {
                    s = s[1..];
                    if (s.IsEmpty || !char.IsAsciiDigit(s[0])) return false;
                    var day = s[0] - '0';
                    if (day >= DaysInWeek) return false;
                    firstDayOfWeek = (DayOfWeek)day;
                    s = s[1..];
                }

                result.Kind = PatternKind.WeeklyByWeekDays;
                result.WeekDays = new WeekDays(days[0], days[1], days[2], days[3], days[4], days[5], days[6]);
                result.FirstDayOfWeek = firstDayOfWeek;
                return true;
            }

            case 'M':
            {
                if (s.IsEmpty) return false;
                var fieldChar = char.ToUpperInvariant(s[0]);
                s = s[1..];

                if (fieldChar == 'D')
                {
                    if (!TryReadInt(ref s, out var dayOfMonth)) return false;
                    if (dayOfMonth is < 1 or > 31) return false;

                    result.Kind = PatternKind.MonthlyByDayOfMonth;
                    result.DayOfMonth = new DayOfMonth(dayOfMonth);
                    return true;
                }

                if (fieldChar == 'I')
                {
                    if (!TryReadIndexAndDay(ref s, out var indexOfDay, out var dayOfWeek)) return false;

                    result.Kind = PatternKind.MonthlyByDayOfWeek;
                    result.IndexOfDay = indexOfDay;
                    result.DayOfWeek = dayOfWeek;
                    return true;
                }

                return false;
            }

            case 'Y':
            {
                if (s.IsEmpty) return false;
                var fieldChar = char.ToUpperInvariant(s[0]);
                s = s[1..];

                if (fieldChar == 'D')
                {
                    if (!TryReadInt(ref s, out var dayOfYear)) return false;
                    if (dayOfYear is < 1 or > 366) return false;

                    result.Kind = PatternKind.YearlyByDayOfYear;
                    result.DayOfYear = new DayOfYear(dayOfYear);
                    return true;
                }

                if (fieldChar != 'M') return false;
                if (!TryReadInt(ref s, out var month)) return false;
                if (month is < 1 or > MonthsInYear) return false;
                result.MonthOfYear = new MonthOfYear(month);

                if (s.IsEmpty) return false;
                fieldChar = char.ToUpperInvariant(s[0]);
                s = s[1..];

                if (fieldChar == 'D')
                {
                    if (!TryReadInt(ref s, out var dayOfMonth)) return false;
                    if (dayOfMonth is < 1 or > 31) return false;

                    result.Kind = PatternKind.YearlyByDayOfMonth;
                    result.DayOfMonth = new DayOfMonth(dayOfMonth);
                    return true;
                }

                if (fieldChar == 'I')
                {
                    if (!TryReadIndexAndDay(ref s, out var indexOfDay, out var dayOfWeek)) return false;

                    result.Kind = PatternKind.YearlyByDayOfWeek;
                    result.IndexOfDay = indexOfDay;
                    result.DayOfWeek = dayOfWeek;
                    return true;
                }

                return false;
            }

            default:
                return false;
        }
    }

    private static bool TryReadTag(ref ReadOnlySpan<char> s, char tag)
    {
        if (s.IsEmpty || char.ToUpperInvariant(s[0]) != tag) return false;

        s = s[1..];
        return true;
    }

    private static bool TryReadIndexAndDay(ref ReadOnlySpan<char> s, out IndexOfDay indexOfDay, out DayOfWeek dayOfWeek)
    {
        indexOfDay = default;
        dayOfWeek = default;

        if (s.Length < 2) return false;

        var indexChar = char.ToUpperInvariant(s[0]);
        if (indexChar == 'L')
        {
            indexOfDay = IndexOfDay.Last;
        }
        else if (indexChar is >= '1' and <= '4')
        {
            indexOfDay = (IndexOfDay)(indexChar - '1');
        }
        else
        {
            return false;
        }

        var dayChar = s[1];
        if (!char.IsAsciiDigit(dayChar) || dayChar - '0' >= DaysInWeek) return false;
        dayOfWeek = (DayOfWeek)(dayChar - '0');

        s = s[2..];
        return true;
    }

    private static bool TryReadInt(ref ReadOnlySpan<char> s, out int value)
    {
        value = 0;

        var length = 0;
        var accumulator = 0L;
        while (length < s.Length && char.IsAsciiDigit(s[length]))
        {
            accumulator = accumulator * 10 + (s[length] - '0');
            if (accumulator > int.MaxValue) return false;
            length++;
        }

        if (length == 0) return false;

        value = (int)accumulator;
        s = s[length..];
        return true;
    }

    private static bool TryReadDate(ref ReadOnlySpan<char> s, out DateOnly value)
    {
        value = default;

        if (s.Length < DateFormat.Length) return false;
        if (!DateOnly.TryParseExact(s[..DateFormat.Length], DateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out value))
        {
            return false;
        }

        s = s[DateFormat.Length..];
        return true;
    }

    private static char GetIndexChar(IndexOfDay indexOfDay)
    {
        return indexOfDay switch
        {
            IndexOfDay.First => '1',
            IndexOfDay.Second => '2',
            IndexOfDay.Third => '3',
            IndexOfDay.Fourth => '4',
            IndexOfDay.Last => 'L',
            _ => throw new ArgumentOutOfRangeException(nameof(indexOfDay))
        };
    }
}
