using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(MonthlyByDayOfMonthRecurrence))]
public class MonthlyByDayOfMonthRecurrenceTest
{
    [Fact]
    public void METHOD()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 1); // Monday
        var interval = new Interval(7);
        var count = 200;
        var dayOfMonth = new DayOfMonth(1);
        var range = new Range(beginDate, count);
        var pattern = new MonthlyByDayOfMonthPattern(interval, dayOfMonth);
        var enumerator = Recurrence.Monthly(beginDate, count, dayOfMonth, interval);
        var sut = MonthlyByDayOfMonthRecurrence.New(range, pattern);

        // Act
        var list = new List<DateOnly>(DateOnly.MaxValue.DayNumber);
        var isContains = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            isContains &= sut.Contains(enumerator.Current);
        }

        // Assert
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        isContains.Should().BeTrue();
    }

    [Fact]
    public void METHOD_2()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 1); // Monday
        var interval = new Interval(7);
        var endDate = beginDate.AddMonths(2000);
        var dayOfMonth = new DayOfMonth(1);
        var range = new Range(beginDate, endDate);
        var pattern = new MonthlyByDayOfMonthPattern(interval, dayOfMonth);
        var enumerator = Recurrence.Monthly(beginDate, endDate, dayOfMonth, interval);
        var sut = MonthlyByDayOfMonthRecurrence.New(range, pattern);

        // Act
        var list = new List<DateOnly>(DateOnly.MaxValue.DayNumber);
        var isContains = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            isContains &= sut.Contains(enumerator.Current);
        }

        // Assert
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        isContains.Should().BeTrue();
    }
}