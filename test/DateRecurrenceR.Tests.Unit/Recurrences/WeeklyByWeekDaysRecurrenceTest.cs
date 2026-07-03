using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(WeeklyByWeekDaysRecurrence))]
public class WeeklyByWeekDaysRecurrenceTest
{
    private static List<DateOnly> Collect(WeeklyByWeekDaysRecurrence recurrence)
    {
        var list = new List<DateOnly>();
        var enumerator = recurrence.GetEnumerator();
        while (enumerator.MoveNext()) list.Add(enumerator.Current);
        return list;
    }

    /// <summary>
    /// Regression: the count-based end date landed in an off-grid week when the last occurrence
    /// wraps past the end of the week with interval &gt; 1.
    /// </summary>
    [Fact]
    public void StopDate_by_count_with_interval_2_is_last_enumerated_date()
    {
        // Arrange
        var pattern = new WeeklyByWeekDaysPattern(
            new Interval(2),
            new WeekDays(DayOfWeek.Monday, DayOfWeek.Friday),
            DayOfWeek.Monday);
        var sut = WeeklyByWeekDaysRecurrence.New(new DateRange(new DateOnly(2026, 1, 1), 10), pattern);

        // Act
        var dates = Collect(sut);

        // Assert
        dates.Should().Equal(
            new DateOnly(2026, 1, 2),
            new DateOnly(2026, 1, 12),
            new DateOnly(2026, 1, 16),
            new DateOnly(2026, 1, 26),
            new DateOnly(2026, 1, 30),
            new DateOnly(2026, 2, 9),
            new DateOnly(2026, 2, 13),
            new DateOnly(2026, 2, 23),
            new DateOnly(2026, 2, 27),
            new DateOnly(2026, 3, 9));
        sut.StartDate.Should().Be(dates.First());
        sut.StopDate.Should().Be(dates.Last());
        sut.Count.Should().Be(dates.Count);
        sut.Contains(dates.Last()).Should().BeTrue();
    }

    [Theory]
    [InlineData(2, DayOfWeek.Monday, 1)]
    [InlineData(2, DayOfWeek.Monday, 2)]
    [InlineData(2, DayOfWeek.Monday, 9)]
    [InlineData(2, DayOfWeek.Monday, 10)]
    [InlineData(3, DayOfWeek.Sunday, 7)]
    [InlineData(3, DayOfWeek.Wednesday, 11)]
    [InlineData(4, DayOfWeek.Saturday, 13)]
    public void StopDate_by_count_with_interval_greater_than_1_is_last_enumerated_date(
        int interval, DayOfWeek firstDayOfWeek, int count)
    {
        // Arrange
        var pattern = new WeeklyByWeekDaysPattern(
            new Interval(interval),
            new WeekDays(DayOfWeek.Sunday, DayOfWeek.Tuesday, DayOfWeek.Friday),
            firstDayOfWeek);
        var sut = WeeklyByWeekDaysRecurrence.New(new DateRange(new DateOnly(2026, 1, 1), count), pattern);

        // Act
        var dates = Collect(sut);

        // Assert
        dates.Should().HaveCount(count);
        sut.StopDate.Should().Be(dates.Last());
        sut.Count.Should().Be(count);
    }

    /// <summary>
    /// Regression: the week-grid check in <see cref="WeeklyByWeekDaysRecurrence.Contains"/> accepted
    /// residue 7, the first day of an off-grid week.
    /// </summary>
    [Fact]
    public void Contains_rejects_days_of_off_grid_weeks()
    {
        // Arrange
        var pattern = new WeeklyByWeekDaysPattern(
            new Interval(2),
            new WeekDays(DayOfWeek.Monday, DayOfWeek.Friday),
            DayOfWeek.Monday);
        var sut = WeeklyByWeekDaysRecurrence.New(new DateRange(new DateOnly(2026, 1, 1), 10), pattern);

        // Assert
        // 2026-01-05 is a Monday in the off-grid week Jan 5..11: residue 7
        sut.Contains(new DateOnly(2026, 1, 5)).Should().BeFalse();
        // 2026-01-09 is a Friday in the same off-grid week: residue 11
        sut.Contains(new DateOnly(2026, 1, 9)).Should().BeFalse();
        // on-grid occurrences around the off-grid week
        sut.Contains(new DateOnly(2026, 1, 2)).Should().BeTrue();
        sut.Contains(new DateOnly(2026, 1, 12)).Should().BeTrue();
    }

    /// <summary>
    /// A selected day on the last day of the week has residue 6 (still on-grid) while the next day
    /// has residue 7 (off-grid): both sides of the boundary must be classified correctly.
    /// </summary>
    [Fact]
    public void Contains_boundary_residues_6_and_7()
    {
        // Arrange: weeks start on Monday, so Sunday is the last day (residue 6) of an on-grid week
        var pattern = new WeeklyByWeekDaysPattern(
            new Interval(2),
            new WeekDays(DayOfWeek.Sunday, DayOfWeek.Monday),
            DayOfWeek.Monday);
        var sut = WeeklyByWeekDaysRecurrence.New(new DateRange(new DateOnly(2026, 1, 5), 6), pattern);

        // Act
        var dates = Collect(sut);

        // Assert
        dates.Should().Equal(
            new DateOnly(2026, 1, 5),
            new DateOnly(2026, 1, 11),
            new DateOnly(2026, 1, 19),
            new DateOnly(2026, 1, 25),
            new DateOnly(2026, 2, 2),
            new DateOnly(2026, 2, 8));
        // 2026-01-11 is the on-grid Sunday: residue 6
        sut.Contains(new DateOnly(2026, 1, 11)).Should().BeTrue();
        // 2026-01-12 is the off-grid Monday right after it: residue 7
        sut.Contains(new DateOnly(2026, 1, 12)).Should().BeFalse();
    }

    [Theory]
    [InlineData(1, DayOfWeek.Monday)]
    [InlineData(2, DayOfWeek.Monday)]
    [InlineData(2, DayOfWeek.Sunday)]
    [InlineData(3, DayOfWeek.Thursday)]
    public void Contains_agrees_with_enumerator_for_every_date_in_range(int interval, DayOfWeek firstDayOfWeek)
    {
        // Arrange
        var pattern = new WeeklyByWeekDaysPattern(
            new Interval(interval),
            new WeekDays(DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Friday),
            firstDayOfWeek);
        var sut = WeeklyByWeekDaysRecurrence.New(new DateRange(new DateOnly(2026, 1, 1), 12), pattern);

        // Act
        var dates = Collect(sut);
        var expected = new HashSet<DateOnly>(dates);

        // Assert
        for (var date = dates.First().AddDays(-interval * 7); date <= dates.Last().AddDays(interval * 7); date = date.AddDays(1))
        {
            sut.Contains(date).Should().Be(expected.Contains(date), $"Contains({date:yyyy-MM-dd}) must agree with the enumerator");
        }
    }

    /// <summary>
    /// Regression: the endDate-based count included selected days of an off-grid week right after
    /// the last on-grid occurrence, so the enumerator ran past the end date.
    /// </summary>
    [Fact]
    public void ByEndDate_does_not_enumerate_beyond_endDate_when_endDate_is_in_off_grid_week()
    {
        // Arrange: weeks start on Monday; endDate 2026-01-12 is the off-grid Monday after Sun Jan 11
        var pattern = new WeeklyByWeekDaysPattern(
            new Interval(2),
            new WeekDays(DayOfWeek.Sunday, DayOfWeek.Monday),
            DayOfWeek.Monday);
        var sut = WeeklyByWeekDaysRecurrence.New(
            new DateRange(new DateOnly(2026, 1, 5), new DateOnly(2026, 1, 12)), pattern);

        // Act
        var dates = Collect(sut);

        // Assert
        dates.Should().Equal(new DateOnly(2026, 1, 5), new DateOnly(2026, 1, 11));
        sut.StopDate.Should().Be(dates.Last());
        sut.Count.Should().Be(dates.Count);
    }
}
