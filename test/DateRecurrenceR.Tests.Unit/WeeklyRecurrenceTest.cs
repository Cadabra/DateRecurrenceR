using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Tests.Unit;

[TestSubject(typeof(WeeklyByWeekDaysRecurrence))]
public class WeeklyRecurrenceTest
{
    [Fact]
    public void Enumerator_matches_Recurrence_all_days_large_interval()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 1); // Monday
        var endDate = beginDate.AddDays(6000);
        var weekDays = new WeekDays(true, true, true, true, true, true, true);
        var firstDayOfWeek = DayOfWeek.Monday;
        var interval = new Interval(110);
        var weeklyPattern = new WeeklyByWeekDaysPattern(interval, weekDays, firstDayOfWeek);
        var enumerator = Recurrence.Weekly(beginDate, endDate, weekDays, firstDayOfWeek, interval);
        var sut = WeeklyByWeekDaysRecurrence.New(new Range(beginDate, endDate), weeklyPattern);

        // Act
        var list = new List<DateOnly>();
        var allContained = true;
        using var e = sut.GetEnumerator();
        while (enumerator.MoveNext())
        {
            e.MoveNext();
            enumerator.Current.Should().Be(e.Current);
            list.Add(enumerator.Current);
            allContained &= sut.Contains(enumerator.Current);
        }

        e.MoveNext().Should().Be(enumerator.MoveNext());

        // Assert
        beginDate.Should().Be(list.First());
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        allContained.Should().BeTrue();
    }

    [Fact]
    public void Enumerator_matches_Contains_all_days_short_range()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 3); // Wednesday
        var endDate = beginDate.AddDays(20);
        var weekDays = new WeekDays(true, true, true, true, true, true, true);
        var firstDayOfWeek = DayOfWeek.Monday;
        var interval = new Interval(1);
        var weeklyPattern = new WeeklyByWeekDaysPattern(interval, weekDays, firstDayOfWeek);
        var enumerator = Recurrence.Weekly(beginDate, endDate, weekDays, firstDayOfWeek, interval);
        var sut = WeeklyByWeekDaysRecurrence.New(new Range(beginDate, endDate), weeklyPattern);

        // Act
        var list = new List<DateOnly>();
        var allContained = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            allContained &= sut.Contains(enumerator.Current);
        }

        // Assert
        beginDate.Should().Be(list.First());
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        allContained.Should().BeTrue();
    }

    [Fact]
    public void Enumerator_matches_Contains_two_days_short_range()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 5); // Friday
        var endDate = beginDate.AddDays(20);
        var weekDays = new WeekDays(DayOfWeek.Monday, DayOfWeek.Friday);
        var firstDayOfWeek = DayOfWeek.Monday;
        var interval = new Interval(1);
        var weeklyPattern = new WeeklyByWeekDaysPattern(interval, weekDays, firstDayOfWeek);
        var enumerator = Recurrence.Weekly(beginDate, endDate, weekDays, firstDayOfWeek, interval);
        var sut = WeeklyByWeekDaysRecurrence.New(new Range(beginDate, endDate), weeklyPattern);

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
    public void Enumerator_matches_Contains_all_days_Tuesday_firstDayOfWeek_large_interval()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 3); // Wednesday
        var endDate = beginDate.AddDays(6000);
        var weekDays = new WeekDays(true, true, true, true, true, true, true);
        var firstDayOfWeek = DayOfWeek.Tuesday;
        var interval = new Interval(110);
        var weeklyPattern = new WeeklyByWeekDaysPattern(interval, weekDays, firstDayOfWeek);
        var enumerator = Recurrence.Weekly(beginDate, endDate, weekDays, firstDayOfWeek, interval);
        var sut = WeeklyByWeekDaysRecurrence.New(new Range(beginDate, endDate), weeklyPattern);

        // Act
        var list = new List<DateOnly>();
        var allContained = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            allContained &= sut.Contains(enumerator.Current);
        }

        // Assert
        beginDate.Should().Be(list.First());
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        allContained.Should().BeTrue();
    }

    [Fact]
    public void Enumerator_matches_Contains_two_days_Tuesday_firstDayOfWeek_large_interval()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 5); // Friday
        var endDate = beginDate.AddDays(6000);
        var weekDays = new WeekDays(DayOfWeek.Tuesday, DayOfWeek.Friday);
        var firstDayOfWeek = DayOfWeek.Tuesday;
        var interval = new Interval(110);
        var weeklyPattern = new WeeklyByWeekDaysPattern(interval, weekDays, firstDayOfWeek);
        var enumerator = Recurrence.Weekly(beginDate, endDate, weekDays, firstDayOfWeek, interval);
        var sut = WeeklyByWeekDaysRecurrence.New(new Range(beginDate, endDate), weeklyPattern);

        // Act
        var list = new List<DateOnly>();
        var allContained = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            allContained &= sut.Contains(enumerator.Current);
        }

        // Assert
        beginDate.Should().Be(list.First());
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        allContained.Should().BeTrue();
    }
}
