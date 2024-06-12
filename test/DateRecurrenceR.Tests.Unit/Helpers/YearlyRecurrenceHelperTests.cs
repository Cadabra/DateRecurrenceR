using DateRecurrenceR.Helpers;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit.Helpers;

public sealed class YearlyRecurrenceHelperTests
{
    [Fact]
    public void Method_TryGetStartDate_returns_True()
    {
        // Arrange
        var beginDate = DateOnly.MinValue;
        var fromDate = DateOnly.MinValue;
        var dayOfYear = 1;
        var interval = 1;

        // Act
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfYear,
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
        var dayOfYear = 1;
        var interval = 1;

        // Act
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfYear,
            interval,
            out _);

        //Assert
        canStart.Should().BeFalse();
    }

    [Fact]
    public void Method_TryGetStartDate_returns_False_when_FromDate_gt_BeginDate_and_result_is_outOfRange()
    {
        // Arrange
        var beginDate = new DateOnly(9999, 1, 1);
        var fromDate = new DateOnly(9999, 1, 2);
        var dayOfYear = 1;
        var interval = 1;

        // Act
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfYear,
            interval,
            out _);

        //Assert
        canStart.Should().BeFalse();
    }
}