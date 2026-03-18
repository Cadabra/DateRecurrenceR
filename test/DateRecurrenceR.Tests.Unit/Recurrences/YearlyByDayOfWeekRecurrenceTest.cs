using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(YearlyByDayOfWeekRecurrence))]
public class YearlyByDayOfWeekRecurrenceTest
{
    // Use a narrow end-date range so the pre-existing StopDate bug does not interfere.
    private static YearlyByDayOfWeekRecurrence CreateRecurrence(
        DateOnly beginDate, DateOnly endDate, DayOfWeek dayOfWeek, IndexOfDay indexOfDay,
        MonthOfYear monthOfYear, Interval interval)
    {
        var pattern = new YearlyByDayOfWeekPattern(interval, dayOfWeek, indexOfDay, monthOfYear);
        return YearlyByDayOfWeekRecurrence.New(new Range(beginDate, endDate), pattern);
    }

    [Fact]
    public void Yearly_ByDayOfWeek_Contains_ReturnsFalse_ForWrongOccurrence()
    {
        // Arrange: every 1st Monday of March, from 2020 to 2021.
        // March 2020: 1st Monday = March 2; 2nd Monday = March 9
        var sut = CreateRecurrence(
            new DateOnly(2020, 1, 1), new DateOnly(2021, 12, 31),
            DayOfWeek.Monday, IndexOfDay.First, new MonthOfYear(3), new Interval(1));

        var secondMondayMarch2020 = new DateOnly(2020, 3, 9);

        // Act & Assert: the 2nd Monday of March should NOT be in a "1st Monday of March" recurrence
        sut.Contains(secondMondayMarch2020).Should().BeFalse();
    }

    [Fact]
    public void Yearly_ByDayOfWeek_Contains_ReturnsFalse_ForDifferentMonth()
    {
        // Arrange: every 1st Monday of March; a Monday in April should not be contained
        var sut = CreateRecurrence(
            new DateOnly(2020, 1, 1), new DateOnly(2021, 12, 31),
            DayOfWeek.Monday, IndexOfDay.First, new MonthOfYear(3), new Interval(1));

        var firstMondayApril = new DateOnly(2020, 4, 6); // 1st Monday of April 2020

        sut.Contains(firstMondayApril).Should().BeFalse();
    }

    [Theory]
    [InlineData(IndexOfDay.First)]
    [InlineData(IndexOfDay.Second)]
    [InlineData(IndexOfDay.Third)]
    [InlineData(IndexOfDay.Fourth)]
    [InlineData(IndexOfDay.Last)]
    public void Yearly_ByDayOfWeek_Contains_CorrectForAllIndexOfDay(IndexOfDay indexOfDay)
    {
        // Arrange: Thursday at the given occurrence in November, narrow 2-year range
        var beginDate = new DateOnly(2020, 1, 1);
        var endDate = new DateOnly(2021, 12, 31);
        var interval = new Interval(1);
        var monthOfYear = new MonthOfYear(11);
        var sut = CreateRecurrence(beginDate, endDate, DayOfWeek.Thursday, indexOfDay, monthOfYear, interval);
        var enumerator = Recurrence.Yearly(beginDate, endDate, DayOfWeek.Thursday, indexOfDay, monthOfYear, interval);

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

    [Fact]
    public void Yearly_ByDayOfWeek_Contains_FirstMondayOfMarch_CorrectForKnownDates()
    {
        // Arrange: 1st Monday of March, every year, narrow range 2020-2022
        var sut = CreateRecurrence(
            new DateOnly(2020, 1, 1), new DateOnly(2022, 12, 31),
            DayOfWeek.Monday, IndexOfDay.First, new MonthOfYear(3), new Interval(1));

        // Known 1st Mondays of March:
        // 2020: March 2  (March 1 is Sunday)
        // 2021: March 1  (March 1 is Monday)
        // 2022: March 7  (March 1 is Tuesday)
        sut.Contains(new DateOnly(2020, 3, 2)).Should().BeTrue();
        sut.Contains(new DateOnly(2021, 3, 1)).Should().BeTrue();
        sut.Contains(new DateOnly(2022, 3, 7)).Should().BeTrue();

        // 2nd Mondays of March (should be false):
        sut.Contains(new DateOnly(2020, 3, 9)).Should().BeFalse();
        sut.Contains(new DateOnly(2021, 3, 8)).Should().BeFalse();
        sut.Contains(new DateOnly(2022, 3, 14)).Should().BeFalse();
    }
}
