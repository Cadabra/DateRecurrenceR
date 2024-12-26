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
    public void Enumerator_matches_Contains_by_count()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 1);
        var interval = new Interval(7);
        var count = 200;
        var dayOfMonth = new DayOfMonth(1);
        var range = new Range(beginDate, count);
        var pattern = new MonthlyByDayOfMonthPattern(interval, dayOfMonth);
        var enumerator = Recurrence.Monthly(beginDate, count, dayOfMonth, interval);
        var sut = MonthlyByDayOfMonthRecurrence.New(range, pattern);

        // Act
        var list = new List<DateOnly>();
        var allContained = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            allContained &= sut.Contains(enumerator.Current);
        }

        // Assert
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        allContained.Should().BeTrue();
    }

    [Fact]
    public void Enumerator_matches_Contains_by_endDate()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 1);
        var interval = new Interval(7);
        var endDate = beginDate.AddMonths(2000);
        var dayOfMonth = new DayOfMonth(1);
        var range = new Range(beginDate, endDate);
        var pattern = new MonthlyByDayOfMonthPattern(interval, dayOfMonth);
        var enumerator = Recurrence.Monthly(beginDate, endDate, dayOfMonth, interval);
        var sut = MonthlyByDayOfMonthRecurrence.New(range, pattern);

        // Act
        var list = new List<DateOnly>();
        var allContained = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            allContained &= sut.Contains(enumerator.Current);
        }

        // Assert
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        allContained.Should().BeTrue();
    }

    /// <summary>
    /// CRITICAL: Regression — December month with interval=12 (always December).
    /// </summary>
    [Fact]
    public void Enumerator_matches_Contains_December_interval_12()
    {
        // Arrange
        var beginDate = new DateOnly(2020, 12, 25);
        var interval = new Interval(12);
        var count = 10;
        var dayOfMonth = new DayOfMonth(25);
        var range = new Range(beginDate, count);
        var pattern = new MonthlyByDayOfMonthPattern(interval, dayOfMonth);
        var enumerator = Recurrence.Monthly(beginDate, count, dayOfMonth, interval);
        var sut = MonthlyByDayOfMonthRecurrence.New(range, pattern);

        // Act
        var list = new List<DateOnly>();
        var allContained = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            allContained &= sut.Contains(enumerator.Current);
        }

        // Assert
        list.Should().HaveCount(count);
        list.Should().AllSatisfy(d => d.Month.Should().Be(12));
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        allContained.Should().BeTrue();
    }
}
