using DateRecurrenceR.Core;
using DateRecurrenceR.Internals;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Internals;

[TestSubject(typeof(PatternSerializer))]
public class PatternSerializerTests
{
    [Fact]
    public void Format_with_count_range_appends_start_date_and_count()
    {
        var body = PatternSerializer.Format(new DailyPattern(new Interval(2)));

        PatternSerializer.Format(new DateRange(new DateOnly(2026, 1, 1), 10), body)
            .Should().Be("20260101D2C10");
    }

    [Fact]
    public void Format_with_end_date_range_appends_start_date_and_until()
    {
        var body = PatternSerializer.Format(new DailyPattern(new Interval(2)));

        PatternSerializer.Format(new DateRange(new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31)), body)
            .Should().Be("20260101D2U20261231");
    }

    [Fact]
    public void Format_with_open_ended_range_appends_only_start_date()
    {
        var body = PatternSerializer.Format(new DailyPattern(new Interval(2)));

        PatternSerializer.Format(new DateRange(new DateOnly(2026, 1, 1)), body)
            .Should().Be("20260101D2");
    }

    [Fact]
    public void TryParse_with_range_parts_reads_start_date_and_count()
    {
        PatternSerializer.TryParse("20260101D2C10", allowRangeParts: true, out var components).Should().BeTrue();

        components.Kind.Should().Be(PatternKind.Daily);
        components.DtStart.Should().Be(new DateOnly(2026, 1, 1));
        components.Count.Should().Be(10);
        components.Until.Should().BeNull();
    }

    [Fact]
    public void TryParse_with_range_parts_reads_start_date_and_until()
    {
        PatternSerializer.TryParse("20260101D2U20261231", allowRangeParts: true, out var components).Should().BeTrue();

        components.DtStart.Should().Be(new DateOnly(2026, 1, 1));
        components.Until.Should().Be(new DateOnly(2026, 12, 31));
        components.Count.Should().BeNull();
    }

    [Fact]
    public void TryParse_without_range_parts_rejects_a_leading_date()
    {
        PatternSerializer.TryParse("20260101D2", allowRangeParts: false, out _).Should().BeFalse();
    }

    [Fact]
    public void TryParse_rejects_count_combined_with_until()
    {
        PatternSerializer.TryParse("20260101D2C10U20261231", allowRangeParts: true, out _).Should().BeFalse();
    }
}
