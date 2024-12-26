using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Helpers;

[TestSubject(typeof(DateOnlyHelper))]
public class DateOnlyHelperTest
{
    [Fact]
    public void Method_GetDateByDayOfMonth()
    {
        // Arrange
        const int year = 1;
        const int month = 1;
        const int dayOfMonth = 1;

        // Act
        var date = DateOnlyHelper.GetDateByDayOfMonth(year, month, dayOfMonth);

        //Assert
        Assert.Equal(DateOnly.MinValue, date);
    }

    [Fact]
    public void Method_GetDateByDayOfMonth_by_FirstWeek()
    {
        // Arrange
        const int year = 1;
        const int month = 1;
        const DayOfWeek dayOfWeek = DayOfWeek.Saturday;
        const IndexOfDay numberOfWeek = IndexOfDay.First;
        var expectedDate = new DateOnly(1, 1, 6);

        // Act
        var date = DateOnlyHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);

        //Assert
        Assert.Equal(expectedDate, date);
    }

    [Fact]
    public void Method_GetDateByDayOfMonth_by_SecondWeek()
    {
        // Arrange
        const int year = 1;
        const int month = 1;
        const DayOfWeek dayOfWeek = DayOfWeek.Monday;
        const IndexOfDay numberOfWeek = IndexOfDay.Second;
        var expectedDate = new DateOnly(1, 1, 8);

        // Act
        var date = DateOnlyHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);

        //Assert
        Assert.Equal(expectedDate, date);
    }

    [Fact]
    public void Method_GetDateByDayOfMonth_by_ThirdWeek()
    {
        // Arrange
        const int year = 1;
        const int month = 1;
        const DayOfWeek dayOfWeek = DayOfWeek.Monday;
        const IndexOfDay numberOfWeek = IndexOfDay.Third;
        var expectedDate = new DateOnly(1, 1, 15);

        // Act
        var date = DateOnlyHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);

        //Assert
        Assert.Equal(expectedDate, date);
    }

    [Fact]
    public void Method_GetDateByDayOfMonth_by_FourthWeek()
    {
        // Arrange
        const int year = 1;
        const int month = 1;
        const DayOfWeek dayOfWeek = DayOfWeek.Monday;
        const IndexOfDay numberOfWeek = IndexOfDay.Fourth;
        var expectedDate = new DateOnly(1, 1, 22);

        // Act
        var date = DateOnlyHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);

        //Assert
        Assert.Equal(expectedDate, date);
    }

    [Fact]
    public void Method_GetDateByDayOfMonth_by_LastWeek()
    {
        // Arrange
        const int year = 1;
        const int month = 1;
        const DayOfWeek dayOfWeek = DayOfWeek.Monday;
        const IndexOfDay numberOfWeek = IndexOfDay.Last;
        var expectedDate = new DateOnly(1, 1, 29);

        // Act
        var date = DateOnlyHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);

        //Assert
        Assert.Equal(expectedDate, date);
    }

    [Fact]
    public void Method_GetDateByDayOfMonth_throws_Exception_when_NumberOfWeek_is_outOfRange()
    {
        // Arrange
        const int year = 1;
        const int month = 1;
        const DayOfWeek dayOfWeek = DayOfWeek.Monday;
        const IndexOfDay numberOfWeek = (IndexOfDay)5;

        // Act
        var action = () => { _ = DateOnlyHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek); };

        //Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }


    [Fact]
    public void Method_GetDateByDayOfYear()
    {
        // Arrange
        const int year = 1;
        const int dayOfYear = 1;
        var expectedDate = new DateOnly(1, 1, 1);

        // Act
        var date = DateOnlyHelper.GetDateByDayOfYear(year, dayOfYear);

        //Assert
        Assert.Equal(expectedDate, date);
    }

    [Fact]
    public void Method_GetDateByDayOfYear_returns_max_365()
    {
        // Arrange
        const int year = 1;
        const int dayOfYear = 365;
        var expectedDate = new DateOnly(year, 12, 31);

        // Act
        var date = DateOnlyHelper.GetDateByDayOfYear(year, dayOfYear);

        //Assert
        Assert.Equal(expectedDate, date);
    }

    [Fact]
    public void Method_GetDateByDayOfYear_returns_max_366()
    {
        // Arrange
        const int year = 4;
        const int dayOfYear = 366;
        var expectedDate = new DateOnly(year, 12, 31);

        // Act
        var date = DateOnlyHelper.GetDateByDayOfYear(year, dayOfYear);

        //Assert
        Assert.Equal(expectedDate, date);
    }
}