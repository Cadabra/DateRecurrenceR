using DateRecurrenceR.Core;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit;

[TestSubject(typeof(Recurrence))]
public sealed class RecurrenceWeeklyTests
{
    private static List<DateOnly> Collect(IEnumerator<DateOnly> e)
    {
        var list = new List<DateOnly>();
        while (e.MoveNext()) list.Add(e.Current);
        return list;
    }

    // --- by count ---

    [Fact]
    public void Weekly_ByCount_returns_empty_when_count_is_zero()
    {
        var weekDays = new WeekDays(DayOfWeek.Monday);
        var dates = Collect(Recurrence.Weekly(new DateOnly(2025, 1, 6), 0, weekDays, DayOfWeek.Monday, new Interval(1)));

        dates.Should().BeEmpty();
    }

    /// <summary>
    /// Regression: a negative count near the calendar start produced a negative day number
    /// and threw <see cref="ArgumentOutOfRangeException"/> instead of yielding nothing.
    /// </summary>
    [Fact]
    public void Weekly_ByCount_returns_empty_when_count_is_negative()
    {
        var weekDays = new WeekDays(DayOfWeek.Monday, DayOfWeek.Friday);
        var dates = Collect(Recurrence.Weekly(new DateOnly(1, 1, 1), -5, weekDays, DayOfWeek.Sunday, new Interval(1)));

        dates.Should().BeEmpty();
    }

    /// <summary>
    /// Regression: an empty <see cref="WeekDays"/> (reachable via <c>default</c> or the all-false
    /// constructor) reached the weekly helpers, which divide by the number of selected days,
    /// and threw <see cref="DivideByZeroException"/>.
    /// </summary>
    [Fact]
    public void Weekly_ByCount_returns_empty_when_weekDays_is_empty()
    {
        var dates = Collect(Recurrence.Weekly(new DateOnly(2025, 1, 6), 5, default(WeekDays), DayOfWeek.Monday, new Interval(1)));

        dates.Should().BeEmpty();
    }

    /// <inheritdoc cref="Weekly_ByCount_returns_empty_when_weekDays_is_empty"/>
    [Fact]
    public void Weekly_ByEndDate_returns_empty_when_weekDays_is_empty()
    {
        var weekDays = new WeekDays(false, false, false, false, false, false, false);
        var dates = Collect(Recurrence.Weekly(new DateOnly(2025, 1, 6), new DateOnly(2025, 3, 1), weekDays, DayOfWeek.Monday, new Interval(1)));

        dates.Should().BeEmpty();
    }

    [Fact]
    public void Weekly_ByCount_single_day_every_week()
    {
        // 2025-01-06 is Monday
        var begin = new DateOnly(2025, 1, 6);
        var weekDays = new WeekDays(DayOfWeek.Monday);

        var dates = Collect(Recurrence.Weekly(begin, 4, weekDays, DayOfWeek.Monday, new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 6),
            new DateOnly(2025, 1, 13),
            new DateOnly(2025, 1, 20),
            new DateOnly(2025, 1, 27));
    }

    [Fact]
    public void Weekly_ByCount_two_days_every_week()
    {
        // 2025-01-06 is Monday
        var begin = new DateOnly(2025, 1, 6);
        var weekDays = new WeekDays(DayOfWeek.Monday, DayOfWeek.Wednesday);

        var dates = Collect(Recurrence.Weekly(begin, 4, weekDays, DayOfWeek.Monday, new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 6),
            new DateOnly(2025, 1, 8),
            new DateOnly(2025, 1, 13),
            new DateOnly(2025, 1, 15));
    }

    [Fact]
    public void Weekly_ByCount_all_seven_days()
    {
        var begin = new DateOnly(2025, 1, 6); // Monday
        var weekDays = new WeekDays(true, true, true, true, true, true, true);

        var dates = Collect(Recurrence.Weekly(begin, 7, weekDays, DayOfWeek.Monday, new Interval(1)));

        dates.Should().HaveCount(7);
        dates.Should().BeInAscendingOrder();
        // 7 consecutive days starting from Monday
        dates[0].Should().Be(new DateOnly(2025, 1, 6));
        dates[6].Should().Be(new DateOnly(2025, 1, 12));
    }

    [Fact]
    public void Weekly_ByCount_with_interval_2_skips_alternate_weeks()
    {
        var begin = new DateOnly(2025, 1, 6); // Monday
        var weekDays = new WeekDays(DayOfWeek.Monday);

        var dates = Collect(Recurrence.Weekly(begin, 3, weekDays, DayOfWeek.Monday, new Interval(2)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 6),
            new DateOnly(2025, 1, 20),
            new DateOnly(2025, 2, 3));
    }

    [Fact]
    public void Weekly_ByCount_beginDate_not_on_selected_day_starts_from_next_occurrence()
    {
        var begin = new DateOnly(2025, 1, 7); // Tuesday
        var weekDays = new WeekDays(DayOfWeek.Friday);

        var dates = Collect(Recurrence.Weekly(begin, 2, weekDays, DayOfWeek.Monday, new Interval(1)));

        dates[0].DayOfWeek.Should().Be(DayOfWeek.Friday);
        dates[0].Should().Be(new DateOnly(2025, 1, 10));
    }

    [Fact]
    public void Weekly_ByCount_different_firstDayOfWeek()
    {
        var begin = new DateOnly(2025, 1, 5); // Sunday
        var weekDays = new WeekDays(DayOfWeek.Sunday);

        var dates = Collect(Recurrence.Weekly(begin, 3, weekDays, DayOfWeek.Sunday, new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 5),
            new DateOnly(2025, 1, 12),
            new DateOnly(2025, 1, 19));
    }

    // --- by endDate ---

    [Fact]
    public void Weekly_ByEndDate_does_not_exceed_endDate()
    {
        var begin = new DateOnly(2025, 1, 6); // Monday
        var end = new DateOnly(2025, 1, 20);
        var weekDays = new WeekDays(DayOfWeek.Monday);

        var dates = Collect(Recurrence.Weekly(begin, end, weekDays, DayOfWeek.Monday, new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 6),
            new DateOnly(2025, 1, 13),
            new DateOnly(2025, 1, 20));
        dates.Should().AllSatisfy(d => d.Should().BeOnOrBefore(end));
    }

    [Fact]
    public void Weekly_ByEndDate_crosses_year_boundary()
    {
        var begin = new DateOnly(2025, 12, 29); // Monday
        var end = new DateOnly(2026, 1, 12);
        var weekDays = new WeekDays(DayOfWeek.Monday);

        var dates = Collect(Recurrence.Weekly(begin, end, weekDays, DayOfWeek.Monday, new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 12, 29),
            new DateOnly(2026, 1, 5),
            new DateOnly(2026, 1, 12));
    }

    // --- ordering ---

    [Fact]
    public void Weekly_all_dates_are_in_ascending_order()
    {
        var begin = new DateOnly(2025, 1, 1);
        var weekDays = new WeekDays(DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday);

        var dates = Collect(Recurrence.Weekly(begin, 50, weekDays, DayOfWeek.Monday, new Interval(1)));

        dates.Should().BeInAscendingOrder();
    }

    /// <summary>The by-dates overload clips to the earlier of toDate and endDate.</summary>
    [Fact]
    public void Weekly_ByDates_clips_to_the_earlier_of_toDate_and_endDate()
    {
        var weekDays = new WeekDays(DayOfWeek.Monday, DayOfWeek.Friday);
        // toDate before endDate: stop at toDate.
        var byTo = Collect(Recurrence.Weekly(
            new DateOnly(2026, 1, 1), new DateOnly(2026, 3, 31),
            new DateOnly(2026, 1, 12), new DateOnly(2026, 1, 23), weekDays, DayOfWeek.Monday, new Interval(1)));
        // toDate after endDate: stop at endDate (same window).
        var byEnd = Collect(Recurrence.Weekly(
            new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 23),
            new DateOnly(2026, 1, 12), new DateOnly(2026, 3, 31), weekDays, DayOfWeek.Monday, new Interval(1)));

        byTo.Should().Equal(byEnd).And.OnlyContain(d => d >= new DateOnly(2026, 1, 12) && d <= new DateOnly(2026, 1, 23));
        byTo.Should().NotBeEmpty();
    }
}
