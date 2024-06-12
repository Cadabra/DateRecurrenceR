using DateRecurrenceR.Helpers;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit.Helpers;

public class MonthlyRecurrenceHelperTests
{
    [Fact]
    public void Method_TryGetStartDate_returns_True()
    {
        // Arrange
        var beginDate = DateOnly.MinValue;
        var fromDate = DateOnly.MinValue;
        const int dayOfMonth = 1;
        const int interval = 1;

        // Act
        var canStart = MonthlyRecurrenceHelper
            .TryGetStartDate(beginDate, fromDate, dayOfMonth, interval, out _);

        //Assert
        canStart.Should().BeTrue();
    }

    [Fact]
    public void Method_TryGetStartDate_returns_False_when_FromDate_eq_BeginDate_and_result_is_outOfRange()
    {
        // Arrange
        var beginDate = DateOnly.MaxValue;
        var fromDate = DateOnly.MaxValue;
        const int dayOfMonth = 1;
        const int interval = 1;

        // Act
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfMonth,
            interval,
            out _);

        //Assert
        canStart.Should().BeFalse();
    }

    [Fact]
    public void Method_TryGetStartDate_returns_False_when_FromDate_gt_BeginDate_and_result_is_outOfRange()
    {
        // Arrange
        var beginDate = new DateOnly(9999, 12, 1);
        var fromDate = new DateOnly(9999, 12, 2);
        const int dayOfMonth = 1;
        const int interval = 1;

        // Act
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            dayOfMonth,
            interval,
            out _);

        //Assert
        canStart.Should().BeFalse();
    }
}