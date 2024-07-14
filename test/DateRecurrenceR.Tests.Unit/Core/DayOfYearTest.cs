using DateRecurrenceR.Core;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Core;

[TestSubject(typeof(DayOfYear))]
public class DayOfYearTest
{
    [Fact]
    public void Empty_ctor_creates_minValue()
    {
        // Arrange
        var sut = new DayOfYear();

        // Act

        //Assert
        sut.Should().Be(DayOfYear.MinValue);
    }
    
    [Fact]
    public void Ctor_creates_specified_value()
    {
        // Arrange
        const int someDayOfYear = 183;
        var sut = new DayOfYear(someDayOfYear);

        // Act

        //Assert
        ((int)sut).Should().Be(someDayOfYear);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(367)]
    public void Ctor_throws_exception_if_value_outOfRange(int value)
    {
        // Arrange
        var sut = () => new DayOfYear(value);

        // Act

        //Assert
        sut.Should().Throw<ArgumentOutOfRangeException>();
    }
}