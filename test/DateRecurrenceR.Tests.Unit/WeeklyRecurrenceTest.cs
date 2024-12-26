using DateRecurrenceR;
using DateRecurrenceR.Core;
using FluentAssertions;
using JetBrains.Annotations;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Tests.Unit;

[TestSubject(typeof(WeeklyRecurrence))]
public class WeeklyRecurrenceTest
{
    [Fact]
    public void METHOD()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 1); // Monday
        var endDate = beginDate.AddDays(6000);
        var weekDays = new WeekDays(true, true, true, true, true, true, true);
        var firstDayOfWeek = DayOfWeek.Monday;
        var interval = new Interval(110);
        var range = new Range(beginDate, endDate);
        var weeklyPattern = new WeeklyPattern(interval, weekDays, firstDayOfWeek);
        var enumerator = Recurrence.Weekly(beginDate, endDate, weekDays, firstDayOfWeek, interval);
        var sut = new WeeklyRecurrence(range, weeklyPattern);

        // Act
        var list = new List<DateOnly>(DateOnly.MaxValue.DayNumber);
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
        }

        // Assert
        sut.Count.Should().Be(list.Count);
    }

    [Fact]
    public void METHOD_2()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 3); // Wednesday
        var endDate = beginDate.AddDays(20);
        var weekDays = new WeekDays(false, true, false, false, false, true, false);
        // var weekDays = new WeekDays(true, true, true, true, true, true, true);
        var firstDayOfWeek = DayOfWeek.Monday;
        var interval = new Interval(1);
        var range = new Range(beginDate, endDate);
        var weeklyPattern = new WeeklyPattern(interval, weekDays, firstDayOfWeek);
        var enumerator = Recurrence.Weekly(beginDate, endDate, weekDays, firstDayOfWeek, interval);
        var sut = new WeeklyRecurrence(range, weeklyPattern);

        // Act
        var list = new List<DateOnly>(DateOnly.MaxValue.DayNumber);
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
        }

        // Assert
        sut.Count.Should().Be(list.Count);
    }

    [Fact]
    public void METHOD_3()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 3); // Wednesday
        var endDate = beginDate.AddDays(6000);
        var weekDays = new WeekDays(false, false, true, false, false, true, false);
        // var weekDays = new WeekDays(true, true, true, true, true, true, true);
        var firstDayOfWeek = DayOfWeek.Tuesday;
        var interval = new Interval(110);
        var range = new Range(beginDate, endDate);
        var weeklyPattern = new WeeklyPattern(interval, weekDays, firstDayOfWeek);
        var enumerator = Recurrence.Weekly(beginDate, endDate, weekDays, firstDayOfWeek, interval);
        var sut = new WeeklyRecurrence(range, weeklyPattern);

        // Act
        var list = new List<DateOnly>(DateOnly.MaxValue.DayNumber);
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
        }

        // Assert
        sut.Count.Should().Be(list.Count);
    }
}