using DateRecurrenceR.Core;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Core;

[TestSubject(typeof(WeekDaysArray))]
public class WeekDaysArrayTests
{
    [Fact]
    public void Length_is_seven()
    {
        WeekDaysArray.Length.Should().Be(7);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public void Element_access_stores_and_returns_the_value(int index)
    {
        var sut = new WeekDaysArray();
        sut[index] = true;

        sut[index].Should().BeTrue();
        sut.GetCountOfSelected().Should().Be(1);
    }

    [Fact]
    public void Default_instance_has_no_selected_days()
    {
        var sut = default(WeekDaysArray);

        sut.GetCountOfSelected().Should().Be(0);
    }

    [Fact]
    public void GetCountOfSelected_counts_all_days()
    {
        var sut = new WeekDaysArray();
        for (var i = 0; i < WeekDaysArray.Length; i++) sut[i] = true;

        sut.GetCountOfSelected().Should().Be(7);
    }
}
