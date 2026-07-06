using DateRecurrenceR.Core;
using DateRecurrenceR.Internals;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Internals;

[TestSubject(typeof(MonthlyDateResolver))]
public class MonthlyDateResolverTests
{
    [Fact]
    public void ByDayOfMonth_returns_the_day_in_the_month()
    {
        var sut = MonthlyDateResolver.ByDayOfMonth(15);

        sut.GetDate(2026, 3).Should().Be(new DateOnly(2026, 3, 15));
    }

    [Fact]
    public void ByDayOfMonth_clamps_to_the_last_day_of_a_short_month()
    {
        var sut = MonthlyDateResolver.ByDayOfMonth(31);

        sut.GetDate(2026, 4).Should().Be(new DateOnly(2026, 4, 30));
        sut.GetDate(2026, 2).Should().Be(new DateOnly(2026, 2, 28));
        sut.GetDate(2028, 2).Should().Be(new DateOnly(2028, 2, 29), "2028 is a leap year");
    }

    [Fact]
    public void ByDayOfWeek_returns_the_nth_weekday()
    {
        // 2nd Tuesday of January 2026 is January 13
        var sut = MonthlyDateResolver.ByDayOfWeek(DayOfWeek.Tuesday, IndexOfDay.Second);

        sut.GetDate(2026, 1).Should().Be(new DateOnly(2026, 1, 13));
    }

    [Fact]
    public void ByDayOfWeek_with_Last_returns_the_last_weekday_of_the_month()
    {
        // January 2026 has five Fridays: 2, 9, 16, 23, 30
        var sut = MonthlyDateResolver.ByDayOfWeek(DayOfWeek.Friday, IndexOfDay.Last);

        sut.GetDate(2026, 1).Should().Be(new DateOnly(2026, 1, 30));
        // February 2026 has four Fridays; the last one is the 27th
        sut.GetDate(2026, 2).Should().Be(new DateOnly(2026, 2, 27));
    }
}
