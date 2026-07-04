namespace DateRecurrenceR.Internals;

/// <summary>
/// Contract implemented by every pattern struct so that <see cref="PatternParser"/> can parse it
/// from the compact string format without per-pattern plumbing.
/// </summary>
/// <typeparam name="TSelf">The pattern type itself.</typeparam>
internal interface IPatternParsable<TSelf> where TSelf : struct, IPatternParsable<TSelf>
{
    /// <summary>Gets the <see cref="PatternKind"/> this pattern type accepts.</summary>
    static abstract PatternKind Kind { get; }

    /// <summary>Creates a pattern from parsed components of a matching <see cref="Kind"/>.</summary>
    static abstract TSelf FromComponents(in PatternComponents components);
}

/// <summary>
/// Shared parsing plumbing for the pattern structs' <c>Parse</c>/<c>TryParse</c> methods.
/// </summary>
internal static class PatternParser
{
    public static T Parse<T>(string s) where T : struct, IPatternParsable<T>
    {
        ArgumentNullException.ThrowIfNull(s);

        return Parse<T>(s.AsSpan());
    }

    public static T Parse<T>(ReadOnlySpan<char> s) where T : struct, IPatternParsable<T>
    {
        if (!TryParse<T>(s, out var result))
        {
            throw new FormatException($"The input is not a valid pattern string for {typeof(T).Name}.");
        }

        return result;
    }

    public static bool TryParse<T>(ReadOnlySpan<char> s, out T result) where T : struct, IPatternParsable<T>
    {
        if (PatternSerializer.TryParse(s, allowRangeParts: false, out var components) &&
            components.Kind == T.Kind)
        {
            result = T.FromComponents(in components);
            return true;
        }

        result = default;
        return false;
    }
}
