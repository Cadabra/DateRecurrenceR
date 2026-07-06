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
    public void Indexer_stores_and_returns_the_value(int index)
    {
        var sut = new WeekDaysArray { [index] = true };

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
        var sut = new WeekDaysArray
        {
            [0] = true, [1] = true, [2] = true, [3] = true, [4] = true, [5] = true, [6] = true
        };

        sut.GetCountOfSelected().Should().Be(7);
    }
}
