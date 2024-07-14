using DateRecurrenceR.Core;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Core;

[TestSubject(typeof(DayOfMonth))]
public class DayOfMonthTest
{
    [Fact]
    public void Empty_ctor_creates_minValue()
    {
        // Arrange
        var sut = new DayOfMonth();

        // Act

        //Assert
        sut.Should().Be(DayOfMonth.MinValue);
    }
    
    [Fact]
    public void Ctor_creates_specified_value()
    {
        // Arrange
        const int someDayOfMonth = 15;
        var sut = new DayOfMonth(someDayOfMonth);

        // Act

        //Assert
        ((int)sut).Should().Be(someDayOfMonth);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(32)]
    public void Ctor_throws_exception_if_value_outOfRange(int value)
    {
        // Arrange
        var sut = () => new DayOfMonth(value);

        // Act

        //Assert
        sut.Should().Throw<ArgumentOutOfRangeException>();
    }
}