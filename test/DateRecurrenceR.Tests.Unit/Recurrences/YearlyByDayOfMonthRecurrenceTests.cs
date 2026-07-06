using DateRecurrenceR.Core;
using DateRecurrenceR.Tests.Common;
using DateRecurrenceR.Recurrences;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Recurrences;

[TestSubject(typeof(YearlyByDayOfMonthRecurrence))]
public class YearlyByDayOfMonthRecurrenceTests : RecurrenceContractTests<YearlyByDayOfMonthRecurrence>
{
    private static List<DateOnly> Collect(YearlyByDayOfMonthRecurrence recurrence)
    {
        var list = new List<DateOnly>();
        var enumerator = recurrence.GetEnumerator();
        while (enumerator.MoveNext()) list.Add(enumerator.Current);
        return list;
    }

    /// <summary>
    /// Regression: <see cref="YearlyByDayOfMonthRecurrence.New(DateRange, YearlyByDayOfMonthPattern)"/>
    /// converted the month and day to a day-of-year number of the begin year and reused it for the
    /// actual start year. When the start year had a different leap status,
    /// <see cref="YearlyByDayOfMonthRecurrence.StartDate"/> shifted by one day and was not an occurrence.
    /// </summary>
    [Fact]
    public void StartDate_is_not_shifted_by_leap_year()
    {
        // Arrange: March 10 every year, begin date is after March 2027 (a non-leap year).
        // The next occurrence is 2028-03-10; 2028 is a leap year.
        var pattern = new YearlyByDayOfMonthPattern(new Interval(1), new DayOfMonth(10), new MonthOfYear(3));
        var sut = YearlyByDayOfMonthRecurrence.New(new DateRange(new DateOnly(2027, 4, 1), 3), pattern);

        // Act
        var dates = Collect(sut);

        // Assert
        dates.Should().Equal(
            new DateOnly(2028, 3, 10),
            new DateOnly(2029, 3, 10),
            new DateOnly(2030, 3, 10));
        sut.StartDate.Should().Be(new DateOnly(2028, 3, 10), "StartDate must be the first occurrence");
    }

    protected override YearlyByDayOfMonthRecurrence CreateByCount(DateOnly beginDate, int count)
    {
        return YearlyByDayOfMonthRecurrence.New(new DateRange(beginDate, count), new YearlyByDayOfMonthPattern(new Interval(1), new DayOfMonth(10), new MonthOfYear(3)));
    }

    protected override YearlyByDayOfMonthRecurrence CreateByEndDate(DateOnly beginDate, DateOnly endDate)
    {
        return YearlyByDayOfMonthRecurrence.New(new DateRange(beginDate, endDate), new YearlyByDayOfMonthPattern(new Interval(1), new DayOfMonth(10), new MonthOfYear(3)));
    }
}
