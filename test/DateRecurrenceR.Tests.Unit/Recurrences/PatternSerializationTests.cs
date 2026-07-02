using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

/// <summary>
/// Tests for the compact string representation of the pattern structs
/// (<c>ToString</c>/<c>Parse</c>/<c>TryParse</c>) and the <see cref="Recurrence.FromString(string)"/> factory.
/// </summary>
public sealed class PatternSerializationTests
{
    private static List<DateOnly> Collect(IRecurrence recurrence)
    {
        var list = new List<DateOnly>();
        var enumerator = recurrence.GetEnumerator();
        while (enumerator.MoveNext()) list.Add(enumerator.Current);
        return list;
    }

    [Fact]
    public void Daily_pattern_ToString_produces_expected_string()
    {
        var pattern = new DailyPattern(new Interval(2));

        pattern.ToString().Should().Be("D2");
    }

    [Fact]
    public void Weekly_pattern_ToString_produces_expected_string()
    {
        var pattern = new WeeklyByWeekDaysPattern(
            new Interval(1),
            new WeekDays(DayOfWeek.Monday, DayOfWeek.Friday),
            DayOfWeek.Sunday);

        pattern.ToString().Should().Be("W1D15S0");
    }

    [Fact]
    public void MonthlyByDayOfMonth_pattern_ToString_produces_expected_string()
    {
        var pattern = new MonthlyByDayOfMonthPattern(new Interval(3), new DayOfMonth(15));

        pattern.ToString().Should().Be("M3D15");
    }

    [Fact]
    public void MonthlyByDayOfWeek_pattern_ToString_produces_expected_string()
    {
        var pattern = new MonthlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Tuesday, IndexOfDay.Second);

        pattern.ToString().Should().Be("M1I22");
    }

    [Fact]
    public void MonthlyByDayOfWeek_pattern_ToString_encodes_Last_as_L()
    {
        var pattern = new MonthlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Friday, IndexOfDay.Last);

        pattern.ToString().Should().Be("M1IL5");
    }

    [Fact]
    public void YearlyByDayOfMonth_pattern_ToString_produces_expected_string()
    {
        var pattern = new YearlyByDayOfMonthPattern(new Interval(1), new DayOfMonth(10), new MonthOfYear(3));

        pattern.ToString().Should().Be("Y1M3D10");
    }

    [Fact]
    public void YearlyByDayOfWeek_pattern_ToString_produces_expected_string()
    {
        var pattern = new YearlyByDayOfWeekPattern(
            new Interval(2),
            DayOfWeek.Thursday,
            IndexOfDay.Fourth,
            new MonthOfYear(11));

        pattern.ToString().Should().Be("Y2M11I44");
    }

    [Fact]
    public void YearlyByDayOfYear_pattern_ToString_produces_expected_string()
    {
        var pattern = new YearlyByDayOfYearPattern(new Interval(1), new DayOfYear(100));

        pattern.ToString().Should().Be("Y1D100");
    }

    [Fact]
    public void Daily_pattern_round_trips_through_Parse()
    {
        var source = new DailyPattern(new Interval(7));

        var parsed = DailyPattern.Parse(source.ToString());

        parsed.ToString().Should().Be(source.ToString());
    }

    [Fact]
    public void Weekly_pattern_round_trips_through_Parse()
    {
        var source = new WeeklyByWeekDaysPattern(
            new Interval(2),
            new WeekDays(DayOfWeek.Sunday, DayOfWeek.Wednesday, DayOfWeek.Saturday),
            DayOfWeek.Wednesday);

        var parsed = WeeklyByWeekDaysPattern.Parse(source.ToString());

        parsed.ToString().Should().Be(source.ToString());
    }

    [Fact]
    public void MonthlyByDayOfMonth_pattern_round_trips_through_Parse()
    {
        var source = new MonthlyByDayOfMonthPattern(new Interval(6), new DayOfMonth(31));

        var parsed = MonthlyByDayOfMonthPattern.Parse(source.ToString());

        parsed.ToString().Should().Be(source.ToString());
    }

    [Fact]
    public void MonthlyByDayOfWeek_pattern_round_trips_through_Parse()
    {
        var source = new MonthlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Sunday, IndexOfDay.Last);

        var parsed = MonthlyByDayOfWeekPattern.Parse(source.ToString());

        parsed.ToString().Should().Be(source.ToString());
    }

    [Fact]
    public void YearlyByDayOfMonth_pattern_round_trips_through_Parse()
    {
        var source = new YearlyByDayOfMonthPattern(new Interval(1), new DayOfMonth(29), new MonthOfYear(2));

        var parsed = YearlyByDayOfMonthPattern.Parse(source.ToString());

        parsed.ToString().Should().Be(source.ToString());
    }

    [Fact]
    public void YearlyByDayOfWeek_pattern_round_trips_through_Parse()
    {
        var source = new YearlyByDayOfWeekPattern(
            new Interval(4),
            DayOfWeek.Monday,
            IndexOfDay.First,
            new MonthOfYear(1));

        var parsed = YearlyByDayOfWeekPattern.Parse(source.ToString());

        parsed.ToString().Should().Be(source.ToString());
    }

    [Fact]
    public void YearlyByDayOfYear_pattern_round_trips_through_Parse()
    {
        var source = new YearlyByDayOfYearPattern(new Interval(1), new DayOfYear(366));

        var parsed = YearlyByDayOfYearPattern.Parse(source.ToString());

        parsed.ToString().Should().Be(source.ToString());
    }

    [Fact]
    public void Parse_is_case_insensitive_and_ignores_surrounding_whitespace()
    {
        YearlyByDayOfMonthPattern.Parse(" y1m3d10 ").ToString().Should().Be("Y1M3D10");
        MonthlyByDayOfWeekPattern.Parse("m1il5").ToString().Should().Be("M1IL5");
        WeeklyByWeekDaysPattern.Parse("w2d15s0").ToString().Should().Be("W2D15S0");
    }

    [Fact]
    public void Weekly_Parse_defaults_first_day_of_week_to_Monday_when_S_part_is_omitted()
    {
        var parsed = WeeklyByWeekDaysPattern.Parse("W1D2");

        parsed.FirstDayOfWeek.Should().Be(DayOfWeek.Monday);
    }

    [Fact]
    public void Weekly_Parse_accepts_days_in_any_order()
    {
        WeeklyByWeekDaysPattern.Parse("W1D51S0").ToString().Should().Be("W1D15S0");
    }

    [Theory]
    [InlineData("")]
    [InlineData("D")]
    [InlineData("D0")]
    [InlineData("D-1")]
    [InlineData("X2")]
    [InlineData("D2X")]
    [InlineData("D2C10")]
    [InlineData("20260101D2")]
    [InlineData("D99999999999")]
    public void Daily_TryParse_rejects_invalid_input(string input)
    {
        DailyPattern.TryParse(input, null, out _).Should().BeFalse();
    }

    [Theory]
    [InlineData("W1")]
    [InlineData("W1D")]
    [InlineData("W1D7")]
    [InlineData("W1D11")]
    [InlineData("W1D1S")]
    [InlineData("W1D1S7")]
    [InlineData("W1D1S0X")]
    [InlineData("W0D1")]
    public void Weekly_TryParse_rejects_invalid_input(string input)
    {
        WeeklyByWeekDaysPattern.TryParse(input, null, out _).Should().BeFalse();
    }

    [Theory]
    [InlineData("M1")]
    [InlineData("M1D0")]
    [InlineData("M1D32")]
    [InlineData("M1I")]
    [InlineData("M1I2")]
    [InlineData("M1I02")]
    [InlineData("M1I52")]
    [InlineData("M1I27")]
    [InlineData("M1IX2")]
    [InlineData("M1D15X")]
    public void Monthly_TryParse_rejects_invalid_input(string input)
    {
        MonthlyByDayOfMonthPattern.TryParse(input, null, out _).Should().BeFalse();
        MonthlyByDayOfWeekPattern.TryParse(input, null, out _).Should().BeFalse();
    }

    [Theory]
    [InlineData("Y1")]
    [InlineData("Y1M3")]
    [InlineData("Y1M13D1")]
    [InlineData("Y1M0D1")]
    [InlineData("Y1M3D0")]
    [InlineData("Y1M3D32")]
    [InlineData("Y1D0")]
    [InlineData("Y1D367")]
    [InlineData("Y1M3I52")]
    [InlineData("Y1M3D10X")]
    public void Yearly_TryParse_rejects_invalid_input(string input)
    {
        YearlyByDayOfMonthPattern.TryParse(input, null, out _).Should().BeFalse();
        YearlyByDayOfWeekPattern.TryParse(input, null, out _).Should().BeFalse();
        YearlyByDayOfYearPattern.TryParse(input, null, out _).Should().BeFalse();
    }

    [Fact]
    public void TryParse_rejects_a_pattern_string_of_a_different_kind()
    {
        DailyPattern.TryParse("W1D1", null, out _).Should().BeFalse();
        WeeklyByWeekDaysPattern.TryParse("D1", null, out _).Should().BeFalse();
        MonthlyByDayOfMonthPattern.TryParse("M1I22", null, out _).Should().BeFalse();
        MonthlyByDayOfWeekPattern.TryParse("M1D15", null, out _).Should().BeFalse();
        YearlyByDayOfMonthPattern.TryParse("Y1D100", null, out _).Should().BeFalse();
        YearlyByDayOfYearPattern.TryParse("Y1M3D10", null, out _).Should().BeFalse();
    }

    [Fact]
    public void Parse_throws_FormatException_for_invalid_input()
    {
        FluentActions.Invoking(() => DailyPattern.Parse("not a pattern"))
            .Should().Throw<FormatException>();
    }

    [Fact]
    public void ToString_with_count_range_appends_start_date_and_count()
    {
        var pattern = new DailyPattern(new Interval(2));
        var dateRange = new DateRange(new DateOnly(2026, 1, 1), 10);

        pattern.ToString(dateRange).Should().Be("20260101D2C10");
    }

    [Fact]
    public void ToString_with_end_date_range_appends_start_date_and_until()
    {
        var pattern = new DailyPattern(new Interval(2));
        var dateRange = new DateRange(new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31));

        pattern.ToString(dateRange).Should().Be("20260101D2U20261231");
    }

    [Fact]
    public void ToString_with_open_ended_range_appends_only_start_date()
    {
        var pattern = new DailyPattern(new Interval(2));
        var dateRange = new DateRange(new DateOnly(2026, 1, 1));

        pattern.ToString(dateRange).Should().Be("20260101D2");
    }

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
        Collect(actual).Should().Equal(Collect(expected));
    }

    [Fact]
    public void FromString_with_until_creates_the_same_recurrence_as_the_typed_factory()
    {
        var pattern = new MonthlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Friday, IndexOfDay.Last);
        var dateRange = new DateRange(new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31));
        var expected = MonthlyByDayOfWeekRecurrence.New(dateRange, pattern);

        var actual = Recurrence.FromString(pattern.ToString(dateRange));

        actual.Should().BeOfType<MonthlyByDayOfWeekRecurrence>();
        Collect(actual).Should().Equal(Collect(expected));
    }

    [Fact]
    public void FromString_with_date_range_accepts_pattern_only_strings()
    {
        var pattern = new YearlyByDayOfYearPattern(new Interval(1), new DayOfYear(100));
        var dateRange = new DateRange(new DateOnly(2020, 1, 1), 5);
        var expected = YearlyByDayOfYearRecurrence.New(dateRange, pattern);

        var actual = Recurrence.FromString(pattern.ToString(), dateRange);

        actual.Should().BeOfType<YearlyByDayOfYearRecurrence>();
        Collect(actual).Should().Equal(Collect(expected));
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
