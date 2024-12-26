using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(DailyRecurrence))]
public class DailyRecurrenceTest
{
    [Fact]
    public void METHOD()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 1); // Monday
        var endDate = beginDate.AddDays(1000);
        var interval = new Interval(3);
        var range = new Range(beginDate, endDate);
        var pattern = new DailyPattern(interval);
        var enumerator = Recurrence.Daily(beginDate, endDate, interval);
        var sut = DailyRecurrence.New(range, pattern);

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
        var interval = new Interval(3);
        var count = 200;
        var range = new Range(beginDate, count);
        var pattern = new DailyPattern(interval);
        var enumerator = Recurrence.Daily(beginDate, count, interval);
        var sut = DailyRecurrence.New(range, pattern);

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