using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit.Helpers;

public sealed class MonthlyRecurrenceHelperTests
{
    // ========================================================================
    // TryGetStartDate
    // ========================================================================

    [Fact]
    public void TryGetStartDate_returns_true_when_fromDate_equals_beginDate()
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            DateOnly.MinValue, DateOnly.MinValue, 1, 1, out _);

        canStart.Should().BeTrue();
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
    public void TryGetStartDate_returns_false_at_MaxValue()
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            DateOnly.MaxValue, DateOnly.MaxValue, 1, 1, out _);

        canStart.Should().BeFalse();
    }

    [Fact]
    public void TryGetStartDate_returns_false_when_out_of_range()
    {
        var canStart = MonthlyRecurrenceHelper.TryGetStartDate(
            new DateOnly(9999, 12, 1), new DateOnly(9999, 12, 2), 1, 1, out _);

        canStart.Should().BeFalse();
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

    // ========================================================================
    // GetEndDateAndCount (by dayOfMonth, by count)
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

    /// <summary>
    /// CRITICAL: End date falling in December — tests the month/year arithmetic.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_ByCount_ending_in_December()
    {
        var start = new DateOnly(2025, 1, 1);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 1, 1, 12);

        count.Should().Be(12);
        endDate.Should().Be(new DateOnly(2025, 12, 1));
    }

    /// <summary>
    /// CRITICAL: Start in December, count=1 — the end IS December.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_ByCount_single_December()
    {
        var start = new DateOnly(2025, 12, 15);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 15, 1, 1);

        count.Should().Be(1);
        endDate.Should().Be(new DateOnly(2025, 12, 15));
    }

    /// <summary>
    /// CRITICAL: Start in December, count>1 — must cross into next year correctly.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_ByCount_starting_December_crossing_year()
    {
        var start = new DateOnly(2025, 12, 1);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 1, 1, 3);

        count.Should().Be(3);
        endDate.Should().Be(new DateOnly(2026, 2, 1));
    }

    /// <summary>
    /// CRITICAL: interval=12 starting in December — every occurrence is December.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_ByCount_interval_12_always_December()
    {
        var start = new DateOnly(2020, 12, 25);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 25, 12, 4);

        count.Should().Be(4);
        endDate.Should().Be(new DateOnly(2023, 12, 25));
    }

    // ========================================================================
    // GetEndDateAndCount (by dayOfMonth, by endDate)
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

    /// <summary>
    /// CRITICAL: End date in December.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_ByEndDate_ending_in_December()
    {
        var start = new DateOnly(2025, 1, 15);
        var end = new DateOnly(2025, 12, 15);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(start, 15, 1, end);

        count.Should().BeGreaterThan(0);
        endDate.Month.Should().Be(12);
        endDate.Year.Should().Be(2025);
    }

    // ========================================================================
    // GetEndDateAndCount (by dayOfWeek+indexOfDay, by count)
    // ========================================================================

    /// <summary>
    /// CRITICAL: December ending with dayOfWeek variant.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_DayOfWeek_ByCount_ending_in_December()
    {
        var start = new DateOnly(2025, 1, 6); // First Monday of Jan

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            start, DayOfWeek.Monday, IndexOfDay.First, 1, 12);

        count.Should().Be(12);
        endDate.Month.Should().Be(12);
        endDate.DayOfWeek.Should().Be(DayOfWeek.Monday);
    }

    // ========================================================================
    // GetEndDateAndCount (by dayOfWeek+indexOfDay, by endDate)
    // ========================================================================

    /// <summary>
    /// CRITICAL: December ending with dayOfWeek variant by endDate.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_DayOfWeek_ByEndDate_ending_in_December()
    {
        var start = new DateOnly(2025, 1, 6); // First Monday of Jan
        var end = new DateOnly(2025, 12, 31);

        var (endDate, count) = MonthlyRecurrenceHelper.GetEndDateAndCount(
            start, DayOfWeek.Monday, IndexOfDay.First, 1, end);

        count.Should().BeGreaterThan(0);
        endDate.Month.Should().Be(12);
        endDate.DayOfWeek.Should().Be(DayOfWeek.Monday);
    }
}
