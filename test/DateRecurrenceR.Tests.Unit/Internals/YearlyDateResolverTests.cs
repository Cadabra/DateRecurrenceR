using DateRecurrenceR.Core;
using DateRecurrenceR.Internals;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Internals;

[TestSubject(typeof(YearlyDateResolver))]
public class YearlyDateResolverTests
{
    [Fact]
    public void ByDayOfYear_returns_the_day_of_the_year()
    {
        var sut = YearlyDateResolver.ByDayOfYear(100);

        sut.GetDate(2026).Should().Be(new DateOnly(2026, 4, 10));
        sut.GetDate(2028).Should().Be(new DateOnly(2028, 4, 9), "2028 is a leap year");
    }

    [Fact]
    public void ByDayOfYear_clamps_day_366_in_non_leap_years()
    {
        var sut = YearlyDateResolver.ByDayOfYear(366);

        sut.GetDate(2024).Should().Be(new DateOnly(2024, 12, 31), "2024 is a leap year");
        sut.GetDate(2025).Should().Be(new DateOnly(2025, 12, 31), "day 366 clamps to the last day");
    }

    [Fact]
    public void ByDayOfMonth_returns_the_day_in_the_month()
    {
        var sut = YearlyDateResolver.ByDayOfMonth(3, 10);

        sut.GetDate(2026).Should().Be(new DateOnly(2026, 3, 10));
        sut.GetDate(2028).Should().Be(new DateOnly(2028, 3, 10), "the calendar date must not shift in leap years");
    }

    [Fact]
    public void ByDayOfMonth_clamps_February_29_in_non_leap_years()
    {
        var sut = YearlyDateResolver.ByDayOfMonth(2, 29);

        sut.GetDate(2028).Should().Be(new DateOnly(2028, 2, 29));
        sut.GetDate(2027).Should().Be(new DateOnly(2027, 2, 28));
    }

    [Fact]
    public void ByDayOfWeek_returns_the_nth_weekday_of_the_month()
    {
        // 2nd Tuesday of March 2027 is March 9
        var sut = YearlyDateResolver.ByDayOfWeek(3, DayOfWeek.Tuesday, IndexOfDay.Second);

        sut.GetDate(2027).Should().Be(new DateOnly(2027, 3, 9));
    }
}
