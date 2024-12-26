using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit.Helpers;

public sealed class WeeklyRecurrenceHelperTests
{
    [Fact]
    public void Method_TryGetStartDate_returns_True_when_FromDate_eq_BeginDate()
    {
        // Arrange
        var beginDate = DateOnly.MinValue;
        var fromDate = DateOnly.MinValue;
        var weekDays = new WeekDays(DayOfWeek.Monday);
        var firstDayOfWeek = DayOfWeek.Monday;
        var interval = 1;
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval, DayOfWeek.Sunday);

        // Act
        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            patternHash,
            weekDays,
            firstDayOfWeek,
            interval,
            out _);

        //Assert
        canStart.Should().BeTrue();
    }

    [Fact]
    public void Method_TryGetStartDate_returns_True_when_FromDate_gt_BeginDate()
    {
        // Arrange
        var beginDate = DateOnly.MinValue;
        var fromDate = new DateOnly(1, 1, 5);
        var weekDays = new WeekDays(DayOfWeek.Monday, DayOfWeek.Friday);
        var firstDayOfWeek = DayOfWeek.Monday;
        var interval = 1;
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval, DayOfWeek.Sunday);

        // Act
        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            patternHash,
            weekDays,
            firstDayOfWeek,
            interval,
            out _);

        //Assert
        canStart.Should().BeTrue();
    }

    [Fact]
    public void Method_TryGetStartDate_returns_False_when_FromDate_eq_BeginDate_and_result_is_outOfRange()
    {
        // Arrange
        var beginDate = DateOnly.MaxValue;
        var fromDate = DateOnly.MaxValue;
        var weekDays = new WeekDays(DayOfWeek.Saturday);
        var firstDayOfWeek = DayOfWeek.Monday;
        var interval = 1;
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval, DayOfWeek.Sunday);

        // Act
        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            patternHash,
            weekDays,
            firstDayOfWeek,
            interval,
            out _);

        //Assert
        canStart.Should().BeFalse();
    }

    [Fact]
    public void Method_TryGetStartDate_returns_False_when_FromDate_gt_BeginDate_and_result_is_outOfRange()
    {
        // Arrange
        var beginDate = new DateOnly(9999, 12, 27);
        var fromDate = new DateOnly(9999, 12, 28);
        var weekDays = new WeekDays(DayOfWeek.Monday, DayOfWeek.Saturday);
        var firstDayOfWeek = DayOfWeek.Monday;
        var interval = 1;
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval, DayOfWeek.Sunday);

        // Act
        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            patternHash,
            weekDays,
            firstDayOfWeek,
            interval,
            out _);

        //Assert
        canStart.Should().BeFalse();
    }

    [Fact]
    public void Method_TryGetStartDate_returns_Result_eq_FromDate()
    {
        // Arrange
        const int interval = 1;
        var beginDate = new DateOnly(1, 1, 1);
        var fromDate = beginDate.AddDays(7 * interval * 2);
        var weekDays = new WeekDays(DayOfWeek.Monday);
        var firstDayOfWeek = DayOfWeek.Monday;
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval, DayOfWeek.Sunday);

        // Act
        WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            patternHash,
            weekDays,
            firstDayOfWeek,
            interval,
            out var startDate);

        //Assert
        startDate.Should().Be(fromDate);
    }

    [Fact]
    public void Method_TryGetStartDate_returns_Result_eq_FromDate_2()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 8);
        var fromDate = beginDate;
        var firstDayOfWeek = DayOfWeek.Monday;
        var weekDays = new WeekDays(DayOfWeek.Sunday, DayOfWeek.Monday);
        var interval = 1;
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, interval, DayOfWeek.Sunday);

        // Act
        WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            patternHash,
            weekDays,
            firstDayOfWeek,
            interval,
            out var startDate);

        //Assert
        startDate.Should().Be(fromDate);
    }

    [Fact]
    public void Method_GetPatternHash_returns_correct_hash_for_Sunday_and_Monday_with_Sunday_as_FirstDayOfWeek()
    {
        // Arrange
        var weekDays = new WeekDays(DayOfWeek.Sunday, DayOfWeek.Monday);

        // Act
        var hash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, 2, DayOfWeek.Sunday);

        //Assert
        hash[DayOfWeek.Sunday].Should().Be(1);
        hash[DayOfWeek.Monday].Should().Be(13);
        hash[DayOfWeek.Tuesday].Should().Be(0);
        hash[DayOfWeek.Wednesday].Should().Be(0);
        hash[DayOfWeek.Thursday].Should().Be(0);
        hash[DayOfWeek.Friday].Should().Be(0);
        hash[DayOfWeek.Saturday].Should().Be(0);
    }

    [Fact]
    public void Method_GetPatternHash_returns_correct_hash_for_Sunday_and_Monday_with_Monday_as_FirstDayOfWeek()
    {
        // Arrange
        var weekDays = new WeekDays(DayOfWeek.Sunday, DayOfWeek.Monday);

        // Act
        var hash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, 2, DayOfWeek.Monday);

        //Assert
        hash[DayOfWeek.Sunday].Should().Be(8);
        hash[DayOfWeek.Monday].Should().Be(6);
        hash[DayOfWeek.Tuesday].Should().Be(0);
        hash[DayOfWeek.Wednesday].Should().Be(0);
        hash[DayOfWeek.Thursday].Should().Be(0);
        hash[DayOfWeek.Friday].Should().Be(0);
        hash[DayOfWeek.Saturday].Should().Be(0);
    }

    [Fact]
    public void Method_GetPatternHash_returns_correct_hash_for_one_day()
    {
        // Arrange
        var weekDays = new WeekDays(DayOfWeek.Monday);

        // Act
        var hash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, 1, DayOfWeek.Sunday);

        //Assert
        hash[DayOfWeek.Sunday].Should().Be(0);
        hash[DayOfWeek.Monday].Should().Be(7);
        hash[DayOfWeek.Tuesday].Should().Be(0);
        hash[DayOfWeek.Wednesday].Should().Be(0);
        hash[DayOfWeek.Thursday].Should().Be(0);
        hash[DayOfWeek.Friday].Should().Be(0);
        hash[DayOfWeek.Saturday].Should().Be(0);
    }

    [Fact]
    public void Method_GetPatternHash_returns_correct_hash_for_two_days()
    {
        // Arrange
        var weekDays = new WeekDays(DayOfWeek.Tuesday, DayOfWeek.Friday);

        // Act
        var hash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, 1, DayOfWeek.Sunday);

        //Assert
        hash[DayOfWeek.Sunday].Should().Be(0);
        hash[DayOfWeek.Monday].Should().Be(0);
        hash[DayOfWeek.Tuesday].Should().Be(3);
        hash[DayOfWeek.Wednesday].Should().Be(0);
        hash[DayOfWeek.Thursday].Should().Be(0);
        hash[DayOfWeek.Friday].Should().Be(4);
        hash[DayOfWeek.Saturday].Should().Be(0);
    }

    [Fact]
    public void Method_GetPatternHash_returns_correct_hash_for_seven_days()
    {
        // Arrange
        var weekDays = new WeekDays(
            DayOfWeek.Sunday,
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday
        );

        // Act
        var hash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, 1, DayOfWeek.Sunday);

        //Assert
        hash[DayOfWeek.Sunday].Should().Be(1);
        hash[DayOfWeek.Monday].Should().Be(1);
        hash[DayOfWeek.Tuesday].Should().Be(1);
        hash[DayOfWeek.Wednesday].Should().Be(1);
        hash[DayOfWeek.Thursday].Should().Be(1);
        hash[DayOfWeek.Friday].Should().Be(1);
        hash[DayOfWeek.Saturday].Should().Be(1);
    }

    [Fact]
    public void Method_GetPatternHash_returns_correct_hash_for_seven_days_2()
    {
        // Arrange
        var weekDays = new WeekDays(true,
            true,
            true,
            true,
            true,
            true,
            true);

        // Act
        var hash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, 1, DayOfWeek.Sunday);

        //Assert
        hash[DayOfWeek.Sunday].Should().Be(1);
        hash[DayOfWeek.Monday].Should().Be(1);
        hash[DayOfWeek.Tuesday].Should().Be(1);
        hash[DayOfWeek.Wednesday].Should().Be(1);
        hash[DayOfWeek.Thursday].Should().Be(1);
        hash[DayOfWeek.Friday].Should().Be(1);
        hash[DayOfWeek.Saturday].Should().Be(1);
    }

#if NET8_0_OR_GREATER
    [Fact]
    public void Method_8_GetPatternHash_returns_correct_hash_for_seven_days()
    {
        // Arrange
        var weekDaysArray = new WeekDaysArray();
        weekDaysArray[0] = true;
        weekDaysArray[1] = true;
        weekDaysArray[2] = true;
        weekDaysArray[3] = true;
        weekDaysArray[4] = true;
        weekDaysArray[5] = true;
        weekDaysArray[6] = true;
        var weekDays = new WeekDays(weekDaysArray);

        // Act
        var hash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, 1, DayOfWeek.Sunday);

        //Assert
        hash[DayOfWeek.Sunday].Should().Be(1);
        hash[DayOfWeek.Monday].Should().Be(1);
        hash[DayOfWeek.Tuesday].Should().Be(1);
        hash[DayOfWeek.Wednesday].Should().Be(1);
        hash[DayOfWeek.Thursday].Should().Be(1);
        hash[DayOfWeek.Friday].Should().Be(1);
        hash[DayOfWeek.Saturday].Should().Be(1);
    }
#endif
}
