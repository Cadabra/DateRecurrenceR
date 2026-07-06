using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(RecurrenceInfoHelper))]
public class RecurrenceInfoHelperTests
{
    [Fact]
    public void Merge_by_count_takes_the_smaller_count()
    {
        var range = (startDate: new DateOnly(2026, 1, 1), end: new DateOnly(2026, 12, 31), count: 10);

        var (_, count) = range.Merge(new DateOnly(2026, 1, 1), 3);

        count.Should().Be(3);
    }

    [Fact]
    public void Merge_by_count_keeps_the_original_count_when_take_is_larger()
    {
        var range = (startDate: new DateOnly(2026, 1, 1), end: new DateOnly(2026, 12, 31), count: 5);

        var (_, count) = range.Merge(new DateOnly(2026, 1, 1), 100);

        count.Should().Be(5);
    }

    [Fact]
    public void Merge_by_count_moves_the_start_to_fromDate_inside_the_range()
    {
        var range = (startDate: new DateOnly(2026, 1, 1), end: new DateOnly(2026, 12, 31), count: 10);

        var (start, _) = range.Merge(new DateOnly(2026, 6, 1), 10);

        start.Should().Be(new DateOnly(2026, 6, 1));
    }

    [Fact]
    public void Merge_by_count_clamps_the_start_to_the_range_end()
    {
        var range = (startDate: new DateOnly(2026, 1, 1), end: new DateOnly(2026, 12, 31), count: 10);

        var (start, _) = range.Merge(new DateOnly(2027, 6, 1), 10);

        start.Should().Be(new DateOnly(2026, 12, 31));
    }

    [Fact]
    public void Merge_by_dates_returns_the_intersection()
    {
        var range = (startDate: new DateOnly(2026, 1, 1), endDate: new DateOnly(2026, 12, 31));

        var (start, end) = range.Merge(new DateOnly(2026, 3, 1), new DateOnly(2027, 6, 1));

        start.Should().Be(new DateOnly(2026, 3, 1));
        end.Should().Be(new DateOnly(2026, 12, 31));
    }

    [Fact]
    public void Merge_by_dates_keeps_the_range_when_it_is_narrower()
    {
        var range = (startDate: new DateOnly(2026, 3, 1), endDate: new DateOnly(2026, 6, 1));

        var (start, end) = range.Merge(new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31));

        start.Should().Be(new DateOnly(2026, 3, 1));
        end.Should().Be(new DateOnly(2026, 6, 1));
    }
}
