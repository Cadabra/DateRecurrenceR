using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(YearlyByDayOfWeekRecurrence))]
public class YearlyByDayOfWeekRecurrenceTest
{
    private static List<DateOnly> Collect(YearlyByDayOfWeekRecurrence recurrence)
    {
        var list = new List<DateOnly>();
        var enumerator = recurrence.GetEnumerator();
        while (enumerator.MoveNext()) list.Add(enumerator.Current);
        return list;
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
}
