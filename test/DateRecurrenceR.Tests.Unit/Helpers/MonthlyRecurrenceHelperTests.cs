using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Helpers;

[TestSubject(typeof(MonthlyRecurrenceHelper))]
public sealed class MonthlyRecurrenceHelperTests
{
    // ========================================================================
    // TryGetStartDate
    // ========================================================================

    [Fact]
    public void TryGetStartDate_returns_true_and_sets_startDate_when_fromDate_equals_beginDate()
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            new DateOnly(2025, 3, 15), new DateOnly(2025, 3, 15), 15, 1, out var startDate);

        canStart.Should().BeTrue();
        startDate.Should().Be(new DateOnly(2025, 3, 15));
    }

    [Fact]
    public void TryGetStartDate_returns_true_when_fromDate_before_dayOfMonth_in_same_month()
    {
        // fromDate.Day < dayOfMonth → lands on dayOfMonth in the same month
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10), 15, 1, out var startDate);

        canStart.Should().BeTrue();
        startDate.Should().Be(new DateOnly(2025, 1, 15));
    }

    [Fact]
    public void TryGetStartDate_returns_true_when_fromDate_on_dayOfMonth_boundary()
    {
        // fromDate.Day == dayOfMonth → must NOT skip to next month
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 15), 15, 1, out var startDate);

        canStart.Should().BeTrue();
        startDate.Should().Be(new DateOnly(2025, 1, 15));
    }

    [Fact]
    public void TryGetStartDate_skips_to_next_aligned_month_when_fromDate_past_day()
    {
        // beginDate=Jan 1, fromDate=Jan 20, dayOfMonth=15 → fromDate is past the 15th,
        // so should skip to Feb 15
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 20), 15, 1, out var startDate);

        canStart.Should().BeTrue();
        startDate.Should().Be(new DateOnly(2025, 2, 15));
    }

    [Fact]
    public void TryGetStartDate_returns_true_when_fromDate_after_beginDate()
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            new DateOnly(2025, 1, 1), new DateOnly(2025, 3, 1), 1, 1, out var startDate);

        canStart.Should().BeTrue();
        startDate.Should().Be(new DateOnly(2025, 3, 1));
    }

    [Fact]
    public void TryGetStartDate_with_interval_skips_to_next_aligned_month_when_fromDate_not_on_boundary()
    {
        // Aligned months from Jan with interval=3: Jan, Apr, Jul, ...
        // fromDate=Mar is between Jan and Apr → must skip to Apr
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            new DateOnly(2025, 1, 1), new DateOnly(2025, 3, 1), 1, 3, out var startDate);

        canStart.Should().BeTrue();
        startDate.Should().Be(new DateOnly(2025, 4, 1));
    }

    [Fact]
    public void TryGetStartDate_with_interval_lands_on_aligned_month_when_fromDate_is_on_boundary()
    {
        // fromDate=Apr 1 is exactly 3 months from beginDate=Jan 1 → land on Apr 1
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            new DateOnly(2025, 1, 1), new DateOnly(2025, 4, 1), 1, 3, out var startDate);

        canStart.Should().BeTrue();
        startDate.Should().Be(new DateOnly(2025, 4, 1));
    }

    [Fact]
    public void TryGetStartDate_with_interval_skips_past_day_on_aligned_month()
    {
        // fromDate=Apr 20 is exactly on the 3-month boundary but past dayOfMonth=15
        // → must advance one more interval to Jul 15
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            new DateOnly(2025, 1, 1), new DateOnly(2025, 4, 20), 15, 3, out var startDate);

        canStart.Should().BeTrue();
        startDate.Should().Be(new DateOnly(2025, 7, 15));
    }

    [Fact]
    public void TryGetStartDate_returns_false_at_MaxValue()
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            DateOnly.MaxValue, DateOnly.MaxValue, 1, 1, out _);

        canStart.Should().BeFalse();
    }

    [Fact]
    public void TryGetStartDate_returns_false_when_out_of_range()
    {
        // beginDate=Dec 9999, fromDate past dayOfMonth=1 → adding 1 month overflows
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            new DateOnly(9999, 12, 1), new DateOnly(9999, 12, 2), 1, 1, out _);

        canStart.Should().BeFalse();
    }

    // ========================================================================
    // GetEndDateAndCount — by dayOfMonth, by count
    // ========================================================================

    [Fact]
    public void GetEndDateAndCount_ByCount_basic()
    {
        var start = new DateOnly(2025, 1, 15);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 15, 1, 5);

        count.Should().Be(5);
        endDate.Should().Be(new DateOnly(2025, 5, 15));
    }

    [Fact]
    public void GetEndDateAndCount_ByCount_with_interval()
    {
        var start = new DateOnly(2025, 1, 1);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 1, 3, 4);

        count.Should().Be(4);
        endDate.Should().Be(new DateOnly(2025, 10, 1));
    }

    /// <summary>End date falling in December — tests the month/year arithmetic.</summary>
    [Fact]
    public void GetEndDateAndCount_ByCount_ending_in_December()
    {
        var start = new DateOnly(2025, 1, 1);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 1, 1, 12);

        count.Should().Be(12);
        endDate.Should().Be(new DateOnly(2025, 12, 1));
    }

    /// <summary>Start in December, count=1 — the end IS December.</summary>
    [Fact]
    public void GetEndDateAndCount_ByCount_single_December()
    {
        var start = new DateOnly(2025, 12, 15);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 15, 1, 1);

        count.Should().Be(1);
        endDate.Should().Be(new DateOnly(2025, 12, 15));
    }

    /// <summary>Start in December, count&gt;1 — must cross into next year correctly.</summary>
    [Fact]
    public void GetEndDateAndCount_ByCount_starting_December_crossing_year()
    {
        var start = new DateOnly(2025, 12, 1);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 1, 1, 3);

        count.Should().Be(3);
        endDate.Should().Be(new DateOnly(2026, 2, 1));
    }

    /// <summary>interval=12 starting in December — every occurrence is December.</summary>
    [Fact]
    public void GetEndDateAndCount_ByCount_interval_12_always_December()
    {
        var start = new DateOnly(2020, 12, 25);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 25, 12, 4);

        count.Should().Be(4);
        endDate.Should().Be(new DateOnly(2023, 12, 25));
    }

    /// <summary>dayOfMonth=31 landing in February — must clamp to Feb 28 (non-leap year).</summary>
    [Fact]
    public void GetEndDateAndCount_ByCount_dayOfMonth_31_clamps_in_February()
    {
        var start = new DateOnly(2025, 1, 31);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 31, 1, 2);

        count.Should().Be(2);
        endDate.Should().Be(new DateOnly(2025, 2, 28)); // 2025 is not a leap year
    }

    /// <summary>dayOfMonth=31 landing in April (30 days) — must clamp to Apr 30.</summary>
    [Fact]
    public void GetEndDateAndCount_ByCount_dayOfMonth_31_clamps_in_short_month()
    {
        var start = new DateOnly(2025, 3, 31);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 31, 1, 2);

        count.Should().Be(2);
        endDate.Should().Be(new DateOnly(2025, 4, 30));
    }

    /// <summary>safeCount clamped when requested count exceeds remaining months near MaxValue.</summary>
    [Fact]
    public void GetEndDateAndCount_ByCount_safeCount_is_clamped_near_MaxValue()
    {
        // Oct 9999: only 2 months left (Nov, Dec)
        var start = new DateOnly(9999, 10, 1);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 1, 1, 1000);

        count.Should().Be(2);
        endDate.Should().Be(new DateOnly(9999, 11, 1));
    }

    // ========================================================================
    // GetEndDateAndCount — by dayOfMonth, by endDate
    // ========================================================================

    [Fact]
    public void GetEndDateAndCount_ByEndDate_basic()
    {
        var start = new DateOnly(2025, 1, 1);
        var end = new DateOnly(2025, 4, 1);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 1, 1, end);

        count.Should().Be(4);
        endDate.Should().Be(new DateOnly(2025, 4, 1));
    }

    /// <summary>End date in December — exact count and exact date must match.</summary>
    [Fact]
    public void GetEndDateAndCount_ByEndDate_ending_in_December()
    {
        var start = new DateOnly(2025, 1, 15);
        var end = new DateOnly(2025, 12, 15);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 15, 1, end);

        count.Should().Be(12);
        endDate.Should().Be(new DateOnly(2025, 12, 15));
    }

    [Fact]
    public void GetEndDateAndCount_ByEndDate_with_interval()
    {
        // interval=3: Jan → Apr → Jul
        var start = new DateOnly(2025, 1, 1);
        var end = new DateOnly(2025, 7, 1);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 1, 3, end);

        count.Should().Be(3);
        endDate.Should().Be(new DateOnly(2025, 7, 1));
    }

    /// <summary>endDate is between two interval boundaries — result snaps back to last aligned month.</summary>
    [Fact]
    public void GetEndDateAndCount_ByEndDate_snaps_back_when_endDate_not_on_interval_boundary()
    {
        // interval=3 from Jan: aligned months are Jan, Apr, Jul, ...
        // endDate=May falls between Apr and Jul → snap back to Apr
        var start = new DateOnly(2025, 1, 1);
        var end = new DateOnly(2025, 5, 1);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 1, 3, end);

        count.Should().Be(2);
        endDate.Should().Be(new DateOnly(2025, 4, 1));
    }

    [Fact]
    public void GetEndDateAndCount_ByEndDate_crosses_year_boundary()
    {
        var start = new DateOnly(2025, 11, 15);
        var end = new DateOnly(2026, 2, 15);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 15, 1, end);

        count.Should().Be(4); // Nov, Dec, Jan, Feb
        endDate.Should().Be(new DateOnly(2026, 2, 15));
    }

    [Fact]
    public void GetEndDateAndCount_ByEndDate_crosses_year_boundary_excludes_last_month_when_endDate_before_dayOfMonth()
    {
        var start = new DateOnly(2025, 11, 15);
        var end = new DateOnly(2026, 2, 14);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 15, 1, end);

        count.Should().Be(3); // Nov, Dec, Jan — Feb 15 > Feb 14, so Feb is excluded
        endDate.Should().Be(new DateOnly(2026, 1, 15));
    }

    // ========================================================================
    // GetEndDateAndCount — by DayOfWeek + IndexOfDay, by count
    // ========================================================================

    [Fact]
    public void GetEndDateAndCount_DayOfWeek_ByCount_basic()
    {
        // First Monday of each month for 3 months starting Jan 2025
        // Jan 6 → Feb 3 → Mar 3
        var start = new DateOnly(2025, 1, 6);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            start, DayOfWeek.Monday, IndexOfDay.First, 1, 3);

        count.Should().Be(3);
        endDate.Should().Be(new DateOnly(2025, 3, 3));
    }

    /// <summary>December ending with dayOfWeek variant — exact date and exact count.</summary>
    [Fact]
    public void GetEndDateAndCount_DayOfWeek_ByCount_ending_in_December()
    {
        // First Monday of each month for 12 months from Jan 2025 → Dec 1 2025
        var start = new DateOnly(2025, 1, 6);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            start, DayOfWeek.Monday, IndexOfDay.First, 1, 12);

        count.Should().Be(12);
        endDate.Should().Be(new DateOnly(2025, 12, 1)); // First Monday of Dec 2025
        endDate.DayOfWeek.Should().Be(DayOfWeek.Monday);
    }

    [Fact]
    public void GetEndDateAndCount_DayOfWeek_ByCount_with_interval()
    {
        // First Monday every 3 months: Jan → Apr → Jul
        var start = new DateOnly(2025, 1, 6);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            start, DayOfWeek.Monday, IndexOfDay.First, 3, 3);

        count.Should().Be(3);
        endDate.Should().Be(new DateOnly(2025, 7, 7)); // First Monday of Jul 2025
        endDate.DayOfWeek.Should().Be(DayOfWeek.Monday);
    }

    /// <summary>IndexOfDay.Last exercises the conditional branch in DateOnlyHelper.</summary>
    [Fact]
    public void GetEndDateAndCount_DayOfWeek_ByCount_last_index_of_day()
    {
        // Last Friday: Jan 31 → Feb 28 → Mar 28
        var start = new DateOnly(2025, 1, 31);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            start, DayOfWeek.Friday, IndexOfDay.Last, 1, 3);

        count.Should().Be(3);
        endDate.Should().Be(new DateOnly(2025, 3, 28)); // Last Friday of Mar 2025
        endDate.DayOfWeek.Should().Be(DayOfWeek.Friday);
    }

    // ========================================================================
    // GetEndDateAndCount — by DayOfWeek + IndexOfDay, by endDate
    // ========================================================================

    [Fact]
    public void GetEndDateAndCount_DayOfWeek_ByEndDate_basic()
    {
        // First Monday Jan–Mar 2025
        var start = new DateOnly(2025, 1, 6);
        var end = new DateOnly(2025, 3, 31);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            start, DayOfWeek.Monday, IndexOfDay.First, 1, end);

        count.Should().Be(3);
        endDate.Should().Be(new DateOnly(2025, 3, 3)); // First Monday of Mar 2025
        endDate.DayOfWeek.Should().Be(DayOfWeek.Monday);
    }

    /// <summary>December ending with dayOfWeek variant by endDate — exact count and exact date.</summary>
    [Fact]
    public void GetEndDateAndCount_DayOfWeek_ByEndDate_ending_in_December()
    {
        var start = new DateOnly(2025, 1, 6);
        var end = new DateOnly(2025, 12, 31);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            start, DayOfWeek.Monday, IndexOfDay.First, 1, end);

        count.Should().Be(12);
        endDate.Should().Be(new DateOnly(2025, 12, 1)); // First Monday of Dec 2025
        endDate.DayOfWeek.Should().Be(DayOfWeek.Monday);
    }

    [Fact]
    public void GetEndDateAndCount_DayOfWeek_ByEndDate_with_interval()
    {
        // First Monday every 3 months: Jan → Apr → Jul (end=Sep)
        var start = new DateOnly(2025, 1, 6);
        var end = new DateOnly(2025, 9, 30);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            start, DayOfWeek.Monday, IndexOfDay.First, 3, end);

        count.Should().Be(3);
        endDate.Should().Be(new DateOnly(2025, 7, 7)); // First Monday of Jul 2025
        endDate.DayOfWeek.Should().Be(DayOfWeek.Monday);
    }

    /// <summary>endDate between interval boundaries — result snaps back to last aligned month.</summary>
    [Fact]
    public void GetEndDateAndCount_DayOfWeek_ByEndDate_snaps_back_when_endDate_not_on_interval_boundary()
    {
        // interval=3 from Jan: aligned months are Jan, Apr, Jul, ...
        // endDate=May falls between Apr and Jul → snap back to Apr
        var start = new DateOnly(2025, 1, 6);
        var end = new DateOnly(2025, 5, 31);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            start, DayOfWeek.Monday, IndexOfDay.First, 3, end);

        count.Should().Be(2); // Jan and Apr only
        endDate.Should().Be(new DateOnly(2025, 4, 7)); // First Monday of Apr 2025
        endDate.DayOfWeek.Should().Be(DayOfWeek.Monday);
    }

    /// <summary>IndexOfDay.Last exercises the conditional branch in DateOnlyHelper.</summary>
    [Fact]
    public void GetEndDateAndCount_DayOfWeek_ByEndDate_last_index_of_day()
    {
        // Last Friday Jan–Mar 2025
        var start = new DateOnly(2025, 1, 31);
        var end = new DateOnly(2025, 3, 31);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            start, DayOfWeek.Friday, IndexOfDay.Last, 1, end);

        count.Should().Be(3);
        endDate.Should().Be(new DateOnly(2025, 3, 28)); // Last Friday of Mar 2025
        endDate.DayOfWeek.Should().Be(DayOfWeek.Friday);
    }

    /// <summary>
    /// Occurrence in the last candidate month falls after endDate — that month must be excluded.
    /// First Monday of November 2025 = Nov 3, but endDate = Nov 2 → roll back to Oct 6.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_DayOfWeek_ByEndDate_excludes_month_when_occurrence_is_after_endDate()
    {
        var start = new DateOnly(2025, 1, 6); // First Monday of Jan 2025
        var end = new DateOnly(2025, 11, 2);  // before First Monday of Nov (Nov 3)

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            start, DayOfWeek.Monday, IndexOfDay.First, 1, end);

        count.Should().Be(10); // Jan through Oct
        endDate.Should().Be(new DateOnly(2025, 10, 6)); // First Monday of Oct 2025
        endDate.DayOfWeek.Should().Be(DayOfWeek.Monday);
    }
}
