using DateRecurrenceR.Core;
using DateRecurrenceR.Tests.Common;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(YearlyByDayOfWeekRecurrence))]
public class YearlyByDayOfWeekRecurrenceTests : RecurrenceContractTests<YearlyByDayOfWeekRecurrence>
{
    private static List<DateOnly> Collect(YearlyByDayOfWeekRecurrence recurrence)
    {
        var list = new List<DateOnly>();
        var enumerator = recurrence.GetEnumerator();
        while (enumerator.MoveNext()) list.Add(enumerator.Current);
        return list;
    }

    /// <summary>
    /// Regression: <see cref="YearlyByDayOfWeekRecurrence.New(DateRange, YearlyByDayOfWeekPattern)"/>
    /// converted the occurrence of the begin year to a day-of-year number and reused it for the
    /// actual start year, so <see cref="YearlyByDayOfWeekRecurrence.StartDate"/> pointed to a date
    /// that is not an occurrence and disagreed with the first enumerated date.
    /// </summary>
    [Fact]
    public void StartDate_equals_first_enumerated_date()
    {
        // Arrange: 2nd Tuesday of March every year, begin date is after March 2026.
        // The 2nd Tuesday of March 2027 is March 9.
        var pattern = new YearlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Tuesday, IndexOfDay.Second, new MonthOfYear(3));
        var sut = YearlyByDayOfWeekRecurrence.New(new DateRange(new DateOnly(2026, 4, 1), 3), pattern);

        // Act
        var dates = Collect(sut);

        // Assert
        dates.First().Should().Be(new DateOnly(2027, 3, 9), "it is the 2nd Tuesday of March 2027");
        sut.StartDate.Should().Be(dates.First(), "StartDate must be the first occurrence");
    }

    /// <summary>
    /// Regression: <see cref="YearlyByDayOfWeekRecurrence.Contains"/> ignored the index of the day,
    /// so any Friday of the matching month was reported as an occurrence.
    /// </summary>
    [Fact]
    public void Contains_rejects_matching_weekday_with_wrong_index()
    {
        // Arrange: 3rd Friday of June every year; June 2026 Fridays are 5, 12, 19, 26
        var pattern = new YearlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Friday, IndexOfDay.Third, new MonthOfYear(6));
        var sut = YearlyByDayOfWeekRecurrence.New(new DateRange(new DateOnly(2026, 1, 1), 5), pattern);

        // Assert
        sut.Contains(new DateOnly(2026, 6, 19)).Should().BeTrue("it is the 3rd Friday of June 2026");
        sut.Contains(new DateOnly(2026, 6, 12)).Should().BeFalse("it is the 2nd Friday of June 2026");
        sut.Contains(new DateOnly(2026, 6, 26)).Should().BeFalse("it is the 4th Friday of June 2026");
    }

    [Theory]
    [InlineData(IndexOfDay.First, 1)]
    [InlineData(IndexOfDay.Third, 1)]
    [InlineData(IndexOfDay.Last, 1)]
    [InlineData(IndexOfDay.Third, 2)]
    public void Contains_agrees_with_enumerator_for_every_date_in_range(IndexOfDay indexOfDay, int interval)
    {
        // Arrange
        var pattern = new YearlyByDayOfWeekPattern(new Interval(interval), DayOfWeek.Friday, indexOfDay, new MonthOfYear(6));
        var sut = YearlyByDayOfWeekRecurrence.New(new DateRange(new DateOnly(2026, 1, 1), 5), pattern);

        // Act
        var dates = Collect(sut);
        var expected = new HashSet<DateOnly>(dates);

        // Assert
        for (var date = dates.First().AddYears(-interval); date <= dates.Last().AddYears(interval); date = date.AddDays(1))
        {
            sut.Contains(date).Should().Be(expected.Contains(date), $"Contains({date:yyyy-MM-dd}) must agree with the enumerator");
        }
    }

    protected override YearlyByDayOfWeekRecurrence CreateByCount(DateOnly beginDate, int count)
    {
        return YearlyByDayOfWeekRecurrence.New(new DateRange(beginDate, count), new YearlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Tuesday, IndexOfDay.Second, new MonthOfYear(3)));
    }

    protected override YearlyByDayOfWeekRecurrence CreateByEndDate(DateOnly beginDate, DateOnly endDate)
    {
        return YearlyByDayOfWeekRecurrence.New(new DateRange(beginDate, endDate), new YearlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Tuesday, IndexOfDay.Second, new MonthOfYear(3)));
    }
}
