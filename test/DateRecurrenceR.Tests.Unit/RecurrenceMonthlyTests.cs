using DateRecurrenceR.Core;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit;

/// <summary>
/// Tests for Recurrence.Monthly — the public API entry point.
/// Covers both by-dayOfMonth and by-dayOfWeek+indexOfDay variants,
/// with both by-count and by-endDate overloads.
/// </summary>
public sealed class RecurrenceMonthlyTests
{
    private static List<DateOnly> Collect(IEnumerator<DateOnly> e)
    {
        var list = new List<DateOnly>();
        while (e.MoveNext()) list.Add(e.Current);
        return list;
    }

    // ========================================================================
    // By DayOfMonth, by count
    // ========================================================================

    [Fact]
    public void Monthly_ByDayOfMonth_ByCount_returns_empty_when_count_is_zero()
    {
        var dates = Collect(Recurrence.Monthly(
            new DateOnly(2025, 1, 1), 0, new DayOfMonth(15), new Interval(1)));

        dates.Should().BeEmpty();
    }

    [Fact]
    public void Monthly_ByDayOfMonth_ByCount_returns_single_date()
    {
        var dates = Collect(Recurrence.Monthly(
            new DateOnly(2025, 6, 15), 1, new DayOfMonth(15), new Interval(1)));

        dates.Should().Equal(new DateOnly(2025, 6, 15));
    }

    [Fact]
    public void Monthly_ByDayOfMonth_ByCount_consecutive_months()
    {
        var dates = Collect(Recurrence.Monthly(
            new DateOnly(2025, 1, 1), 4, new DayOfMonth(1), new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 1),
            new DateOnly(2025, 2, 1),
            new DateOnly(2025, 3, 1),
            new DateOnly(2025, 4, 1));
    }

    [Fact]
    public void Monthly_ByDayOfMonth_ByCount_with_interval_2()
    {
        var dates = Collect(Recurrence.Monthly(
            new DateOnly(2025, 1, 15), 3, new DayOfMonth(15), new Interval(2)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 15),
            new DateOnly(2025, 3, 15),
            new DateOnly(2025, 5, 15));
    }

    [Fact]
    public void Monthly_ByDayOfMonth_ByCount_crosses_year_boundary()
    {
        var dates = Collect(Recurrence.Monthly(
            new DateOnly(2025, 11, 1), 4, new DayOfMonth(1), new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 11, 1),
            new DateOnly(2025, 12, 1),
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 2, 1));
    }

    /// <summary>
    /// CRITICAL: December recurrence — the month number 12 must not produce
    /// year+1 and month 0. This is the edge case that was previously broken.
    /// </summary>
    [Fact]
    public void Monthly_ByDayOfMonth_ByCount_December_recurrence_is_correct()
    {
        // Start in December, every 1 month, 3 dates
        var dates = Collect(Recurrence.Monthly(
            new DateOnly(2025, 12, 15), 3, new DayOfMonth(15), new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 12, 15),
            new DateOnly(2026, 1, 15),
            new DateOnly(2026, 2, 15));
    }

    /// <summary>
    /// CRITICAL: Yearly interval that always lands on December.
    /// With interval=12, every occurrence is in December.
    /// </summary>
    [Fact]
    public void Monthly_ByDayOfMonth_ByCount_interval_12_always_December()
    {
        var dates = Collect(Recurrence.Monthly(
            new DateOnly(2020, 12, 25), 4, new DayOfMonth(25), new Interval(12)));

        dates.Should().Equal(
            new DateOnly(2020, 12, 25),
            new DateOnly(2021, 12, 25),
            new DateOnly(2022, 12, 25),
            new DateOnly(2023, 12, 25));
    }

    [Fact]
    public void Monthly_ByDayOfMonth_ByCount_day31_clamped_in_short_months()
    {
        // Day 31 in months with fewer days should clamp to last day
        var dates = Collect(Recurrence.Monthly(
            new DateOnly(2025, 1, 31), 4, new DayOfMonth(31), new Interval(1)));

        dates.Should().HaveCount(4);
        dates[0].Should().Be(new DateOnly(2025, 1, 31));
        dates[1].Day.Should().Be(28); // Feb 2025 has 28 days
        dates[2].Should().Be(new DateOnly(2025, 3, 31));
        dates[3].Day.Should().Be(30); // Apr has 30 days
    }

    [Fact]
    public void Monthly_ByDayOfMonth_ByCount_day29_feb_leap_year()
    {
        // 2024 is leap year, 2025 is not
        var dates = Collect(Recurrence.Monthly(
            new DateOnly(2024, 1, 29), 14, new DayOfMonth(29), new Interval(1)));

        dates.Should().HaveCount(14);
        // Feb 2024 (leap) — should be 29
        dates[1].Should().Be(new DateOnly(2024, 2, 29));
    }

    // ========================================================================
    // By DayOfMonth, by endDate
    // ========================================================================

    [Fact]
    public void Monthly_ByDayOfMonth_ByEndDate_within_range()
    {
        var begin = new DateOnly(2025, 1, 10);
        var end = new DateOnly(2025, 4, 10);

        var dates = Collect(Recurrence.Monthly(begin, end, new DayOfMonth(10), new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 10),
            new DateOnly(2025, 2, 10),
            new DateOnly(2025, 3, 10),
            new DateOnly(2025, 4, 10));
    }

    [Fact]
    public void Monthly_ByDayOfMonth_ByEndDate_does_not_exceed_endDate()
    {
        var begin = new DateOnly(2025, 1, 15);
        var end = new DateOnly(2025, 4, 14); // one day before April occurrence

        var dates = Collect(Recurrence.Monthly(begin, end, new DayOfMonth(15), new Interval(1)));

        dates.Should().AllSatisfy(d => d.Should().BeOnOrBefore(end));
    }

    /// <summary>
    /// CRITICAL: endDate in December — tests December month arithmetic.
    /// </summary>
    [Fact]
    public void Monthly_ByDayOfMonth_ByEndDate_ending_in_December()
    {
        var begin = new DateOnly(2025, 10, 1);
        var end = new DateOnly(2025, 12, 31);

        var dates = Collect(Recurrence.Monthly(begin, end, new DayOfMonth(1), new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 10, 1),
            new DateOnly(2025, 11, 1),
            new DateOnly(2025, 12, 1));
    }

    /// <summary>
    /// CRITICAL: Multi-year range ending in December.
    /// </summary>
    [Fact]
    public void Monthly_ByDayOfMonth_ByEndDate_multi_year_ending_in_December()
    {
        var begin = new DateOnly(2024, 6, 15);
        var end = new DateOnly(2025, 12, 15);

        var dates = Collect(Recurrence.Monthly(begin, end, new DayOfMonth(15), new Interval(1)));

        dates.Should().HaveCountGreaterThan(0);
        dates.First().Should().Be(new DateOnly(2024, 6, 15));
        dates.Last().Should().Be(new DateOnly(2025, 12, 15));
        dates.Should().BeInAscendingOrder();
    }

    [Fact]
    public void Monthly_ByDayOfMonth_ByEndDate_begin_eq_end()
    {
        var date = new DateOnly(2025, 6, 15);

        var dates = Collect(Recurrence.Monthly(date, date, new DayOfMonth(15), new Interval(1)));

        dates.Should().Equal(date);
    }

    // ========================================================================
    // By DayOfWeek + IndexOfDay, by count
    // ========================================================================

    [Fact]
    public void Monthly_ByDayOfWeek_ByCount_first_monday_of_every_month()
    {
        var begin = new DateOnly(2025, 1, 1);

        var dates = Collect(Recurrence.Monthly(
            begin, 4, DayOfWeek.Monday, IndexOfDay.First, new Interval(1)));

        dates.Should().HaveCount(4);
        dates.Should().AllSatisfy(d => d.DayOfWeek.Should().Be(DayOfWeek.Monday));
        dates.Should().AllSatisfy(d => d.Day.Should().BeLessThanOrEqualTo(7));
        dates.Should().BeInAscendingOrder();
    }

    [Fact]
    public void Monthly_ByDayOfWeek_ByCount_last_friday_of_every_month()
    {
        var begin = new DateOnly(2025, 1, 1);

        var dates = Collect(Recurrence.Monthly(
            begin, 3, DayOfWeek.Friday, IndexOfDay.Last, new Interval(1)));

        dates.Should().HaveCount(3);
        dates.Should().AllSatisfy(d => d.DayOfWeek.Should().Be(DayOfWeek.Friday));
        dates.Should().BeInAscendingOrder();
    }

    [Fact]
    public void Monthly_ByDayOfWeek_ByCount_with_interval()
    {
        var begin = new DateOnly(2025, 1, 1);

        var dates = Collect(Recurrence.Monthly(
            begin, 3, DayOfWeek.Wednesday, IndexOfDay.Second, new Interval(2)));

        dates.Should().HaveCount(3);
        dates.Should().AllSatisfy(d => d.DayOfWeek.Should().Be(DayOfWeek.Wednesday));
        dates.Should().BeInAscendingOrder();

        // With interval=2, months should be Jan, Mar, May
        dates[0].Month.Should().Be(1);
        dates[1].Month.Should().Be(3);
        dates[2].Month.Should().Be(5);
    }

    // ========================================================================
    // By DayOfWeek + IndexOfDay, by endDate
    // ========================================================================

    [Fact]
    public void Monthly_ByDayOfWeek_ByEndDate_within_range()
    {
        var begin = new DateOnly(2025, 1, 1);
        var end = new DateOnly(2025, 4, 30);

        var dates = Collect(Recurrence.Monthly(
            begin, end, DayOfWeek.Monday, IndexOfDay.First, new Interval(1)));

        dates.Should().HaveCountGreaterThan(0);
        dates.Should().AllSatisfy(d => d.Should().BeOnOrBefore(end));
        dates.Should().AllSatisfy(d => d.DayOfWeek.Should().Be(DayOfWeek.Monday));
        dates.Should().BeInAscendingOrder();
    }

    // ========================================================================
    // Enumerator advances correctly (regression for AddMonths bug)
    // ========================================================================

    /// <summary>
    /// Regression test: each MoveNext must produce a DIFFERENT month.
    /// </summary>
    [Fact]
    public void Monthly_ByDayOfMonth_each_date_is_in_a_different_month()
    {
        var dates = Collect(Recurrence.Monthly(
            new DateOnly(2025, 1, 10), 6, new DayOfMonth(10), new Interval(1)));

        // Each date should be in a distinct (year, month)
        var yearMonths = dates.Select(d => (d.Year, d.Month)).ToList();
        yearMonths.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void Monthly_ByDayOfMonth_months_advance_by_interval()
    {
        var dates = Collect(Recurrence.Monthly(
            new DateOnly(2025, 1, 1), 5, new DayOfMonth(1), new Interval(3)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 1),
            new DateOnly(2025, 4, 1),
            new DateOnly(2025, 7, 1),
            new DateOnly(2025, 10, 1),
            new DateOnly(2026, 1, 1));
    }

    // ========================================================================
    // Ordering & general invariants
    // ========================================================================

    [Fact]
    public void Monthly_all_dates_are_in_ascending_order()
    {
        var dates = Collect(Recurrence.Monthly(
            new DateOnly(2020, 1, 15), 50, new DayOfMonth(15), new Interval(1)));

        dates.Should().BeInAscendingOrder();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(6)]
    [InlineData(12)]
    public void Monthly_ByDayOfMonth_ByCount_various_intervals_produce_correct_count(int interval)
    {
        const int count = 10;
        var dates = Collect(Recurrence.Monthly(
            new DateOnly(2025, 1, 1), count, new DayOfMonth(1), new Interval(interval)));

        dates.Should().HaveCount(count);
        dates.Should().BeInAscendingOrder();
    }

    /// <summary>
    /// CRITICAL: Test specifically for every month (1..12) as the starting month
    /// to catch off-by-one errors in month arithmetic.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    public void Monthly_ByDayOfMonth_ByCount_starting_in_each_month_of_year(int startMonth)
    {
        var begin = new DateOnly(2025, startMonth, 1);

        var dates = Collect(Recurrence.Monthly(
            begin, 5, new DayOfMonth(1), new Interval(1)));

        dates.Should().HaveCount(5);
        dates.First().Should().Be(begin);
        dates.Should().BeInAscendingOrder();

        // Each date should be on the 1st
        dates.Should().AllSatisfy(d => d.Day.Should().Be(1));
    }

    /// <summary>
    /// CRITICAL: Start in December, interval=1 — next month must be January of next year.
    /// </summary>
    [Fact]
    public void Monthly_ByDayOfMonth_starting_December_advances_to_January()
    {
        var dates = Collect(Recurrence.Monthly(
            new DateOnly(2025, 12, 1), 3, new DayOfMonth(1), new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 12, 1),
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 2, 1));
    }
}
