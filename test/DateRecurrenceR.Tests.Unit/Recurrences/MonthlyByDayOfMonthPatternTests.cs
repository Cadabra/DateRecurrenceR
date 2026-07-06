using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(MonthlyByDayOfMonthPattern))]
public sealed class MonthlyByDayOfMonthPatternTests : PatternContractTests<MonthlyByDayOfMonthPattern>
{
    protected override MonthlyByDayOfMonthPattern CreateSample()
    {
        return new MonthlyByDayOfMonthPattern(new Interval(6), new DayOfMonth(31));
    }

    protected override IEnumerable<string> InvalidInputs =>
    [
        ..base.InvalidInputs,
        "M1",
        "M1D0",
        "M1D32",
        "M1D15X",
        "M1I22" // a different pattern kind
    ];

    [Fact]
    public void ToString_produces_expected_string()
    {
        var pattern = new MonthlyByDayOfMonthPattern(new Interval(3), new DayOfMonth(15));

        pattern.ToString().Should().Be("M3D15");
    }
}
