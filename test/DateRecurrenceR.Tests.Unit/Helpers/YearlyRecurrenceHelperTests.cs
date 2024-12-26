using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit.Helpers;

public sealed class YearlyRecurrenceHelperTests
{
    // ========================================================================
    // TryGetStartDate
    // ========================================================================

    [Fact]
    public void TryGetStartDate_returns_true_when_fromDate_equals_beginDate()
    {
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            DateOnly.MinValue, DateOnly.MinValue, 1, 1, out _);

        canStart.Should().BeTrue();
    }

    [Fact]
    public void TryGetStartDate_returns_true_when_fromDate_after_beginDate()
    {
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            new DateOnly(2020, 1, 1), new DateOnly(2023, 6, 1), 100, 1, out var startDate);

        canStart.Should().BeTrue();
        startDate.Year.Should().BeGreaterThanOrEqualTo(2023);
    }

    [Fact]
    public void TryGetStartDate_returns_false_at_MaxValue()
    {
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            DateOnly.MaxValue, DateOnly.MaxValue, 1, 1, out _);

        canStart.Should().BeFalse();
    }

    [Fact]
    public void TryGetStartDate_returns_false_when_out_of_range()
    {
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            new DateOnly(9999, 1, 1), new DateOnly(9999, 1, 2), 1, 1, out _);

        canStart.Should().BeFalse();
    }

    [Fact]
    public void TryGetStartDate_skips_to_next_year_when_fromDate_past_dayOfYear()
    {
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            new DateOnly(2020, 1, 1), new DateOnly(2020, 6, 1), 100, 1, out var startDate);

        canStart.Should().BeTrue();
        startDate.Year.Should().Be(2021);
    }

    [Fact]
    public void TryGetStartDate_respects_interval()
    {
        var canStart = YearlyRecurrenceHelper.TryGetStartDate(
            new DateOnly(2020, 1, 1), new DateOnly(2021, 1, 1), 1, 3, out var startDate);

        canStart.Should().BeTrue();
        // From 2020 with interval=3, next aligned year >= 2021 is 2023
        startDate.Year.Should().Be(2023);
    }

    // ========================================================================
    // GetEndDateAndCount (by dayOfYear, by count)
    // ========================================================================

    [Fact]
    public void GetEndDateAndCount_DayOfYear_ByCount_basic()
    {
        var start = new DateOnly(2020, 1, 1);

        var (stopDate, count) = YearlyRecurrenceHelper.GetEndDateAndCount(start, 1, new Interval(1), 5);

        count.Should().Be(5);
        stopDate.Should().Be(new DateOnly(2025, 1, 1));
    }

    [Fact]
    public void GetEndDateAndCount_DayOfYear_ByCount_with_interval()
    {
        var start = new DateOnly(2020, 1, 1);

        var (stopDate, count) = YearlyRecurrenceHelper.GetEndDateAndCount(start, 1, new Interval(2), 3);

        count.Should().Be(3);
        stopDate.Should().Be(new DateOnly(2026, 1, 1));
    }

    // ========================================================================
    // GetEndDateAndCount (by dayOfYear, by endDate) — BOUNDARY TESTS
    // ========================================================================

    /// <summary>
    /// CRITICAL: endDate is on the recurrence year, but day is BEFORE recurrence day.
    /// Should NOT include that year.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_DayOfYear_ByEndDate_endDate_before_day_excludes()
    {
        var start = new DateOnly(2020, 4, 10); // day 100

        // endDate in 2023 but before day 100
        var end = new DateOnly(2023, 4, 9);

        var (_, count) = YearlyRecurrenceHelper.GetEndDateAndCount(start, 100, new Interval(1), end);

        count.Should().Be(3); // 2020, 2021, 2022 — day 100 of 2023 is after endDate (Apr 9), so 2023 excluded
    }

    /// <summary>
    /// CRITICAL: endDate is EXACTLY on recurrence day. Should include.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_DayOfYear_ByEndDate_endDate_on_day_includes()
    {
        var start = new DateOnly(2020, 4, 10); // day 100

        var end = new DateOnly(2023, 4, 10); // exactly day 100

        var (_, count) = YearlyRecurrenceHelper.GetEndDateAndCount(start, 100, new Interval(1), end);

        count.Should().Be(4); // 2020, 2021, 2022, 2023 all included
    }

    /// <summary>
    /// CRITICAL: endDate is AFTER recurrence day. Should include.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_DayOfYear_ByEndDate_endDate_after_day_includes()
    {
        var start = new DateOnly(2020, 4, 10); // day 100

        var end = new DateOnly(2023, 4, 11);

        var (_, count) = YearlyRecurrenceHelper.GetEndDateAndCount(start, 100, new Interval(1), end);

        count.Should().Be(4); // 2020, 2021, 2022, 2023 all included
    }

    // ========================================================================
    // GetEndDateAndCount (by dayOfMonth+monthOfYear, by count)
    // ========================================================================

    [Fact]
    public void GetEndDateAndCount_DayOfMonth_ByCount_basic()
    {
        var start = new DateOnly(2020, 6, 15);

        var (stopDate, count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            start, new MonthOfYear(6), new DayOfMonth(15), new Interval(1), 3);

        count.Should().Be(3);
        stopDate.Should().Be(new DateOnly(2023, 6, 15));
    }

    /// <summary>
    /// CRITICAL: December day — month 12.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_DayOfMonth_ByCount_December()
    {
        var start = new DateOnly(2020, 12, 25);

        var (stopDate, count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            start, new MonthOfYear(12), new DayOfMonth(25), new Interval(1), 4);

        count.Should().Be(4);
        stopDate.Should().Be(new DateOnly(2024, 12, 25));
    }

    // ========================================================================
    // GetEndDateAndCount (by dayOfMonth+monthOfYear, by endDate) — BOUNDARY
    // ========================================================================

    /// <summary>
    /// CRITICAL: endDate BEFORE the recurrence day in the aligned year.
    /// Should exclude that year (stopDate > endDate → decrement).
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_DayOfMonth_ByEndDate_endDate_before_occurrence_excludes()
    {
        var start = new DateOnly(2020, 6, 15);
        var end = new DateOnly(2023, 6, 14); // one day before recurrence

        var (stopDate, count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            start, new MonthOfYear(6), new DayOfMonth(15), new Interval(1), end);

        stopDate.Should().BeOnOrBefore(end);
        count.Should().Be(3); // 2020, 2021, 2022 — Jun 15 2023 is after endDate (Jun 14), so 2023 excluded
    }

    /// <summary>
    /// CRITICAL: endDate ON the recurrence day. Should include.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_DayOfMonth_ByEndDate_endDate_on_occurrence_includes()
    {
        var start = new DateOnly(2020, 6, 15);
        var end = new DateOnly(2023, 6, 15);

        var (stopDate, count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            start, new MonthOfYear(6), new DayOfMonth(15), new Interval(1), end);

        stopDate.Should().Be(new DateOnly(2023, 6, 15));
        count.Should().Be(4); // 2020, 2021, 2022, 2023 all included
    }

    /// <summary>
    /// CRITICAL: endDate AFTER the recurrence day. Should include.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_DayOfMonth_ByEndDate_endDate_after_occurrence_includes()
    {
        var start = new DateOnly(2020, 6, 15);
        var end = new DateOnly(2023, 6, 16);

        var (stopDate, count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            start, new MonthOfYear(6), new DayOfMonth(15), new Interval(1), end);

        stopDate.Should().Be(new DateOnly(2023, 6, 15));
        count.Should().Be(4); // 2020, 2021, 2022, 2023 all included
    }

    /// <summary>
    /// CRITICAL: December + endDate boundary combined.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_DayOfMonth_ByEndDate_December_before_occurrence()
    {
        var start = new DateOnly(2020, 12, 25);
        var end = new DateOnly(2023, 12, 24); // one day before

        var (stopDate, count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            start, new MonthOfYear(12), new DayOfMonth(25), new Interval(1), end);

        stopDate.Should().BeOnOrBefore(end);
    }

    [Fact]
    public void GetEndDateAndCount_DayOfMonth_ByEndDate_December_on_occurrence()
    {
        var start = new DateOnly(2020, 12, 25);
        var end = new DateOnly(2023, 12, 25);

        var (stopDate, count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            start, new MonthOfYear(12), new DayOfMonth(25), new Interval(1), end);

        stopDate.Should().Be(new DateOnly(2023, 12, 25));
    }

    // ========================================================================
    // GetEndDateAndCount (by dayOfWeek+indexOfDay+monthOfYear, by endDate)
    // ========================================================================

    /// <summary>
    /// CRITICAL: dayOfWeek variant with endDate before occurrence — same
    /// inverted condition bug applied here.
    /// </summary>
    [Fact]
    public void GetEndDateAndCount_DayOfWeek_ByEndDate_endDate_before_occurrence_excludes()
    {
        // First Monday of March 2020 is March 2
        var start = new DateOnly(2020, 3, 2);
        // First Monday of March 2023 is March 6. Set end to March 5.
        var end = new DateOnly(2023, 3, 5);

        var (stopDate, count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            start, new MonthOfYear(3), DayOfWeek.Monday, IndexOfDay.First, new Interval(1), end);

        stopDate.Should().BeOnOrBefore(end);
    }

    [Fact]
    public void GetEndDateAndCount_DayOfWeek_ByEndDate_endDate_after_occurrence_includes()
    {
        var start = new DateOnly(2020, 3, 2);
        var end = new DateOnly(2023, 3, 10);

        var (stopDate, count) = YearlyRecurrenceHelper.GetEndDateAndCount(
            start, new MonthOfYear(3), DayOfWeek.Monday, IndexOfDay.First, new Interval(1), end);

        stopDate.Should().BeOnOrBefore(end);
        stopDate.Year.Should().Be(2023);
    }
}
