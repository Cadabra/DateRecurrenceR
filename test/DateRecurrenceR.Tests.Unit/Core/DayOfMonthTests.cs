using DateRecurrenceR.Core;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Core;

[TestSubject(typeof(DayOfMonth))]
public class DayOfMonthTests : Int32BasedContractTests<DayOfMonth>
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

    protected override int MinAllowed => 1;

    protected override int MaxAllowed => 31;

    protected override DayOfMonth Create(int value)
    {
        return new DayOfMonth(value);
    }

    protected override int ToInt32(DayOfMonth value)
    {
        return value;
    }
}
