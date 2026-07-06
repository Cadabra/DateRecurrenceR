using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using DateRecurrenceR.Internals;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Helpers;

[TestSubject(typeof(DateHelper))]
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
        Assert.True(result);
        Assert.Equal(DateOnly.MinValue, date);
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
        Assert.False(result);
        Assert.Equal(default, date);
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
        Assert.True(result);
        Assert.Equal(DateOnly.FromDayNumber(6), date);
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
        Assert.False(result);
        Assert.Equal(default, date);
    }

    [Fact]
    public void Method_CalculateDaysToNextInterval_with_singleDay()
    {
        // Arrange
        var interval = 50;
        var weekDays = new WeekDays(DayOfWeek.Monday);
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval, DayOfWeek.Sunday);

        // Act
        var res = DateHelper.CalculateDaysToNextInterval(
            DateOnly.MinValue.DayNumber,
            DateOnly.MinValue.AddDays(1).DayNumber,
            interval,
            patternHash);

        //Assert
        Assert.Equal(interval * DaysInWeek, res);
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
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval, DayOfWeek.Sunday);

        // Act
        var res = DateHelper.CalculateDaysToNextInterval(
            startDate.DayNumber,
            fromDate.DayNumber,
            interval,
            patternHash);

        //Assert
        Assert.Equal(expectedDate.DayNumber - startDate.DayNumber, res);
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
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval, DayOfWeek.Sunday);

        // Act
        var res = DateHelper.CalculateDaysToNextInterval(
            startDate.DayNumber,
            fromDate.DayNumber,
            interval,
            patternHash);

        //Assert
        Assert.Equal(expectedDate.DayNumber - startDate.DayNumber, res);
    }

    /// <summary>
    /// Drives the seven unrolled early-return branches of CalculateDaysToNextInterval by
    /// stepping fromDay across a full interval-week with an all-days pattern (each hop is one day).
    /// </summary>
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(4, 4)]
    [InlineData(5, 5)]
    [InlineData(6, 6)]
    [InlineData(7, 7)]
    public void CalculateDaysToNextInterval_returns_days_to_the_next_occurrence(int offset, int expected)
    {
        var allDays = new WeekDays(true, true, true, true, true, true, true);
        var hash = WeeklyRecurrenceHelper.GetPatternHash(allDays, 1, DayOfWeek.Monday);
        var start = new DateOnly(2026, 1, 5).DayNumber; // Monday

        DateHelper.CalculateDaysToNextInterval(start, start + offset, 1, hash).Should().Be(expected);
    }

    [Fact]
    public void CalculateDaysToNextInterval_with_sparse_pattern_skips_unselected_days()
    {
        // Monday and Thursday only; from Tuesday the next selected day is Thursday (2 days on).
        var weekDays = new WeekDays(DayOfWeek.Monday, DayOfWeek.Thursday);
        var hash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, 1, DayOfWeek.Monday);
        var start = new DateOnly(2026, 1, 5).DayNumber; // Monday

        DateHelper.CalculateDaysToNextInterval(start, start + 2, 1, hash).Should().Be(3); // Thursday
    }
}
