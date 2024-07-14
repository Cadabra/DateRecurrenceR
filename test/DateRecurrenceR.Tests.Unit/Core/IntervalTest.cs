using DateRecurrenceR.Core;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Core;

[TestSubject(typeof(Interval))]
public class IntervalTest
{
    [Fact]
    public void Empty_ctor_creates_minValue()
    {
        // Arrange
        var sut = new Interval();

        // Act

        //Assert
        sut.Should().Be(Interval.MinValue);
    }
    
    [Fact]
    public void Ctor_creates_specified_value()
    {
        // Arrange
        const int someInterval = 1_073_741_823;
        var sut = new Interval(someInterval);

        // Act

        //Assert
        ((int)sut).Should().Be(someInterval);
    }

    [Fact]
    public void Ctor_throws_exception_if_value_outOfRange()
    {
        // Arrange
        const int invalidIntervalValue = 0;
        var sut = () => new Interval(invalidIntervalValue);

        // Act

        //Assert
        sut.Should().Throw<ArgumentOutOfRangeException>();
    }
}