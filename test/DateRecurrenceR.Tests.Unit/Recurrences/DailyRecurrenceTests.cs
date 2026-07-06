using DateRecurrenceR.Core;
using DateRecurrenceR.Tests.Common;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(DailyRecurrence))]
public class DailyRecurrenceTests : RecurrenceContractTests<DailyRecurrence>
{
    private static List<DateOnly> Collect(DailyRecurrence recurrence)
    {
        var list = new List<DateOnly>();
        var enumerator = recurrence.GetEnumerator();
        while (enumerator.MoveNext()) list.Add(enumerator.Current);
        return list;
    }

    /// <summary>
    /// Regression: <see cref="DailyRecurrence.New(DateRange, DailyPattern)"/> returned a
    /// default-initialized instance for a <see cref="DateRange"/> with no end date and no count,
    /// discarding the pattern. The resulting zero interval made <see cref="DailyRecurrence.Contains"/>
    /// throw <see cref="DivideByZeroException"/> for <see cref="DateOnly.MinValue"/> and made
    /// every date-based sub-range throw as well.
    /// </summary>
    [Fact]
    public void New_with_default_DateRange_returns_empty_recurrence_that_never_throws()
    {
        // Arrange
        var sut = DailyRecurrence.New(default, new DailyPattern(new Interval(2)));

        // Assert
        sut.Count.Should().Be(0);
        Collect(sut).Should().BeEmpty();
        sut.Contains(DateOnly.MinValue).Should().BeFalse();
        sut.Contains(new DateOnly(2026, 1, 1)).Should().BeFalse();
        Collect(sut.GetSubRange(new DateOnly(2026, 1, 1), 3)).Should().BeEmpty(
            "a sub-range of an empty recurrence must be empty");
        Collect(sut.GetSubRange(3)).Should().BeEmpty();
    }

    /// <summary>
    /// Regression: <see cref="DailyRecurrence.GetSubRange(DateOnly, DateOnly)"/> ignored
    /// <c>fromDate</c> and returned a sub-range starting at the original start date.
    /// </summary>
    [Fact]
    public void GetSubRange_by_dates_starts_at_fromDate()
    {
        // Arrange: Jan 1, 3, 5, ..., 31
        var pattern = new DailyPattern(new Interval(2));
        var sut = DailyRecurrence.New(
            new DateRange(new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 31)), pattern);

        // Act
        var subRange = sut.GetSubRange(new DateOnly(2026, 1, 15), new DateOnly(2026, 1, 21));

        // Assert
        subRange.StartDate.Should().Be(new DateOnly(2026, 1, 15));
        Collect(subRange).Should().Equal(
            new DateOnly(2026, 1, 15),
            new DateOnly(2026, 1, 17),
            new DateOnly(2026, 1, 19),
            new DateOnly(2026, 1, 21));
    }

    /// <summary>
    /// Regression: <see cref="DailyRecurrence.GetSubRange(DateOnly, int)"/> re-anchored the series
    /// at <c>fromDate</c> without aligning it to the original occurrences, so the sub-range
    /// contained dates the original recurrence never produces.
    /// </summary>
    [Fact]
    public void GetSubRange_by_count_keeps_the_original_grid()
    {
        // Arrange: Jan 1, 3, 5, ..., 31; Jan 14 is not an occurrence
        var pattern = new DailyPattern(new Interval(2));
        var sut = DailyRecurrence.New(
            new DateRange(new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 31)), pattern);

        // Act
        var subRange = sut.GetSubRange(new DateOnly(2026, 1, 14), 3);

        // Assert
        Collect(subRange).Should().Equal(
            new DateOnly(2026, 1, 15),
            new DateOnly(2026, 1, 17),
            new DateOnly(2026, 1, 19));
        Collect(subRange).Should().OnlyContain(date => sut.Contains(date),
            "a sub-range must contain only dates of the original recurrence");
    }

    /// <summary>
    /// Regression: the count-based constructor clamped <c>Count</c> to the calendar limit
    /// but computed <c>StopDate</c> from the unclamped count, which threw
    /// <see cref="ArgumentOutOfRangeException"/> for large counts.
    /// </summary>
    [Fact]
    public void New_with_count_beyond_calendar_limit_clamps_instead_of_throwing()
    {
        // Arrange
        var pattern = new DailyPattern(new Interval(1));
        var range = new DateRange(new DateOnly(2026, 1, 1), int.MaxValue);

        // Act
        var act = () => DailyRecurrence.New(range, pattern);

        // Assert
        act.Should().NotThrow();
        var sut = act();
        sut.StopDate.Should().Be(DateOnly.MaxValue);
        sut.Count.Should().Be(DateOnly.MaxValue.DayNumber - new DateOnly(2026, 1, 1).DayNumber + 1);
    }

    [Fact]
    public void Enumerator_matches_Contains_by_endDate()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 1);
        var endDate = beginDate.AddDays(1000);
        var interval = new Interval(3);
        var range = new DateRange(beginDate, endDate);
        var pattern = new DailyPattern(interval);
        var enumerator = Recurrence.Daily(beginDate, endDate, interval);
        var sut = DailyRecurrence.New(range, pattern);

        // Act
        var list = new List<DateOnly>();
        var allContained = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            allContained &= sut.Contains(enumerator.Current);
        }

        // Assert
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        allContained.Should().BeTrue();
    }

    [Fact]
    public void Enumerator_matches_Contains_by_count()
    {
        // Arrange
        var beginDate = new DateOnly(1, 1, 1);
        var interval = new Interval(3);
        var count = 200;
        var range = new DateRange(beginDate, count);
        var pattern = new DailyPattern(interval);
        var enumerator = Recurrence.Daily(beginDate, count, interval);
        var sut = DailyRecurrence.New(range, pattern);

        // Act
        var list = new List<DateOnly>();
        var allContained = true;
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current);
            allContained &= sut.Contains(enumerator.Current);
        }

        // Assert
        sut.StartDate.Should().Be(list.First());
        sut.StopDate.Should().Be(list.Last());
        sut.Count.Should().Be(list.Count);
        allContained.Should().BeTrue();
    }

    protected override DailyRecurrence Create(DateRange range)
    {
        return DailyRecurrence.New(range, new DailyPattern(new Interval(2)));
    }

    /// <summary>Overflow guard: an end date before the start date yields an empty recurrence.</summary>
    [Fact]
    public void New_with_end_date_before_begin_date_is_empty()
    {
        var sut = DailyRecurrence.New(
            new DateRange(new DateOnly(2026, 1, 10), new DateOnly(2026, 1, 5)),
            new DailyPattern(new Interval(2)));

        sut.Count.Should().Be(0);
        Collect(sut).Should().BeEmpty();
    }

    /// <summary>
    /// TryAlignToGrid returns false when the aligned date would run past DateOnly.MaxValue,
    /// so the sub-range is empty.
    /// </summary>
    [Fact]
    public void GetSubRange_from_date_past_the_calendar_end_is_empty()
    {
        // Start one day before the calendar end with a five-day interval; aligning any later
        // fromDate to the grid overflows past DateOnly.MaxValue.
        var sut = DailyRecurrence.New(
            new DateRange(new DateOnly(9999, 12, 30), 1),
            new DailyPattern(new Interval(5)));

        Collect(sut.GetSubRange(new DateOnly(9999, 12, 31), 3)).Should().BeEmpty();
        Collect(sut.GetSubRange(new DateOnly(9999, 12, 31), new DateOnly(9999, 12, 31))).Should().BeEmpty();
    }
}
