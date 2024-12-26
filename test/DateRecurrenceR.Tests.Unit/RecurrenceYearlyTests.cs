using DateRecurrenceR.Core;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit;

/// <summary>
/// Tests for Recurrence.Yearly — the public API entry point.
/// Covers by-dayOfYear, by-dayOfMonth+monthOfYear, and by-dayOfWeek+indexOfDay+monthOfYear,
/// with both by-count and by-endDate overloads.
/// </summary>
public sealed class RecurrenceYearlyTests
{
    private static List<DateOnly> Collect(IEnumerator<DateOnly> e)
    {
        var list = new List<DateOnly>();
        while (e.MoveNext()) list.Add(e.Current);
        return list;
    }

    // ========================================================================
    // By DayOfYear, by count
    // ========================================================================

    [Fact]
    public void Yearly_ByDayOfYear_ByCount_returns_empty_when_count_is_zero()
    {
        var dates = Collect(Recurrence.Yearly(
            new DateOnly(2025, 1, 1), 0, new DayOfYear(1), new Interval(1)));

        dates.Should().BeEmpty();
    }

    [Fact]
    public void Yearly_ByDayOfYear_ByCount_jan1_every_year()
    {
        var dates = Collect(Recurrence.Yearly(
            new DateOnly(2025, 1, 1), 4, new DayOfYear(1), new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 1),
            new DateOnly(2026, 1, 1),
            new DateOnly(2027, 1, 1),
            new DateOnly(2028, 1, 1));
    }

    [Fact]
    public void Yearly_ByDayOfYear_ByCount_day365_is_dec31()
    {
        var dates = Collect(Recurrence.Yearly(
            new DateOnly(2025, 1, 1), 3, new DayOfYear(365), new Interval(1)));

        dates.Should().HaveCount(3);
        dates[0].Should().Be(new DateOnly(2025, 12, 31));
        dates[1].Should().Be(new DateOnly(2026, 12, 31));
    }

    [Fact]
    public void Yearly_ByDayOfYear_ByCount_with_interval_2()
    {
        var dates = Collect(Recurrence.Yearly(
            new DateOnly(2020, 1, 1), 4, new DayOfYear(100), new Interval(2)));

        dates.Should().HaveCount(4);
        dates[0].Year.Should().Be(2020);
        dates[1].Year.Should().Be(2022);
        dates[2].Year.Should().Be(2024);
        dates[3].Year.Should().Be(2026);
    }

    [Fact]
    public void Yearly_ByDayOfYear_ByCount_with_fromDate()
    {
        var begin = new DateOnly(2020, 1, 1);
        var from = new DateOnly(2023, 1, 1);

        var dates = Collect(Recurrence.Yearly(begin, from, 3, new DayOfYear(1), new Interval(1)));

        dates.Should().HaveCount(3);
        dates[0].Year.Should().BeGreaterThanOrEqualTo(2023);
    }

    // ========================================================================
    // By DayOfYear, by endDate
    // ========================================================================

    [Fact]
    public void Yearly_ByDayOfYear_ByEndDate_within_range()
    {
        var begin = new DateOnly(2020, 1, 1);
        var end = new DateOnly(2024, 12, 31);

        var dates = Collect(Recurrence.Yearly(begin, end, new DayOfYear(1), new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2020, 1, 1),
            new DateOnly(2021, 1, 1),
            new DateOnly(2022, 1, 1),
            new DateOnly(2023, 1, 1),
            new DateOnly(2024, 1, 1));
    }

    /// <summary>
    /// CRITICAL: endDate is on the exact recurrence year but BEFORE recurrence day.
    /// The occurrence in that year should NOT be included.
    /// </summary>
    [Fact]
    public void Yearly_ByDayOfYear_ByEndDate_endDate_before_recurrence_day_excludes_year()
    {
        var begin = new DateOnly(2020, 1, 1);
        // Day 100 ~ April 10. endDate is day 99 in 2023
        var end = new DateOnly(2023, 4, 9); // before day 100

        var dates = Collect(Recurrence.Yearly(begin, end, new DayOfYear(100), new Interval(1)));

        // Should include 2020, 2021, 2022 but NOT 2023
        dates.Should().HaveCount(3);
        dates.Last().Year.Should().Be(2022);
    }

    /// <summary>
    /// CRITICAL: endDate is exactly ON the recurrence day.
    /// The occurrence should be included.
    /// </summary>
    [Fact]
    public void Yearly_ByDayOfYear_ByEndDate_endDate_on_recurrence_day_includes_it()
    {
        var begin = new DateOnly(2020, 1, 1);
        var end = new DateOnly(2023, 4, 10); // day 100

        var dates = Collect(Recurrence.Yearly(begin, end, new DayOfYear(100), new Interval(1)));

        dates.Should().HaveCount(4);
        dates.Last().Year.Should().Be(2023);
    }

    /// <summary>
    /// CRITICAL: endDate is on the recurrence year but AFTER recurrence day.
    /// The occurrence should be included.
    /// </summary>
    [Fact]
    public void Yearly_ByDayOfYear_ByEndDate_endDate_after_recurrence_day_includes_it()
    {
        var begin = new DateOnly(2020, 1, 1);
        var end = new DateOnly(2023, 4, 11); // after day 100

        var dates = Collect(Recurrence.Yearly(begin, end, new DayOfYear(100), new Interval(1)));

        dates.Should().HaveCount(4);
        dates.Last().Year.Should().Be(2023);
    }

    // ========================================================================
    // By DayOfMonth + MonthOfYear, by count
    // ========================================================================

    [Fact]
    public void Yearly_ByDayOfMonth_ByCount_jan1_every_year()
    {
        var dates = Collect(Recurrence.Yearly(
            new DateOnly(2025, 1, 1), 3,
            new DayOfMonth(1), new MonthOfYear(1), new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 1),
            new DateOnly(2026, 1, 1),
            new DateOnly(2027, 1, 1));
    }

    /// <summary>
    /// CRITICAL: December 25 every year — tests December month arithmetic in yearly path.
    /// </summary>
    [Fact]
    public void Yearly_ByDayOfMonth_ByCount_dec25_every_year()
    {
        var dates = Collect(Recurrence.Yearly(
            new DateOnly(2020, 1, 1), 5,
            new DayOfMonth(25), new MonthOfYear(12), new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2020, 12, 25),
            new DateOnly(2021, 12, 25),
            new DateOnly(2022, 12, 25),
            new DateOnly(2023, 12, 25),
            new DateOnly(2024, 12, 25));
    }

    /// <summary>
    /// CRITICAL: December 31 every 2 years.
    /// </summary>
    [Fact]
    public void Yearly_ByDayOfMonth_ByCount_dec31_every_2_years()
    {
        var dates = Collect(Recurrence.Yearly(
            new DateOnly(2020, 1, 1), 3,
            new DayOfMonth(31), new MonthOfYear(12), new Interval(2)));

        dates.Should().Equal(
            new DateOnly(2020, 12, 31),
            new DateOnly(2022, 12, 31),
            new DateOnly(2024, 12, 31));
    }

    [Fact]
    public void Yearly_ByDayOfMonth_ByCount_feb29_lands_on_leap_years()
    {
        // Recurrence for Feb 29 starting from leap year 2020
        // In non-leap years, day 29 in Feb should clamp to 28
        var dates = Collect(Recurrence.Yearly(
            new DateOnly(2020, 1, 1), 4,
            new DayOfMonth(29), new MonthOfYear(2), new Interval(1)));

        dates.Should().HaveCount(4);
        dates[0].Should().Be(new DateOnly(2020, 2, 29)); // leap
        dates[1].Should().Be(new DateOnly(2021, 2, 28)); // non-leap, clamped
        dates[2].Should().Be(new DateOnly(2022, 2, 28)); // non-leap, clamped
        dates[3].Should().Be(new DateOnly(2023, 2, 28)); // non-leap, clamped
    }

    // ========================================================================
    // By DayOfMonth + MonthOfYear, by endDate
    // ========================================================================

    [Fact]
    public void Yearly_ByDayOfMonth_ByEndDate_within_range()
    {
        var begin = new DateOnly(2020, 1, 1);
        var end = new DateOnly(2024, 7, 1);

        var dates = Collect(Recurrence.Yearly(
            begin, end, new DayOfMonth(15), new MonthOfYear(6), new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2020, 6, 15),
            new DateOnly(2021, 6, 15),
            new DateOnly(2022, 6, 15),
            new DateOnly(2023, 6, 15),
            new DateOnly(2024, 6, 15));
    }

    /// <summary>
    /// CRITICAL: endDate is on the exact recurrence year but BEFORE the recurrence date.
    /// The occurrence should NOT be included (the condition was previously inverted).
    /// </summary>
    [Fact]
    public void Yearly_ByDayOfMonth_ByEndDate_endDate_before_recurrence_excludes_it()
    {
        var begin = new DateOnly(2020, 1, 1);
        var end = new DateOnly(2023, 6, 14); // one day before June 15

        var dates = Collect(Recurrence.Yearly(
            begin, end, new DayOfMonth(15), new MonthOfYear(6), new Interval(1)));

        // Should include 2020, 2021, 2022 only
        dates.Should().HaveCount(3);
        dates.Last().Should().Be(new DateOnly(2022, 6, 15));
    }

    /// <summary>
    /// CRITICAL: endDate is exactly ON the recurrence date.
    /// The occurrence should be included.
    /// </summary>
    [Fact]
    public void Yearly_ByDayOfMonth_ByEndDate_endDate_on_recurrence_includes_it()
    {
        var begin = new DateOnly(2020, 1, 1);
        var end = new DateOnly(2023, 6, 15);

        var dates = Collect(Recurrence.Yearly(
            begin, end, new DayOfMonth(15), new MonthOfYear(6), new Interval(1)));

        dates.Should().HaveCount(4);
        dates.Last().Should().Be(new DateOnly(2023, 6, 15));
    }

    /// <summary>
    /// CRITICAL: endDate in December for yearly by dayOfMonth+month — tests
    /// the stopDate > endDate condition that was previously inverted.
    /// </summary>
    [Fact]
    public void Yearly_ByDayOfMonth_ByEndDate_December_endDate_before_occurrence()
    {
        var begin = new DateOnly(2020, 1, 1);
        var end = new DateOnly(2023, 12, 24); // before Dec 25

        var dates = Collect(Recurrence.Yearly(
            begin, end, new DayOfMonth(25), new MonthOfYear(12), new Interval(1)));

        // 2023 occurrence (Dec 25) is AFTER endDate (Dec 24), so excluded
        dates.Should().HaveCount(3);
        dates.Last().Should().Be(new DateOnly(2022, 12, 25));
    }

    [Fact]
    public void Yearly_ByDayOfMonth_ByEndDate_December_endDate_on_occurrence()
    {
        var begin = new DateOnly(2020, 1, 1);
        var end = new DateOnly(2023, 12, 25); // exactly Dec 25

        var dates = Collect(Recurrence.Yearly(
            begin, end, new DayOfMonth(25), new MonthOfYear(12), new Interval(1)));

        dates.Should().HaveCount(4);
        dates.Last().Should().Be(new DateOnly(2023, 12, 25));
    }

    // ========================================================================
    // By DayOfWeek + IndexOfDay + MonthOfYear, by count
    // ========================================================================

    [Fact]
    public void Yearly_ByDayOfWeek_ByCount_first_monday_of_january()
    {
        var dates = Collect(Recurrence.Yearly(
            new DateOnly(2025, 1, 1), 3,
            DayOfWeek.Monday, IndexOfDay.First, new MonthOfYear(1), new Interval(1)));

        dates.Should().HaveCount(3);
        dates.Should().AllSatisfy(d =>
        {
            d.DayOfWeek.Should().Be(DayOfWeek.Monday);
            d.Month.Should().Be(1);
            d.Day.Should().BeLessThanOrEqualTo(7);
        });
        dates.Should().BeInAscendingOrder();
    }

    /// <summary>
    /// CRITICAL: Last Friday of December every year.
    /// </summary>
    [Fact]
    public void Yearly_ByDayOfWeek_ByCount_last_friday_of_december()
    {
        var dates = Collect(Recurrence.Yearly(
            new DateOnly(2020, 1, 1), 4,
            DayOfWeek.Friday, IndexOfDay.Last, new MonthOfYear(12), new Interval(1)));

        dates.Should().HaveCount(4);
        dates.Should().AllSatisfy(d =>
        {
            d.DayOfWeek.Should().Be(DayOfWeek.Friday);
            d.Month.Should().Be(12);
        });
        dates.Should().BeInAscendingOrder();
    }

    // ========================================================================
    // By DayOfWeek + IndexOfDay + MonthOfYear, by endDate
    // ========================================================================

    [Fact]
    public void Yearly_ByDayOfWeek_ByEndDate_within_range()
    {
        var begin = new DateOnly(2020, 1, 1);
        var end = new DateOnly(2025, 12, 31);

        var dates = Collect(Recurrence.Yearly(
            begin, end, DayOfWeek.Monday, IndexOfDay.First, new MonthOfYear(3), new Interval(1)));

        dates.Should().HaveCountGreaterThan(0);
        dates.Should().AllSatisfy(d =>
        {
            d.Should().BeOnOrBefore(end);
            d.DayOfWeek.Should().Be(DayOfWeek.Monday);
            d.Month.Should().Be(3);
        });
        dates.Should().BeInAscendingOrder();
    }

    /// <summary>
    /// CRITICAL: endDate boundary test for dayOfWeek yearly — tests the
    /// stopDate > endDate condition with the dayOfWeek variant.
    /// </summary>
    [Fact]
    public void Yearly_ByDayOfWeek_ByEndDate_endDate_before_occurrence_excludes_it()
    {
        var begin = new DateOnly(2020, 1, 1);
        // First Monday of March 2023 is March 6. Set endDate to March 5.
        var end = new DateOnly(2023, 3, 5);

        var dates = Collect(Recurrence.Yearly(
            begin, end, DayOfWeek.Monday, IndexOfDay.First, new MonthOfYear(3), new Interval(1)));

        dates.Should().AllSatisfy(d => d.Should().BeOnOrBefore(end));
        dates.Last().Year.Should().Be(2022);
    }

    // ========================================================================
    // Ordering & general invariants
    // ========================================================================

    [Fact]
    public void Yearly_all_dates_are_in_ascending_order()
    {
        var dates = Collect(Recurrence.Yearly(
            new DateOnly(2000, 1, 1), 50, new DayOfYear(180), new Interval(1)));

        dates.Should().BeInAscendingOrder();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public void Yearly_ByDayOfYear_ByCount_years_advance_by_interval(int interval)
    {
        var dates = Collect(Recurrence.Yearly(
            new DateOnly(2000, 1, 1), 5, new DayOfYear(1), new Interval(interval)));

        for (var i = 1; i < dates.Count; i++)
        {
            (dates[i].Year - dates[i - 1].Year).Should().Be(interval);
        }
    }
}
