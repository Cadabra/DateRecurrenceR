using DateRecurrenceR.Core;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Core;

[TestSubject(typeof(DayOfYear))]
public class DayOfYearTests : Int32BasedContractTests<DayOfYear>
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

    protected override int MinAllowed => 1;

    protected override int MaxAllowed => 366;

    protected override DayOfYear Create(int value)
    {
        return new DayOfYear(value);
    }

    protected override int ToInt32(DayOfYear value)
    {
        return value;
    }
}
