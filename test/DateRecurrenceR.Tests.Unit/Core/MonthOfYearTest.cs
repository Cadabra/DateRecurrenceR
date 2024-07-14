using DateRecurrenceR.Core;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Core;

[TestSubject(typeof(MonthOfYear))]
public class MonthOfYearTest
{
    [Fact]
    public void Empty_ctor_creates_minValue()
    {
        // Arrange
        var sut = new MonthOfYear();

        // Act

        //Assert
        sut.Should().Be(MonthOfYear.MinValue);
    }
    
    [Fact]
    public void Ctor_creates_specified_value()
    {
        // Arrange
        const int someMonthOfYear = 6;
        var sut = new MonthOfYear(someMonthOfYear);

        // Act

        //Assert
        ((int)sut).Should().Be(someMonthOfYear);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void Ctor_throws_exception_if_value_outOfRange(int value)
    {
        // Arrange
        var sut = () => new MonthOfYear(value);

        // Act

        //Assert
        sut.Should().Throw<ArgumentOutOfRangeException>();
    }
}