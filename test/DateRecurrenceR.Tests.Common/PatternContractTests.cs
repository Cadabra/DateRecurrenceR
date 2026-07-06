using FluentAssertions;
using Xunit;

namespace DateRecurrenceR.Tests.Common;

/// <summary>
/// Contract tests for the pattern structs' compact string format. Derive per pattern type and
/// supply a sample instance; the facts below verify the <see cref="ISpanParsable{TSelf}"/>
/// round-trip behavior shared by all patterns.
/// </summary>
/// <typeparam name="TPattern">The pattern type under test.</typeparam>
public abstract class PatternContractTests<TPattern> where TPattern : struct, ISpanParsable<TPattern>
{
    /// <summary>Creates a representative pattern instance for round-trip checks.</summary>
    protected abstract TPattern CreateSample();

    /// <summary>Inputs that no pattern type parses. Extend per type if needed.</summary>
    protected virtual IEnumerable<string> InvalidInputs =>
    [
        "",
        "   ",
        "X1",
        "D",
        "20260101D2C10", // full recurrence strings are not pattern-only strings
        "D2C10"
    ];

    [Fact]
    public void ToString_then_Parse_round_trips()
    {
        var pattern = CreateSample();

        TPattern.Parse(pattern.ToString()!, null).Should().Be(pattern);
    }

    [Fact]
    public void Span_and_string_parsing_agree()
    {
        var text = CreateSample().ToString()!;

        var fromString = TPattern.Parse(text, null);
        var fromSpan = TPattern.Parse(text.AsSpan(), null);

        fromSpan.Should().Be(fromString);
    }

    [Fact]
    public void TryParse_round_trips()
    {
        var pattern = CreateSample();

        TPattern.TryParse(pattern.ToString(), null, out var result).Should().BeTrue();
        result.Should().Be(pattern);
    }

    [Fact]
    public void TryParse_returns_false_for_invalid_input()
    {
        foreach (var input in InvalidInputs)
        {
            TPattern.TryParse(input, null, out _).Should().BeFalse($"'{input}' is not a valid pattern string");
        }
    }

    [Fact]
    public void TryParse_returns_false_for_null()
    {
        TPattern.TryParse((string?)null, null, out _).Should().BeFalse();
    }

    [Fact]
    public void Parse_is_case_insensitive_and_ignores_surrounding_whitespace()
    {
        var pattern = CreateSample();
        var text = $"  {pattern.ToString()!.ToLowerInvariant()}  ";

        TPattern.Parse(text, null).Should().Be(pattern);
    }

    [Fact]
    public void Parse_throws_FormatException_for_invalid_input()
    {
        foreach (var input in InvalidInputs)
        {
            var act = () => TPattern.Parse(input, null);

            act.Should().Throw<FormatException>($"'{input}' is not a valid pattern string");
        }
    }
}
