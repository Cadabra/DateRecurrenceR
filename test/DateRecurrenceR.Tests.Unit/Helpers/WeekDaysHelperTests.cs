using DateRecurrenceR.Helpers;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Helpers;

[TestSubject(typeof(WeekDaysHelper))]
public class WeekDaysHelperTests
{
    [Theory]
    [InlineData(DayOfWeek.Monday, DayOfWeek.Monday, 0)]
    [InlineData(DayOfWeek.Monday, DayOfWeek.Friday, 4)]
    [InlineData(DayOfWeek.Friday, DayOfWeek.Monday, 3)]
    [InlineData(DayOfWeek.Sunday, DayOfWeek.Saturday, 6)]
    [InlineData(DayOfWeek.Saturday, DayOfWeek.Sunday, 1)]
    public void GetDiffToDay_returns_days_from_first_day_to_target(
        DayOfWeek firstDayOfWeek, DayOfWeek dayOfWeek, int expected)
    {
        WeekDaysHelper.GetDiffToDay(firstDayOfWeek, dayOfWeek).Should().Be(expected);
    }

    [Theory]
    [InlineData(DayOfWeek.Monday, DayOfWeek.Monday, 0)]
    [InlineData(DayOfWeek.Friday, DayOfWeek.Monday, 4)]
    [InlineData(DayOfWeek.Monday, DayOfWeek.Friday, 3)]
    [InlineData(DayOfWeek.Sunday, DayOfWeek.Sunday, 0)]
    [InlineData(DayOfWeek.Sunday, DayOfWeek.Monday, 6)]
    public void DayToIndex_returns_position_of_the_day_in_the_week(
        DayOfWeek dayOfWeek, DayOfWeek firstDayOfWeek, int expected)
    {
        WeekDaysHelper.DayToIndex(dayOfWeek, firstDayOfWeek).Should().Be(expected);
    }
}
