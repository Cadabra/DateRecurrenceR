using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(YearlyByDayOfWeekPattern))]
public sealed class YearlyByDayOfWeekPatternTests : PatternContractTests<YearlyByDayOfWeekPattern>
{
    protected override YearlyByDayOfWeekPattern CreateSample()
    {
        return new YearlyByDayOfWeekPattern(
            new Interval(4),
            DayOfWeek.Monday,
            IndexOfDay.First,
            new MonthOfYear(1));
    }

    protected override IEnumerable<string> InvalidInputs =>
    [
        ..base.InvalidInputs,
        "Y1",
        "Y1M3",
        "Y1M3I52",
        "Y1M13I22",
        "Y1M3D10" // a different pattern kind
    ];

    [Fact]
    public void ToString_produces_expected_string()
    {
        var pattern = new YearlyByDayOfWeekPattern(
            new Interval(2),
            DayOfWeek.Thursday,
            IndexOfDay.Fourth,
            new MonthOfYear(11));

        pattern.ToString().Should().Be("Y2M11I44");
    }
}
