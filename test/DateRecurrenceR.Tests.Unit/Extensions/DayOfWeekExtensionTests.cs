using DateRecurrenceR.Extensions;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Extensions;

[TestSubject(typeof(DayOfWeekExtension))]
public class DayOfWeekExtensionTests
{
    [Theory]
    [InlineData(DayOfWeek.Sunday, DayOfWeek.Monday)]
    [InlineData(DayOfWeek.Monday, DayOfWeek.Tuesday)]
    [InlineData(DayOfWeek.Tuesday, DayOfWeek.Wednesday)]
    [InlineData(DayOfWeek.Wednesday, DayOfWeek.Thursday)]
    [InlineData(DayOfWeek.Thursday, DayOfWeek.Friday)]
    [InlineData(DayOfWeek.Friday, DayOfWeek.Saturday)]
    [InlineData(DayOfWeek.Saturday, DayOfWeek.Sunday)]
    public void Next_returns_the_following_day(DayOfWeek day, DayOfWeek expected)
    {
        day.Next().Should().Be(expected);
    }

    [Theory]
    [InlineData(DayOfWeek.Sunday, DayOfWeek.Saturday)]
    [InlineData(DayOfWeek.Monday, DayOfWeek.Sunday)]
    [InlineData(DayOfWeek.Tuesday, DayOfWeek.Monday)]
    [InlineData(DayOfWeek.Wednesday, DayOfWeek.Tuesday)]
    [InlineData(DayOfWeek.Thursday, DayOfWeek.Wednesday)]
    [InlineData(DayOfWeek.Friday, DayOfWeek.Thursday)]
    [InlineData(DayOfWeek.Saturday, DayOfWeek.Friday)]
    public void Prev_returns_the_preceding_day(DayOfWeek day, DayOfWeek expected)
    {
        day.Prev().Should().Be(expected);
    }
}
