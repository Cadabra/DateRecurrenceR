using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(YearlyByDayOfYearRecurrence))]
public class YearlyByDayOfYearRecurrenceTest
{
    private static List<DateOnly> Collect(YearlyByDayOfYearRecurrence recurrence)
    {
        var list = new List<DateOnly>();
        var enumerator = recurrence.GetEnumerator();
        while (enumerator.MoveNext()) list.Add(enumerator.Current);
        return list;
    }

    /// <summary>
    /// Regression: for day of year 366 the enumerator clamps to December 31 in non-leap years,
    /// but <see cref="YearlyByDayOfYearRecurrence.Contains"/> required the day of year to be
    /// exactly 366, so it rejected dates the enumerator itself produced.
    /// </summary>
    [Fact]
    public void Contains_accepts_clamped_day_366_in_non_leap_years()
    {
        // Arrange: day 366 every year, starting in the leap year 2024
        var pattern = new YearlyByDayOfYearPattern(new Interval(1), new DayOfYear(366));
        var sut = YearlyByDayOfYearRecurrence.New(new DateRange(new DateOnly(2024, 1, 1), 3), pattern);

        // Act
        var dates = Collect(sut);

        // Assert
        dates.Should().Equal(
            new DateOnly(2024, 12, 31),
            new DateOnly(2025, 12, 31),
            new DateOnly(2026, 12, 31));
        dates.Should().OnlyContain(date => sut.Contains(date),
            "Contains must accept every date the enumerator produces");
        sut.StopDate.Should().Be(dates.Last(),
            "StopDate must be the last occurrence, clamped the same way as the enumerator");
    }

    /// <summary>
    /// Regression: with a count of zero the yearly helper returned <c>StopDate == StartDate</c>,
    /// so <see cref="YearlyByDayOfYearRecurrence.Contains"/> reported the start date as an occurrence
    /// of a recurrence that enumerates nothing.
    /// </summary>
    [Fact]
    public void Contains_returns_false_for_every_date_when_count_is_zero()
    {
        // Arrange
        var pattern = new YearlyByDayOfYearPattern(new Interval(1), new DayOfYear(1));
        var sut = YearlyByDayOfYearRecurrence.New(new DateRange(new DateOnly(2026, 1, 1), 0), pattern);

        // Assert
        sut.Count.Should().Be(0);
        Collect(sut).Should().BeEmpty();
        sut.Contains(sut.StartDate).Should().BeFalse("an empty recurrence contains no dates");
    }
}
