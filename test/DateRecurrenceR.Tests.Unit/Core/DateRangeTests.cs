using DateRecurrenceR.Core;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Core;

[TestSubject(typeof(DateRange))]
public class DateRangeTests
{
    [Fact]
    public void Begin_only_constructor_creates_an_open_ended_range()
    {
        var sut = new DateRange(new DateOnly(2026, 1, 1));

        sut.BeginDate.Should().Be(new DateOnly(2026, 1, 1));
        sut.EndDate.Should().Be(DateOnly.MaxValue);
        sut.Count.Should().BeNull();
    }

    [Fact]
    public void End_date_constructor_stores_both_dates()
    {
        var sut = new DateRange(new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31));

        sut.BeginDate.Should().Be(new DateOnly(2026, 1, 1));
        sut.EndDate.Should().Be(new DateOnly(2026, 12, 31));
        sut.Count.Should().BeNull();
    }

    [Fact]
    public void Count_constructor_stores_the_count_and_no_end_date()
    {
        var sut = new DateRange(new DateOnly(2026, 1, 1), 10);

        sut.BeginDate.Should().Be(new DateOnly(2026, 1, 1));
        sut.EndDate.Should().BeNull();
        sut.Count.Should().Be(10);
    }

    [Fact]
    public void Count_constructor_accepts_zero()
    {
        var sut = new DateRange(new DateOnly(2026, 1, 1), 0);

        sut.Count.Should().Be(0);
    }

    [Fact]
    public void Count_constructor_throws_for_negative_count()
    {
        var act = () => new DateRange(new DateOnly(2026, 1, 1), -1);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
