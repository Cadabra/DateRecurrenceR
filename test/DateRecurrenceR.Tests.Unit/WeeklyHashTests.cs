using DateRecurrenceR.Internals;
using FluentAssertions;

namespace Cadabra.CS.Date.Tests.Unit;

public sealed class WeeklyHashTests
{
    [Fact]
    public void WeeklyHash_Should_Return_Default_Values()
    {
        // Arrange
        var sut = new WeeklyHash();

        // Act

        // Assert
        sut[DayOfWeek.Sunday].Should().Be(0);
        sut[DayOfWeek.Monday].Should().Be(0);
        sut[DayOfWeek.Tuesday].Should().Be(0);
        sut[DayOfWeek.Wednesday].Should().Be(0);
        sut[DayOfWeek.Thursday].Should().Be(0);
        sut[DayOfWeek.Friday].Should().Be(0);
        sut[DayOfWeek.Saturday].Should().Be(0);
    }

    [Fact]
    public void WeeklyHash_Should_Set_Correct_Values()
    {
        // Arrange
        var sut = new WeeklyHash
        {
            // Act
            [DayOfWeek.Sunday] = 1,
            [DayOfWeek.Monday] = 2,
            [DayOfWeek.Tuesday] = 3,
            [DayOfWeek.Wednesday] = 4,
            [DayOfWeek.Thursday] = 5,
            [DayOfWeek.Friday] = 6,
            [DayOfWeek.Saturday] = 7
        };

        // Act

        // Assert
        sut[DayOfWeek.Sunday].Should().Be(1);
        sut[DayOfWeek.Monday].Should().Be(2);
        sut[DayOfWeek.Tuesday].Should().Be(3);
        sut[DayOfWeek.Wednesday].Should().Be(4);
        sut[DayOfWeek.Thursday].Should().Be(5);
        sut[DayOfWeek.Friday].Should().Be(6);
        sut[DayOfWeek.Saturday].Should().Be(7);
    }

    [Fact]
    public void WeeklyHash_ShouldThrowsException_When_Get_Value()
    {
        // Arrange
        var sut = new WeeklyHash();

        // Act
        var action = () => sut[(DayOfWeek) 7];

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void WeeklyHash_ShouldThrowsException_When_Set_Value()
    {
        // Arrange
        var sut = new WeeklyHash();

        // Act
        var action = () => sut[(DayOfWeek) 7] = 0;

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }
}