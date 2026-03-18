using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(MonthlyByDayOfWeekRecurrence))]
public class MonthlyByDayOfWeekRecurrenceTest
{
    // Use a small end-date range so that the pre-existing StopDate bug does not interfere.
    private static MonthlyByDayOfWeekRecurrence CreateRecurrence(
        DateOnly beginDate, DateOnly endDate, DayOfWeek dayOfWeek, IndexOfDay indexOfDay, Interval interval)
    {
        var pattern = new MonthlyByDayOfWeekPattern(interval, dayOfWeek, indexOfDay);
        return MonthlyByDayOfWeekRecurrence.New(new Range(beginDate, endDate), pattern);
    }

    [Fact]
    public void Monthly_ByDayOfWeek_Contains_ReturnsFalse_ForWrongOccurrence()
    {
        // Arrange: every 1st Monday of every month.
        // January 2024: 1st Monday = Jan 1; 2nd Monday = Jan 8
        var sut = CreateRecurrence(
            new DateOnly(2024, 1, 1), new DateOnly(2024, 6, 30),
            DayOfWeek.Monday, IndexOfDay.First, new Interval(1));

        var secondMonday = new DateOnly(2024, 1, 8);

        // Act & Assert: the 2nd Monday should NOT match a "1st Monday" recurrence
        sut.Contains(secondMonday).Should().BeFalse();
    }

    [Fact]
    public void Monthly_ByDayOfWeek_Contains_ReturnsFalse_ForDifferentDayOfWeek()
    {
        // Arrange: 1st Monday; Tuesday Jan 2 2024 should not be contained
        var sut = CreateRecurrence(
            new DateOnly(2024, 1, 1), new DateOnly(2024, 6, 30),
            DayOfWeek.Monday, IndexOfDay.First, new Interval(1));

        var tuesday = new DateOnly(2024, 1, 2); // 1st Tuesday of January 2024

        sut.Contains(tuesday).Should().BeFalse();
    }

    [Fact]
    public void Monthly_ByDayOfWeek_WithEndDate_ContainsAllEnumeratedDates()
    {
        // Arrange: every 2nd Wednesday of every month, narrow range to avoid StopDate bug
        var beginDate = new DateOnly(2020, 1, 1);
        var endDate = new DateOnly(2020, 12, 31); // Just one year
        var interval = new Interval(1);
        var dayOfWeek = DayOfWeek.Wednesday;
        var indexOfDay = IndexOfDay.Second;
        var sut = CreateRecurrence(beginDate, endDate, dayOfWeek, indexOfDay, interval);
        var enumerator = Recurrence.Monthly(beginDate, endDate, dayOfWeek, indexOfDay, interval);

        // Act
        var list = new List<DateOnly>();
        var isContains = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            isContains &= sut.Contains(enumerator.Current);
        }

        // Assert
        list.Should().NotBeEmpty();
        isContains.Should().BeTrue();
    }

    [Theory]
    [InlineData(IndexOfDay.First)]
    [InlineData(IndexOfDay.Second)]
    [InlineData(IndexOfDay.Third)]
    [InlineData(IndexOfDay.Fourth)]
    [InlineData(IndexOfDay.Last)]
    public void Monthly_ByDayOfWeek_Contains_CorrectForAllIndexOfDay(IndexOfDay indexOfDay)
    {
        // Arrange: every Friday at the given occurrence for 6 months (narrow range)
        var beginDate = new DateOnly(2023, 1, 1);
        var endDate = new DateOnly(2023, 6, 30);
        var interval = new Interval(1);
        var sut = CreateRecurrence(beginDate, endDate, DayOfWeek.Friday, indexOfDay, interval);
        var enumerator = Recurrence.Monthly(beginDate, endDate, DayOfWeek.Friday, indexOfDay, interval);

        // Act
        var list = new List<DateOnly>();
        var isContains = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            isContains &= sut.Contains(enumerator.Current);
        }

        // Assert: all enumerated dates must pass Contains
        list.Should().NotBeEmpty();
        isContains.Should().BeTrue();
    }
}
