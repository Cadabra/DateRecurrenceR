using DateRecurrenceR.Core;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Core;

[TestSubject(typeof(WeekDays))]
public class WeekDaysTests
{
    [Theory]
    [InlineData(DayOfWeek.Sunday, 1 << 0)]
    [InlineData(DayOfWeek.Monday, 1 << 1)]
    [InlineData(DayOfWeek.Tuesday, 1 << 2)]
    [InlineData(DayOfWeek.Wednesday, 1 << 3)]
    [InlineData(DayOfWeek.Thursday, 1 << 4)]
    [InlineData(DayOfWeek.Friday, 1 << 5)]
    [InlineData(DayOfWeek.Saturday, 1 << 6)]
    public void ToFlag_returns_correct_value_for_each_day(DayOfWeek dayOfWeek, int expectedFlag)
    {
        var sut = new WeekDays(dayOfWeek);

        sut.ToFlag().Should().Be(expectedFlag);
    }

    [Fact]
    public void ToFlag_returns_correct_value_for_all_days()
    {
        var sut = new WeekDays(true, true, true, true, true, true, true);

        sut.ToFlag().Should().Be(127);
    }

    [Fact]
    public void DayOfWeek_constructors_select_the_given_days()
    {
        var sut = new WeekDays(DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday);

        sut[DayOfWeek.Monday].Should().BeTrue();
        sut[DayOfWeek.Wednesday].Should().BeTrue();
        sut[DayOfWeek.Friday].Should().BeTrue();
        sut[DayOfWeek.Sunday].Should().BeFalse();
        sut[DayOfWeek.Tuesday].Should().BeFalse();
        sut[DayOfWeek.Thursday].Should().BeFalse();
        sut[DayOfWeek.Saturday].Should().BeFalse();
        sut.CountOfSelected.Should().Be(3);
    }

    [Fact]
    public void Duplicate_days_in_the_constructor_are_counted_once()
    {
        var sut = new WeekDays(DayOfWeek.Monday, DayOfWeek.Monday);

        sut.CountOfSelected.Should().Be(1);
    }

    [Fact]
    public void Bool_constructor_matches_DayOfWeek_constructor()
    {
        var byBools = new WeekDays(false, true, false, false, false, true, false);
        var byDays = new WeekDays(DayOfWeek.Monday, DayOfWeek.Friday);

        byBools.Should().Be(byDays);
    }

    [Theory]
    [InlineData(6, 7)]
    [InlineData(3, 4)]
    [InlineData(-1, 0)]
    public void GetCountOfSelected_counts_days_up_to_max_index_for_all_days(int maxDayIndex, int expected)
    {
        var sut = new WeekDays(true, true, true, true, true, true, true);

        sut.GetCountOfSelected(maxDayIndex, DayOfWeek.Sunday).Should().Be(expected);
    }

    [Fact]
    public void GetCountOfSelected_counts_only_selected_days_shifted_by_first_day()
    {
        var monFri = new WeekDays(DayOfWeek.Monday, DayOfWeek.Friday);
        // from Monday, Friday has index 4
        monFri.GetCountOfSelected(3, DayOfWeek.Monday).Should().Be(1);
        monFri.GetCountOfSelected(4, DayOfWeek.Monday).Should().Be(2);

        var tuesday = new WeekDays(DayOfWeek.Tuesday);
        tuesday.GetCountOfSelected(6, DayOfWeek.Monday).Should().Be(1);
    }

    [Fact]
    public void TryGet_returns_the_day_at_the_selected_index_and_false_past_the_end()
    {
        var sut = new WeekDays(true, true, true, true, true, true, true);

        sut.TryGet(6, DayOfWeek.Sunday, out var last).Should().BeTrue();
        last.Should().Be(DayOfWeek.Saturday);

        sut.TryGet(7, DayOfWeek.Sunday, out _).Should().BeFalse();
    }

    [Theory]
    [InlineData(DayOfWeek.Monday, DayOfWeek.Thursday)]
    [InlineData(DayOfWeek.Monday, DayOfWeek.Friday)]
    public void GetMinByFirstDayOfWeek_returns_first_selected_day_from_first_day(DayOfWeek firstDay, DayOfWeek expected)
    {
        var sut = new WeekDays(expected);

        sut.GetMinByFirstDayOfWeek(firstDay).Should().Be(expected);
    }

    /// <summary>
    /// Regression: <see cref="WeekDays"/> did not override <c>Equals</c>/<c>GetHashCode</c>, so the
    /// built-in <c>ValueType</c> implementation threw <see cref="NotSupportedException"/> because the
    /// backing storage is an <c>[InlineArray]</c> struct.
    /// </summary>
    [Fact]
    public void Equality_members_agree()
    {
        var a = new WeekDays(DayOfWeek.Monday, DayOfWeek.Friday);
        var b = new WeekDays(DayOfWeek.Friday, DayOfWeek.Monday);
        var c = new WeekDays(DayOfWeek.Tuesday);

        (a == b).Should().BeTrue();
        (a != b).Should().BeFalse();
        (a == c).Should().BeFalse();
        (a != c).Should().BeTrue();

        a.Equals(b).Should().BeTrue();
        a.Equals(c).Should().BeFalse();
        a.Equals((object)b).Should().BeTrue();
        a.Equals(null).Should().BeFalse();
        a.Equals("not week days").Should().BeFalse();

        a.GetHashCode().Should().Be(b.GetHashCode());
    }
}
