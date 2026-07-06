using DateRecurrenceR.Core;
using DateRecurrenceR.Tests.Common;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(MonthlyByDayOfMonthRecurrence))]
public class MonthlyByDayOfMonthRecurrenceTests : RecurrenceContractTests<MonthlyByDayOfMonthRecurrence>
{
    [Fact]
    public void Enumerator_matches_Contains_by_count()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 1);
        var interval = new Interval(7);
        var count = 200;
        var dayOfMonth = new DayOfMonth(1);
        var range = new DateRange(beginDate, count);
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
        var range = new DateRange(beginDate, endDate);
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
        var range = new DateRange(beginDate, count);
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

    protected override MonthlyByDayOfMonthRecurrence Create(DateRange range)
    {
        return MonthlyByDayOfMonthRecurrence.New(range, new MonthlyByDayOfMonthPattern(new Interval(1), new DayOfMonth(15)));
    }

    [Fact]
    public void New_that_cannot_start_before_the_calendar_end_is_empty()
    {
        // Day 15 requested from Dec 20 9999: the next occurrence is in year 10000.
        var pattern = new MonthlyByDayOfMonthPattern(new Interval(1), new DayOfMonth(15));
        var sut = MonthlyByDayOfMonthRecurrence.New(new DateRange(new DateOnly(9999, 12, 20), 5), pattern);

        sut.Count.Should().Be(0);
        Collector.Collect(sut).Should().BeEmpty();
    }

    [Fact]
    public void Contains_rejects_a_matching_day_in_an_off_interval_month()
    {
        // Every 2nd month on the 15th from Jan 2026: Jan, Mar, May, ... Feb 15 is off-interval.
        var pattern = new MonthlyByDayOfMonthPattern(new Interval(2), new DayOfMonth(15));
        var sut = MonthlyByDayOfMonthRecurrence.New(new DateRange(new DateOnly(2026, 1, 15), 6), pattern);

        sut.Contains(new DateOnly(2026, 1, 15)).Should().BeTrue();
        sut.Contains(new DateOnly(2026, 3, 15)).Should().BeTrue();
        sut.Contains(new DateOnly(2026, 2, 15)).Should().BeFalse("February is an off-interval month");
    }
}
