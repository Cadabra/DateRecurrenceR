using DateRecurrenceR.Helpers;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit.Helpers;

public sealed class DailyRecurrenceHelperTests
{
    [Fact]
    public void Method_TryGetStartDate_returns_True_when_FromDate_eq_BeginDate()
    {
        // Arrange
        var beginDate = DateOnly.MinValue;
        var fromDate = DateOnly.MinValue;
        const int interval = 1;

        // Act
        var canStart = DailyRecurrenceHelper
            .TryGetStartDate(beginDate, fromDate, interval, out _);

        //Assert
        canStart.Should().BeTrue();
    }

    [Fact]
    public void Method_TryGetStartDate_returns_True_when_FromDate_gt_BeginDate()
    {
        // Arrange
        var beginDate = DateOnly.MinValue;
        var fromDate = new DateOnly(1, 1, 2);
        const int interval = 1;

        // Act
        var canStart = DailyRecurrenceHelper
            .TryGetStartDate(beginDate, fromDate, interval, out _);

        //Assert
        canStart.Should().BeTrue();
    }

    [Fact]
    public void Method_TryGetStartDate_returns_False_when_FromDate_gt_BeginDate_and_result_is_outOfRange()
    {
        // Arrange
        var beginDate = new DateOnly(9999, 12, 30);
        var fromDate = new DateOnly(9999, 12, 31);
        const int interval = 2;

        // Act
        var canStart = DailyRecurrenceHelper.TryGetStartDate(
            beginDate,
            fromDate,
            interval,
            out _);

        //Assert
        canStart.Should().BeFalse();
    }
}