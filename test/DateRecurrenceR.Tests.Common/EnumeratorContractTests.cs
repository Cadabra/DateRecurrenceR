using FluentAssertions;
using Xunit;

namespace DateRecurrenceR.Tests.Common;

/// <summary>
/// Contract tests for the date enumerators. Derive per enumerator type and supply the factory;
/// the facts below verify the shared <see cref="IEnumerator{T}"/> behavior: exact element count,
/// repeated exhaustion, <c>Reset</c>/<c>Dispose</c> semantics, and the calendar upper bound.
/// </summary>
public abstract class EnumeratorContractTests
{
    /// <summary>Creates an enumerator producing at most <paramref name="takeCount"/> dates from <paramref name="startDate"/>.</summary>
    protected abstract IEnumerator<DateOnly> Create(DateOnly startDate, int takeCount);

    /// <summary>The start date used by the contract facts.</summary>
    protected virtual DateOnly StartDate => new(2026, 1, 15);

    /// <summary>A start date close enough to the calendar end to exercise the overflow guard.</summary>
    protected virtual DateOnly NearMaxStartDate => new(9999, 11, 1);

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    public void MoveNext_returns_true_exactly_takeCount_times(int takeCount)
    {
        var enumerator = Create(StartDate, takeCount);

        var moves = 0;
        while (enumerator.MoveNext()) moves++;

        moves.Should().Be(takeCount);
    }

    [Fact]
    public void MoveNext_returns_false_repeatedly_after_the_last_element()
    {
        var enumerator = Create(StartDate, 2);

        while (enumerator.MoveNext())
        {
        }

        enumerator.MoveNext().Should().BeFalse();
        enumerator.MoveNext().Should().BeFalse();
    }

    [Fact]
    public void Current_is_default_after_the_last_element()
    {
        var enumerator = Create(StartDate, 1);

        while (enumerator.MoveNext())
        {
        }

        enumerator.Current.Should().Be(default(DateOnly));
    }

    [Fact]
    public void Dates_are_strictly_ascending()
    {
        var enumerator = Create(StartDate, 10);

        var dates = Collector.Collect(enumerator);

        dates.Should().BeInAscendingOrder();
        dates.Distinct().Should().HaveCount(dates.Count);
    }

    [Fact]
    public void Enumeration_stops_on_or_before_DateOnly_MaxValue()
    {
        var enumerator = Create(NearMaxStartDate, 1000);

        var count = 0;
        var last = default(DateOnly);
        while (enumerator.MoveNext())
        {
            last = enumerator.Current;
            count++;
            count.Should().BeLessThan(1001, "the enumerator must terminate");
        }

        last.Should().BeOnOrBefore(DateOnly.MaxValue);
    }

    [Fact]
    public void Reset_throws_NotSupportedException()
    {
        var enumerator = Create(StartDate, 1);

        var act = () => enumerator.Reset();

        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void Dispose_can_be_called_safely()
    {
        var enumerator = Create(StartDate, 1);

        var act = () => enumerator.Dispose();

        act.Should().NotThrow();
    }
}
