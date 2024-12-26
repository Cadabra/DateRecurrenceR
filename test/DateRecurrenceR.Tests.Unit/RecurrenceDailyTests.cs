using DateRecurrenceR.Core;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit;

public sealed class RecurrenceDailyTests
{
    private static List<DateOnly> Collect(IEnumerator<DateOnly> e)
    {
        var list = new List<DateOnly>();
        while (e.MoveNext()) list.Add(e.Current);
        return list;
    }

    // --- by count ---

    [Fact]
    public void Daily_ByCount_returns_empty_when_count_is_zero()
    {
        var dates = Collect(Recurrence.Daily(new DateOnly(2025, 1, 1), 0, new Interval(1)));

        dates.Should().BeEmpty();
    }

    [Fact]
    public void Daily_ByCount_returns_single_date_when_count_is_one()
    {
        var begin = new DateOnly(2025, 6, 15);

        var dates = Collect(Recurrence.Daily(begin, 1, new Interval(1)));

        dates.Should().Equal(begin);
    }

    [Fact]
    public void Daily_ByCount_returns_consecutive_days_with_interval_1()
    {
        var begin = new DateOnly(2025, 1, 1);

        var dates = Collect(Recurrence.Daily(begin, 5, new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 1),
            new DateOnly(2025, 1, 2),
            new DateOnly(2025, 1, 3),
            new DateOnly(2025, 1, 4),
            new DateOnly(2025, 1, 5));
    }

    [Fact]
    public void Daily_ByCount_respects_interval()
    {
        var begin = new DateOnly(2025, 3, 1);

        var dates = Collect(Recurrence.Daily(begin, 4, new Interval(3)));

        dates.Should().Equal(
            new DateOnly(2025, 3, 1),
            new DateOnly(2025, 3, 4),
            new DateOnly(2025, 3, 7),
            new DateOnly(2025, 3, 10));
    }

    [Fact]
    public void Daily_ByCount_crosses_month_boundary()
    {
        var begin = new DateOnly(2025, 1, 30);

        var dates = Collect(Recurrence.Daily(begin, 3, new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 30),
            new DateOnly(2025, 1, 31),
            new DateOnly(2025, 2, 1));
    }

    [Fact]
    public void Daily_ByCount_crosses_year_boundary()
    {
        var begin = new DateOnly(2025, 12, 30);

        var dates = Collect(Recurrence.Daily(begin, 3, new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 12, 30),
            new DateOnly(2025, 12, 31),
            new DateOnly(2026, 1, 1));
    }

    [Fact]
    public void Daily_ByCount_handles_MaxValue_start()
    {
        var dates = Collect(Recurrence.Daily(DateOnly.MaxValue, 1, new Interval(1)));

        dates.Should().Equal(DateOnly.MaxValue);
    }

    [Fact]
    public void Daily_ByCount_with_fromDate_skips_to_correct_date()
    {
        var begin = new DateOnly(2025, 1, 1);
        var from = new DateOnly(2025, 1, 4);

        var dates = Collect(Recurrence.Daily(begin, from, 3, new Interval(3)));

        dates.Should().HaveCount(3);
        dates[0].Should().Be(new DateOnly(2025, 1, 4));
        dates[1].Should().Be(new DateOnly(2025, 1, 7));
    }

    // --- by endDate ---

    [Fact]
    public void Daily_ByEndDate_returns_dates_within_range()
    {
        var begin = new DateOnly(2025, 1, 1);
        var end = new DateOnly(2025, 1, 5);

        var dates = Collect(Recurrence.Daily(begin, end, new Interval(1)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 1),
            new DateOnly(2025, 1, 2),
            new DateOnly(2025, 1, 3),
            new DateOnly(2025, 1, 4),
            new DateOnly(2025, 1, 5));
    }

    [Fact]
    public void Daily_ByEndDate_with_interval_does_not_exceed_endDate()
    {
        var begin = new DateOnly(2025, 1, 1);
        var end = new DateOnly(2025, 1, 8);

        var dates = Collect(Recurrence.Daily(begin, end, new Interval(3)));

        dates.Should().Equal(
            new DateOnly(2025, 1, 1),
            new DateOnly(2025, 1, 4),
            new DateOnly(2025, 1, 7));
    }

    [Fact]
    public void Daily_ByEndDate_returns_single_when_begin_eq_end()
    {
        var date = new DateOnly(2025, 6, 15);

        var dates = Collect(Recurrence.Daily(date, date, new Interval(1)));

        dates.Should().Equal(date);
    }

    [Fact]
    public void Daily_all_dates_are_in_ascending_order()
    {
        var begin = new DateOnly(2025, 1, 1);

        var dates = Collect(Recurrence.Daily(begin, 100, new Interval(7)));

        dates.Should().BeInAscendingOrder();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(7)]
    [InlineData(30)]
    public void Daily_ByCount_all_intervals_are_exact(int interval)
    {
        var begin = new DateOnly(2025, 1, 1);

        var dates = Collect(Recurrence.Daily(begin, 10, new Interval(interval)));

        for (var i = 1; i < dates.Count; i++)
        {
            (dates[i].DayNumber - dates[i - 1].DayNumber).Should().Be(interval);
        }
    }
}
