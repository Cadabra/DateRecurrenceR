using DateRecurrenceR.Core;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit;

/// <summary>
/// Tests for the <see cref="Recurrence.Union(IEnumerator{DateOnly}, IEnumerator{DateOnly})"/> overloads.
/// </summary>
[TestSubject(typeof(Recurrence))]
public sealed class RecurrenceUnionTests
{
    private static IEnumerator<DateOnly> Daily(DateOnly begin, int count, int interval)
    {
        return Recurrence.Daily(begin, count, new Interval(interval));
    }

    [Fact]
    public void Union_of_two_enumerators_merges_dates_in_ascending_order()
    {
        var union = Recurrence.Union(
            Daily(new DateOnly(2026, 1, 1), 3, 2), // 1, 3, 5
            Daily(new DateOnly(2026, 1, 2), 3, 2)); // 2, 4, 6

        Collector.Collect(union).Should().Equal(
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 2),
            new DateOnly(2026, 1, 3),
            new DateOnly(2026, 1, 4),
            new DateOnly(2026, 1, 5),
            new DateOnly(2026, 1, 6));
    }

    [Fact]
    public void Union_skips_duplicate_dates()
    {
        var union = Recurrence.Union(
            Daily(new DateOnly(2026, 1, 1), 3, 2), // 1, 3, 5
            Daily(new DateOnly(2026, 1, 1), 3, 1)); // 1, 2, 3

        Collector.Collect(union).Should().Equal(
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 2),
            new DateOnly(2026, 1, 3),
            new DateOnly(2026, 1, 5));
    }

    [Fact]
    public void Union_of_three_enumerators_merges_all_dates()
    {
        var union = Recurrence.Union(
            Daily(new DateOnly(2026, 1, 1), 2, 3), // 1, 4
            Daily(new DateOnly(2026, 1, 2), 2, 3), // 2, 5
            Daily(new DateOnly(2026, 1, 3), 2, 3)); // 3, 6

        Collector.Collect(union).Should().HaveCount(6).And.BeInAscendingOrder();
    }

    [Fact]
    public void Union_accepts_an_array_of_enumerators()
    {
        var union = Recurrence.Union(
            Daily(new DateOnly(2026, 1, 1), 2, 1),
            Daily(new DateOnly(2026, 2, 1), 2, 1),
            Daily(new DateOnly(2026, 3, 1), 2, 1),
            Daily(new DateOnly(2026, 4, 1), 2, 1));

        Collector.Collect(union).Should().HaveCount(8).And.BeInAscendingOrder();
    }

    [Fact]
    public void Union_of_unions_flattens_the_inner_enumerators()
    {
        var inner = Recurrence.Union(
            Daily(new DateOnly(2026, 1, 1), 2, 2), // 1, 3
            Daily(new DateOnly(2026, 1, 2), 2, 2)); // 2, 4

        var union = Recurrence.Union(inner, Daily(new DateOnly(2026, 1, 5), 2, 1)); // 5, 6

        Collector.Collect(union).Should().HaveCount(6).And.BeInAscendingOrder();
    }

    [Fact]
    public void Union_of_empty_enumerators_is_empty()
    {
        var union = Recurrence.Union(
            Daily(new DateOnly(2026, 1, 1), 0, 1),
            Daily(new DateOnly(2026, 1, 1), 0, 1));

        Collector.Collect(union).Should().BeEmpty();
    }

    [Fact]
    public void Union_accepts_an_enumerable_of_enumerators()
    {
        // Typed as IEnumerable to bind the enumerable overload, not the params-array one.
        IEnumerable<IEnumerator<DateOnly>> enumerators =
        [
            Daily(new DateOnly(2026, 1, 1), 2, 1),
            Daily(new DateOnly(2026, 2, 1), 2, 1),
            Daily(new DateOnly(2026, 3, 1), 2, 1)
        ];

        var union = Recurrence.Union(enumerators);

        Collector.Collect(union).Should().HaveCount(6).And.BeInAscendingOrder();
    }

    [Fact]
    public void Union_enumerable_flattens_nested_unions()
    {
        var inner = Recurrence.Union(
            Daily(new DateOnly(2026, 1, 1), 2, 2),
            Daily(new DateOnly(2026, 1, 2), 2, 2));
        IEnumerable<IEnumerator<DateOnly>> enumerators = [inner, Daily(new DateOnly(2026, 1, 5), 2, 1)];

        var union = Recurrence.Union(enumerators);

        Collector.Collect(union).Should().HaveCount(6).And.BeInAscendingOrder();
    }

    [Fact]
    public void Union_enumerator_does_not_support_Reset()
    {
        var union = Recurrence.Union(
            Daily(new DateOnly(2026, 1, 1), 2, 1),
            Daily(new DateOnly(2026, 1, 2), 2, 1));

        var act = () => union.Reset();

        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void Union_enumerator_can_be_disposed()
    {
        var union = Recurrence.Union(
            Daily(new DateOnly(2026, 1, 1), 2, 1),
            Daily(new DateOnly(2026, 1, 2), 2, 1));

        var act = () => union.Dispose();

        act.Should().NotThrow();
    }
}
