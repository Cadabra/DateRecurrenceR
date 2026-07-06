using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(DailyPattern))]
public sealed class DailyPatternTests : PatternContractTests<DailyPattern>
{
    protected override DailyPattern CreateSample()
    {
        return new DailyPattern(new Interval(7));
    }

    protected override IEnumerable<string> InvalidInputs =>
    [
        ..base.InvalidInputs,
        "D0",
        "D-1",
        "X2",
        "D2X",
        "20260101D2",
        "D99999999999",
        "W1D1" // a different pattern kind
    ];

    [Fact]
    public void ToString_produces_expected_string()
    {
        var pattern = new DailyPattern(new Interval(2));

        pattern.ToString().Should().Be("D2");
    }

    [Fact]
    public void ToString_with_count_range_appends_start_date_and_count()
    {
        var pattern = new DailyPattern(new Interval(2));
        var dateRange = new DateRange(new DateOnly(2026, 1, 1), 10);

        pattern.ToString(dateRange).Should().Be("20260101D2C10");
    }

    [Fact]
    public void ToString_with_end_date_range_appends_start_date_and_until()
    {
        var pattern = new DailyPattern(new Interval(2));
        var dateRange = new DateRange(new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31));

        pattern.ToString(dateRange).Should().Be("20260101D2U20261231");
    }

    [Fact]
    public void ToString_with_open_ended_range_appends_only_start_date()
    {
        var pattern = new DailyPattern(new Interval(2));
        var dateRange = new DateRange(new DateOnly(2026, 1, 1));

        pattern.ToString(dateRange).Should().Be("20260101D2");
    }
}
