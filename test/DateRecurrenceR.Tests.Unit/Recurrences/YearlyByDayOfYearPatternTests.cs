using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(YearlyByDayOfYearPattern))]
public sealed class YearlyByDayOfYearPatternTests : PatternContractTests<YearlyByDayOfYearPattern>
{
    protected override YearlyByDayOfYearPattern CreateSample()
    {
        return new YearlyByDayOfYearPattern(new Interval(1), new DayOfYear(366));
    }

    protected override IEnumerable<string> InvalidInputs =>
    [
        ..base.InvalidInputs,
        "Y1",
        "Y1D0",
        "Y1D367",
        "Y1M3D10" // a different pattern kind
    ];

    [Fact]
    public void ToString_produces_expected_string()
    {
        var pattern = new YearlyByDayOfYearPattern(new Interval(1), new DayOfYear(100));

        pattern.ToString().Should().Be("Y1D100");
    }
}
