using DateRecurrenceR.Internals;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Internals;

[TestSubject(typeof(PatternParser))]
public class PatternParserTests
{
    [Fact]
    public void TryParse_creates_the_pattern_for_a_matching_kind()
    {
        PatternParser.TryParse<DailyPattern>("D2", out var result).Should().BeTrue();

        result.Interval.Should().Be(new DateRecurrenceR.Core.Interval(2));
    }

    [Fact]
    public void TryParse_rejects_a_pattern_of_a_different_kind()
    {
        PatternParser.TryParse<DailyPattern>("W1D1", out _).Should().BeFalse();
        PatternParser.TryParse<WeeklyByWeekDaysPattern>("D1", out _).Should().BeFalse();
    }

    [Fact]
    public void TryParse_rejects_strings_with_range_parts()
    {
        PatternParser.TryParse<DailyPattern>("20260101D2", out _).Should().BeFalse();
        PatternParser.TryParse<DailyPattern>("D2C10", out _).Should().BeFalse();
        PatternParser.TryParse<DailyPattern>("D2U20261231", out _).Should().BeFalse();
    }

    [Fact]
    public void Parse_throws_FormatException_naming_the_pattern_type()
    {
        var act = () => PatternParser.Parse<DailyPattern>("garbage");

        act.Should().Throw<FormatException>().WithMessage($"*{nameof(DailyPattern)}*");
    }

    [Fact]
    public void Parse_of_string_throws_ArgumentNullException_for_null()
    {
        var act = () => PatternParser.Parse<DailyPattern>((string)null!);

        act.Should().Throw<ArgumentNullException>();
    }
}
