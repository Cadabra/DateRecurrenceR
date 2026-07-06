using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(YearlyByDayOfMonthPattern))]
public sealed class YearlyByDayOfMonthPatternTests : PatternContractTests<YearlyByDayOfMonthPattern>
{
    protected override YearlyByDayOfMonthPattern CreateSample()
    {
        return new YearlyByDayOfMonthPattern(new Interval(1), new DayOfMonth(29), new MonthOfYear(2));
    }

    protected override IEnumerable<string> InvalidInputs =>
    [
        ..base.InvalidInputs,
        "Y1",
        "Y1M3",
        "Y1M13D1",
        "Y1M0D1",
        "Y1M3D0",
        "Y1M3D32",
        "Y1M3D10X",
        "Y1D100" // a different pattern kind
    ];

    [Fact]
    public void ToString_produces_expected_string()
    {
        var pattern = new YearlyByDayOfMonthPattern(new Interval(1), new DayOfMonth(10), new MonthOfYear(3));

        pattern.ToString().Should().Be("Y1M3D10");
    }
}
