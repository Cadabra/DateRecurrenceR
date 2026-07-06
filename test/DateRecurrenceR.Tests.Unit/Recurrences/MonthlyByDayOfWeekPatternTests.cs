using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(MonthlyByDayOfWeekPattern))]
public sealed class MonthlyByDayOfWeekPatternTests : PatternContractTests<MonthlyByDayOfWeekPattern>
{
    protected override MonthlyByDayOfWeekPattern CreateSample()
    {
        return new MonthlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Sunday, IndexOfDay.Last);
    }

    protected override IEnumerable<string> InvalidInputs =>
    [
        ..base.InvalidInputs,
        "M1",
        "M1I",
        "M1I2",
        "M1I02",
        "M1I52",
        "M1I27",
        "M1IX2",
        "M1D15" // a different pattern kind
    ];

    [Fact]
    public void ToString_produces_expected_string()
    {
        var pattern = new MonthlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Tuesday, IndexOfDay.Second);

        pattern.ToString().Should().Be("M1I22");
    }

    [Fact]
    public void ToString_encodes_Last_as_L()
    {
        var pattern = new MonthlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Friday, IndexOfDay.Last);

        pattern.ToString().Should().Be("M1IL5");
    }
}
