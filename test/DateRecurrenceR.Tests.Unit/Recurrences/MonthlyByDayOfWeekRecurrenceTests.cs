using DateRecurrenceR.Core;
using DateRecurrenceR.Tests.Common;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(MonthlyByDayOfWeekRecurrence))]
public class MonthlyByDayOfWeekRecurrenceTests : RecurrenceContractTests<MonthlyByDayOfWeekRecurrence>
{
    private static List<DateOnly> Collect(MonthlyByDayOfWeekRecurrence recurrence)
    {
        var list = new List<DateOnly>();
        var enumerator = recurrence.GetEnumerator();
        while (enumerator.MoveNext()) list.Add(enumerator.Current);
        return list;
    }

    /// <summary>
    /// Regression: <see cref="MonthlyByDayOfWeekRecurrence.New(DateRange, MonthlyByDayOfWeekPattern)"/>
    /// resolved the day of the Nth weekday only for the begin month and reused that fixed day number
    /// in the actual start month. When the begin date was after the occurrence in its month, the
    /// recurrence started on a date that is not an occurrence of the pattern at all.
    /// </summary>
    [Fact]
    public void New_with_beginDate_after_occurrence_starts_at_next_real_occurrence()
    {
        // Arrange: 2nd Tuesday of every month; Jan 2026's is Jan 13, begin date is after it.
        // The 2nd Tuesdays that follow: Feb 10, Mar 10, Apr 14.
        var pattern = new MonthlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Tuesday, IndexOfDay.Second);
        var sut = MonthlyByDayOfWeekRecurrence.New(new DateRange(new DateOnly(2026, 1, 20), 3), pattern);

        // Act
        var dates = Collect(sut);

        // Assert
        dates.Should().Equal(
            new DateOnly(2026, 2, 10),
            new DateOnly(2026, 3, 10),
            new DateOnly(2026, 4, 14));
        sut.StartDate.Should().Be(new DateOnly(2026, 2, 10));
        dates.Should().OnlyContain(date => sut.Contains(date),
            "every enumerated date must be an occurrence of the pattern");
    }

    /// <summary>
    /// Regression: <see cref="MonthlyByDayOfWeekRecurrence.Contains"/> ignored the index of the day,
    /// so any Tuesday of a matching month was reported as an occurrence.
    /// </summary>
    [Fact]
    public void Contains_rejects_matching_weekday_with_wrong_index()
    {
        // Arrange: 2nd Tuesday of every month
        var pattern = new MonthlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Tuesday, IndexOfDay.Second);
        var sut = MonthlyByDayOfWeekRecurrence.New(new DateRange(new DateOnly(2026, 1, 1), 12), pattern);

        // Assert
        sut.Contains(new DateOnly(2026, 1, 13)).Should().BeTrue("it is the 2nd Tuesday of January");
        sut.Contains(new DateOnly(2026, 1, 6)).Should().BeFalse("it is the 1st Tuesday of January");
        sut.Contains(new DateOnly(2026, 1, 20)).Should().BeFalse("it is the 3rd Tuesday of January");
        sut.Contains(new DateOnly(2026, 1, 27)).Should().BeFalse("it is the 4th Tuesday of January");
    }

    [Fact]
    public void Contains_with_IndexOfDay_Last_accepts_only_last_weekday_of_month()
    {
        // Arrange: last Friday of every month; January 2026 has five Fridays (2, 9, 16, 23, 30)
        var pattern = new MonthlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Friday, IndexOfDay.Last);
        var sut = MonthlyByDayOfWeekRecurrence.New(new DateRange(new DateOnly(2026, 1, 1), 12), pattern);

        // Assert
        sut.Contains(new DateOnly(2026, 1, 30)).Should().BeTrue("it is the last Friday of January");
        sut.Contains(new DateOnly(2026, 1, 23)).Should().BeFalse("it is the 4th but not the last Friday of January");
        sut.Contains(new DateOnly(2026, 2, 27)).Should().BeTrue("it is the last (4th) Friday of February");
    }

    [Theory]
    [InlineData(IndexOfDay.First, 1)]
    [InlineData(IndexOfDay.Second, 1)]
    [InlineData(IndexOfDay.Last, 1)]
    [InlineData(IndexOfDay.Second, 2)]
    [InlineData(IndexOfDay.Last, 3)]
    public void Contains_agrees_with_enumerator_for_every_date_in_range(IndexOfDay indexOfDay, int interval)
    {
        // Arrange
        var pattern = new MonthlyByDayOfWeekPattern(new Interval(interval), DayOfWeek.Tuesday, indexOfDay);
        var sut = MonthlyByDayOfWeekRecurrence.New(new DateRange(new DateOnly(2026, 1, 1), 12), pattern);

        // Act
        var dates = Collect(sut);
        var expected = new HashSet<DateOnly>(dates);

        // Assert
        for (var date = dates.First().AddMonths(-interval);
             date <= dates.Last().AddMonths(interval);
             date = date.AddDays(1))
        {
            sut.Contains(date).Should().Be(expected.Contains(date),
                $"Contains({date:yyyy-MM-dd}) must agree with the enumerator");
        }
    }

    protected override MonthlyByDayOfWeekRecurrence CreateByCount(DateOnly beginDate, int count)
    {
        return MonthlyByDayOfWeekRecurrence.New(new DateRange(beginDate, count), new MonthlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Tuesday, IndexOfDay.Second));
    }

    protected override MonthlyByDayOfWeekRecurrence CreateByEndDate(DateOnly beginDate, DateOnly endDate)
    {
        return MonthlyByDayOfWeekRecurrence.New(new DateRange(beginDate, endDate), new MonthlyByDayOfWeekPattern(new Interval(1), DayOfWeek.Tuesday, IndexOfDay.Second));
    }
}
