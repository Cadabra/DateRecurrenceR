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
    public void METHOD()
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
        var list = new List<DateOnly>(DateOnly.MaxValue.DayNumber);
        var isContains = true;
        using var e = sut.GetEnumerator();
        while (enumerator.MoveNext())
        {
            e.MoveNext();
            if (enumerator.Current != e.Current)
            {
                throw new Exception("Enum Panic!");
            }
            list.Add(enumerator.Current);
            isContains &= sut.Contains(enumerator.Current);
        }
        
        Assert.Equal(e.MoveNext(), enumerator.MoveNext());

        // Assert
        beginDate.Should().Be(list.First());
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        isContains.Should().BeTrue();
    }

    [Fact]
    public void METHOD_2()
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
        var list = new List<DateOnly>(DateOnly.MaxValue.DayNumber);
        var isContains = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            isContains &= sut.Contains(enumerator.Current);
        }

        // Assert
        beginDate.Should().Be(list.First());
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        isContains.Should().BeTrue();
    }

    [Fact]
    public void METHOD_2_2()
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
        var list = new List<DateOnly>(DateOnly.MaxValue.DayNumber);
        var isContains = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            isContains &= sut.Contains(enumerator.Current);
        }

        // Assert
        beginDate.Should().Be(list.First());
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        isContains.Should().BeTrue();
    }

    [Fact]
    public void METHOD_2_3()
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
    public void METHOD_3()
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
        var list = new List<DateOnly>(DateOnly.MaxValue.DayNumber);
        var isContains = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            isContains &= sut.Contains(enumerator.Current);
            if (!isContains)
            {
                sut.Contains(enumerator.Current);
            }
        }

        // Assert
        beginDate.Should().Be(list.First());
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        isContains.Should().BeTrue();
    }

    [Fact]
    public void METHOD_3_2()
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
        var list = new List<DateOnly>(DateOnly.MaxValue.DayNumber);
        var isContains = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            isContains &= sut.Contains(enumerator.Current);
            if (!isContains)
            {
                sut.Contains(enumerator.Current);
            }
        }

        // Assert
        beginDate.Should().Be(list.First());
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        isContains.Should().BeTrue();
    }

    [Fact]
    public void METHOD_3_3()
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
        var list = new List<DateOnly>(DateOnly.MaxValue.DayNumber);
        var isContains = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            isContains &= sut.Contains(enumerator.Current);
            if (!isContains)
            {
                sut.Contains(enumerator.Current);
            }
        }

        // Assert
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        isContains.Should().BeTrue();
    }
}