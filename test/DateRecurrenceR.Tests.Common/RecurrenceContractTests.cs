using DateRecurrenceR.Recurrences;
using FluentAssertions;
using Xunit;

namespace DateRecurrenceR.Tests.Common;

/// <summary>
/// Contract tests for the invariants every <see cref="IRecurrence"/> implementation must hold:
/// the precomputed bounds match the enumerator, <c>Contains</c> agrees with the enumerator,
/// empty recurrences contain nothing, and sub-ranges contain only original occurrences.
/// Derive per recurrence type and supply the two factories.
/// </summary>
/// <typeparam name="TRecurrence">The recurrence type under test.</typeparam>
public abstract class RecurrenceContractTests<TRecurrence> where TRecurrence : struct, IRecurrence
{
    /// <summary>Creates a recurrence from a begin date and an occurrence count.</summary>
    protected abstract TRecurrence CreateByCount(DateOnly beginDate, int count);

    /// <summary>Creates a recurrence from a begin date and an end date.</summary>
    protected abstract TRecurrence CreateByEndDate(DateOnly beginDate, DateOnly endDate);

    /// <summary>The begin date used by the contract facts.</summary>
    protected virtual DateOnly BeginDate => new(2026, 1, 15);

    /// <summary>Days probed beyond both ends in the Contains agreement check.</summary>
    protected virtual int ContainsWindowDays => 45;

    [Fact]
    public void ByCount_bounds_and_count_match_the_enumerator()
    {
        var sut = CreateByCount(BeginDate, 10);

        var dates = Collector.Collect(sut);

        dates.Should().HaveCount(10);
        sut.Count.Should().Be(10);
        sut.StartDate.Should().Be(dates.First());
        sut.StopDate.Should().Be(dates.Last());
        dates.Should().BeInAscendingOrder();
    }

    [Fact]
    public void ByEndDate_bounds_and_count_match_the_enumerator()
    {
        var probe = CreateByCount(BeginDate, 8);
        var sut = CreateByEndDate(BeginDate, probe.StopDate);

        var dates = Collector.Collect(sut);

        dates.Should().Equal(Collector.Collect(probe));
        sut.Count.Should().Be(8);
        sut.StartDate.Should().Be(dates.First());
        sut.StopDate.Should().Be(dates.Last());
    }

    [Fact]
    public void Contains_agrees_with_the_enumerator_for_every_date_in_the_window()
    {
        var sut = CreateByCount(BeginDate, 6);

        var dates = Collector.Collect(sut);
        var expected = new HashSet<DateOnly>(dates);

        for (var date = dates.First().AddDays(-ContainsWindowDays);
             date <= dates.Last().AddDays(ContainsWindowDays);
             date = date.AddDays(1))
        {
            sut.Contains(date).Should().Be(expected.Contains(date),
                $"Contains({date:yyyy-MM-dd}) must agree with the enumerator");
        }
    }

    [Fact]
    public void Empty_recurrence_contains_nothing_and_stays_empty()
    {
        var sut = CreateByCount(BeginDate, 0);

        sut.Count.Should().Be(0);
        Collector.Collect(sut).Should().BeEmpty();
        sut.Contains(sut.StartDate).Should().BeFalse();
        sut.Contains(BeginDate).Should().BeFalse();
        Collector.Collect(sut.GetSubRange(3)).Should().BeEmpty();
        Collector.Collect(sut.GetSubRange(BeginDate, 3)).Should().BeEmpty();
    }

    [Fact]
    public void SubRange_by_count_is_a_prefix_of_the_recurrence()
    {
        var sut = CreateByCount(BeginDate, 8);

        var dates = Collector.Collect(sut);
        var subRange = sut.GetSubRange(3);

        Collector.Collect(subRange).Should().Equal(dates.Take(3));
    }

    [Fact]
    public void SubRange_from_date_yields_only_original_occurrences()
    {
        var sut = CreateByCount(BeginDate, 8);

        var dates = Collector.Collect(sut);
        var subRange = sut.GetSubRange(dates[2].AddDays(1), 3);

        Collector.Collect(subRange).Should().Equal(dates.Skip(3).Take(3));
    }
}
