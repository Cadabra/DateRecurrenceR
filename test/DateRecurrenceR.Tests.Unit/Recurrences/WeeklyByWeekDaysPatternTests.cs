using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(WeeklyByWeekDaysPattern))]
public sealed class WeeklyByWeekDaysPatternTests : PatternContractTests<WeeklyByWeekDaysPattern>
{
    protected override WeeklyByWeekDaysPattern CreateSample()
    {
        return new WeeklyByWeekDaysPattern(
            new Interval(2),
            new WeekDays(DayOfWeek.Sunday, DayOfWeek.Wednesday, DayOfWeek.Saturday),
            DayOfWeek.Wednesday);
    }

    protected override IEnumerable<string> InvalidInputs =>
    [
        ..base.InvalidInputs,
        "W1",
        "W1D",
        "W1D7",
        "W1D11",
        "W1D1S",
        "W1D1S7",
        "W1D1S0X",
        "W0D1",
        "D1" // a different pattern kind
    ];

    [Fact]
    public void ToString_produces_expected_string()
    {
        var pattern = new WeeklyByWeekDaysPattern(
            new Interval(1),
            new WeekDays(DayOfWeek.Monday, DayOfWeek.Friday),
            DayOfWeek.Sunday);

        pattern.ToString().Should().Be("W1D15S0");
    }

    [Fact]
    public void Parse_defaults_first_day_of_week_to_Monday_when_S_part_is_omitted()
    {
        var parsed = WeeklyByWeekDaysPattern.Parse("W1D2");

        parsed.FirstDayOfWeek.Should().Be(DayOfWeek.Monday);
    }

    [Fact]
    public void Parse_accepts_days_in_any_order()
    {
        WeeklyByWeekDaysPattern.Parse("W1D51S0").ToString().Should().Be("W1D15S0");
    }
}
