using DateRecurrenceR.Helpers;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit.Helpers;

public sealed class DateHelperTests
{
    [Fact]
    public void TryGetDateOfDayOfWeek_ShouldReturn_CorrectValue()
    {
        // Arrange

        // Act
        var result = DateHelper.TryGetDateOfDayOfWeek(
            DateOnly.MinValue,
            DayOfWeek.Monday,
            DayOfWeek.Sunday,
            out var date);

        //Assert
        result.Should().BeTrue();
        date.Should().Be(DateOnly.MinValue);
    }

    [Fact]
    public void TryGetDateOfDayOfWeek_ShouldReturn_False_When_MinDate_And_FirstDayOfWeek_Is_Sunday()
    {
        // Arrange

        // Act
        var result = DateHelper.TryGetDateOfDayOfWeek(
            DateOnly.MinValue,
            DayOfWeek.Sunday,
            DayOfWeek.Sunday,
            out var date);

        //Assert
        result.Should().BeFalse();
        date.Should().Be(default);
    }

    [Fact]
    public void TryGetDateOfDayOfWeek_ShouldReturn_True_When_MinDate_And_FirstDayOfWeek_Is_Monday()
    {
        // Arrange

        // Act
        var result = DateHelper.TryGetDateOfDayOfWeek(
            DateOnly.MinValue,
            DayOfWeek.Sunday,
            DayOfWeek.Monday,
            out var date);

        //Assert
        result.Should().BeTrue();
        date.Should().Be(DateOnly.FromDayNumber(6));
    }

    [Fact]
    public void TryGetDateOfDayOfWeek_ShouldReturn_True_When_MaxDate()
    {
        // Arrange

        // Act
        var result = DateHelper.TryGetDateOfDayOfWeek(
            DateOnly.MaxValue,
            DayOfWeek.Saturday,
            DayOfWeek.Sunday,
            out var date);

        //Assert
        result.Should().BeFalse();
        date.Should().Be(default);
    }


    [Fact]
    public void Method_GetDateByDayOfMonth()
    {
        // Arrange
        const int year = 1;
        const int month = 1;
        const int dayOfMonth = 1;

        // Act
        var date = DateHelper.GetDateByDayOfMonth(year, month, dayOfMonth);

        //Assert
        date.Should().Be(DateOnly.MinValue);
    }

    [Fact]
    public void Method_GetDateByDayOfMonth_by_FirstWeek()
    {
        // Arrange
        const int year = 1;
        const int month = 1;
        const DayOfWeek dayOfWeek = DayOfWeek.Saturday;
        const NumberOfWeek numberOfWeek = NumberOfWeek.First;
        var expectedDate = new DateOnly(1, 1, 6);

        // Act
        var date = DateHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);

        //Assert
        date.Should().Be(expectedDate);
    }

    [Fact]
    public void Method_GetDateByDayOfMonth_by_SecondWeek()
    {
        // Arrange
        const int year = 1;
        const int month = 1;
        const DayOfWeek dayOfWeek = DayOfWeek.Monday;
        const NumberOfWeek numberOfWeek = NumberOfWeek.Second;
        var expectedDate = new DateOnly(1, 1, 8);

        // Act
        var date = DateHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);

        //Assert
        date.Should().Be(expectedDate);
    }

    [Fact]
    public void Method_GetDateByDayOfMonth_by_ThirdWeek()
    {
        // Arrange
        const int year = 1;
        const int month = 1;
        const DayOfWeek dayOfWeek = DayOfWeek.Monday;
        const NumberOfWeek numberOfWeek = NumberOfWeek.Third;
        var expectedDate = new DateOnly(1, 1, 15);

        // Act
        var date = DateHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);

        //Assert
        date.Should().Be(expectedDate);
    }

    [Fact]
    public void Method_GetDateByDayOfMonth_by_FourthWeek()
    {
        // Arrange
        const int year = 1;
        const int month = 1;
        const DayOfWeek dayOfWeek = DayOfWeek.Monday;
        const NumberOfWeek numberOfWeek = NumberOfWeek.Fourth;
        var expectedDate = new DateOnly(1, 1, 22);

        // Act
        var date = DateHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);

        //Assert
        date.Should().Be(expectedDate);
    }

    [Fact]
    public void Method_GetDateByDayOfMonth_by_LastWeek()
    {
        // Arrange
        const int year = 1;
        const int month = 1;
        const DayOfWeek dayOfWeek = DayOfWeek.Monday;
        const NumberOfWeek numberOfWeek = NumberOfWeek.Last;
        var expectedDate = new DateOnly(1, 1, 29);

        // Act
        var date = DateHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);

        //Assert
        date.Should().Be(expectedDate);
    }

    [Fact]
    public void Method_GetDateByDayOfMonth_throws_Exception_when_NumberOfWeek_is_outOfRange()
    {
        // Arrange
        const int year = 1;
        const int month = 1;
        const DayOfWeek dayOfWeek = DayOfWeek.Monday;
        const NumberOfWeek numberOfWeek = (NumberOfWeek) 5;

        // Act
        var action = () => DateHelper.GetDateByDayOfMonth(year, month, dayOfWeek, numberOfWeek);

        //Assert
        action.Should().Throw<Exception>();
    }


    [Fact]
    public void Method_GetDateByDayOfYear()
    {
        // Arrange
        const int year = 1;
        const int dayOfYear = 1;
        var expectedDate = new DateOnly(1, 1, 1);

        // Act
        var date = DateHelper.GetDateByDayOfYear(year, dayOfYear);

        //Assert
        date.Should().Be(expectedDate);
    }

    [Fact]
    public void Method_GetDateByDayOfYear_returns_max_365()
    {
        // Arrange
        const int year = 1;
        const int dayOfYear = 365;
        var expectedDate = new DateOnly(year, 12, 31);

        // Act
        var date = DateHelper.GetDateByDayOfYear(year, dayOfYear);

        //Assert
        date.Should().Be(expectedDate);
    }

    [Fact]
    public void Method_GetDateByDayOfYear_returns_max_366()
    {
        // Arrange
        const int year = 4;
        const int dayOfYear = 366;
        var expectedDate = new DateOnly(year, 12, 31);

        // Act
        var date = DateHelper.GetDateByDayOfYear(year, dayOfYear);

        //Assert
        date.Should().Be(expectedDate);
    }


    [Fact]
    public void Method_CalculateDaysToNextInterval_with_singleDay()
    {
        // Arrange
        var interval = 50;
        var weekDays = new WeekDays(DayOfWeek.Monday);
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval);

        // Act
        var res = DateHelper.CalculateDaysToNextInterval(
            DateOnly.MinValue.DayNumber,
            DateOnly.MinValue.AddDays(1).DayNumber,
            interval,
            patternHash);

        //Assert
        res.Should().Be(interval * DaysInWeek);
    }

    [Fact]
    public void Method_CalculateDaysToNextInterval_with_multipleDays_with_start_after_lastDay()
    {
        // Arrange
        var interval = 2;
        var weekDays = new WeekDays(DayOfWeek.Tuesday, DayOfWeek.Friday);
        var startDate = new DateOnly(999, 1, 1);
        var fromDate = new DateOnly(999, 2, 16); // right after 999-02-15 Friday
        var expectedDate = new DateOnly(999, 2, 26);
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval);

        // Act
        var res = DateHelper.CalculateDaysToNextInterval(
            startDate.DayNumber,
            fromDate.DayNumber,
            interval,
            patternHash);

        //Assert
        res.Should().Be(expectedDate.DayNumber - startDate.DayNumber);
    }

    [Fact]
    public void Method_CalculateDaysToNextInterval_with_multipleDays_with_start_in_middleDay()
    {
        // Arrange
        var interval = 2;
        var weekDays = new WeekDays(DayOfWeek.Tuesday, DayOfWeek.Friday);
        var startDate = new DateOnly(999, 1, 1);
        var fromDate = new DateOnly(999, 2, 14); // before 999-02-15 Friday
        var expectedDate = new DateOnly(999, 2, 15);
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval);

        // Act
        var res = DateHelper.CalculateDaysToNextInterval(
            startDate.DayNumber,
            fromDate.DayNumber,
            interval,
            patternHash);

        //Assert
        res.Should().Be(expectedDate.DayNumber - startDate.DayNumber);
    }
}