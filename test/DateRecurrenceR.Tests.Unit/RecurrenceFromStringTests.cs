using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit;

/// <summary>
/// Tests for <see cref="Recurrence.FromString(string)"/> and the <c>TryFromString</c> variants.
/// </summary>
[TestSubject(typeof(Recurrence))]
public sealed class RecurrenceFromStringTests
{
    [Fact]
    public void FromString_creates_the_same_recurrence_as_the_typed_factory()
    {
        var pattern = new WeeklyByWeekDaysPattern(
            new Interval(2),
            new WeekDays(DayOfWeek.Monday, DayOfWeek.Friday),
            DayOfWeek.Sunday);
        var dateRange = new DateRange(new DateOnly(2026, 1, 1), 10);
        var expected = WeeklyByWeekDaysRecurrence.New(dateRange, pattern);

        var actual = Recurrence.FromString(pattern.ToString(dateRange));

        actual.Should().BeOfType<WeeklyByWeekDaysRecurrence>();
        actual.StartDate.Should().Be(expected.StartDate);
        actual.StopDate.Should().Be(expected.StopDate);
        actual.Count.Should().Be(expected.Count);
        Collector.Collect(actual).Should().Equal(Collector.Collect(expected));
    }

    [Fact]
    public void FromString_with_until_creates_the_same_recurrence_as_the_typed_factory()
    {
        var pattern = new MonthlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Friday, IndexOfDay.Last);
        var dateRange = new DateRange(new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31));
        var expected = MonthlyByDayOfWeekRecurrence.New(dateRange, pattern);

        var actual = Recurrence.FromString(pattern.ToString(dateRange));

        actual.Should().BeOfType<MonthlyByDayOfWeekRecurrence>();
        Collector.Collect(actual).Should().Equal(Collector.Collect(expected));
    }

    [Fact]
    public void FromString_with_date_range_accepts_pattern_only_strings()
    {
        var pattern = new YearlyByDayOfYearPattern(new Interval(1), new DayOfYear(100));
        var dateRange = new DateRange(new DateOnly(2020, 1, 1), 5);
        var expected = YearlyByDayOfYearRecurrence.New(dateRange, pattern);

        var actual = Recurrence.FromString(pattern.ToString(), dateRange);

        actual.Should().BeOfType<YearlyByDayOfYearRecurrence>();
        Collector.Collect(actual).Should().Equal(Collector.Collect(expected));
    }

    [Fact]
    public void FromString_supports_every_pattern_kind()
    {
        var dateRange = new DateRange(new DateOnly(2026, 1, 1), 3);

        Recurrence.FromString("D1", dateRange)
            .Should().BeOfType<DailyRecurrence>();
        Recurrence.FromString("W1D1S0", dateRange)
            .Should().BeOfType<WeeklyByWeekDaysRecurrence>();
        Recurrence.FromString("M1D15", dateRange)
            .Should().BeOfType<MonthlyByDayOfMonthRecurrence>();
        Recurrence.FromString("M1I22", dateRange)
            .Should().BeOfType<MonthlyByDayOfWeekRecurrence>();
        Recurrence.FromString("Y1M3D10", dateRange)
            .Should().BeOfType<YearlyByDayOfMonthRecurrence>();
        Recurrence.FromString("Y1M3I22", dateRange)
            .Should().BeOfType<YearlyByDayOfWeekRecurrence>();
        Recurrence.FromString("Y1D100", dateRange)
            .Should().BeOfType<YearlyByDayOfYearRecurrence>();
    }

    [Theory]
    [InlineData("D2")] // no start date
    [InlineData("D2C10")] // no start date
    [InlineData("20260101D2C10U20261231")] // count and until are exclusive
    [InlineData("2026-01-01D2")] // wrong date format
    [InlineData("20261301D2")] // invalid month in start date
    [InlineData("20260101")] // no pattern
    [InlineData("20260101D2C")] // count without digits
    [InlineData("20260101D2U2026123")] // until date too short
    public void TryFromString_rejects_invalid_full_strings(string input)
    {
        Recurrence.TryFromString(input, out _).Should().BeFalse();
    }

    [Fact]
    public void TryFromString_with_date_range_rejects_strings_containing_range_parts()
    {
        var dateRange = new DateRange(new DateOnly(2026, 1, 1), 3);

        Recurrence.TryFromString("20260101D2", dateRange, out _).Should().BeFalse();
        Recurrence.TryFromString("D2C5", dateRange, out _).Should().BeFalse();
    }
}
