using DateRecurrenceR.Collections;
using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Collections;

[TestSubject(typeof(WeeklyEnumerator))]
public class WeeklyEnumeratorTests : EnumeratorContractTests
{
    protected override IEnumerator<DateOnly> Create(DateOnly startDate, int takeCount)
    {
        var allDays = new WeekDays(true, true, true, true, true, true, true);
        var hash = WeeklyRecurrenceHelper.GetPatternHash(allDays, new Interval(1), DayOfWeek.Monday);

        return new WeeklyEnumerator(startDate, takeCount, hash);
    }

    [Fact]
    public void Dates_follow_the_selected_week_days()
    {
        // Mondays and Fridays, every week; 2026-01-05 is a Monday
        var weekDays = new WeekDays(DayOfWeek.Monday, DayOfWeek.Friday);
        var hash = WeeklyRecurrenceHelper.GetPatternHash(weekDays, new Interval(1), DayOfWeek.Monday);
        var sut = new WeeklyEnumerator(new DateOnly(2026, 1, 5), 4, hash);

        Collector.Collect(sut).Should().Equal(
            new DateOnly(2026, 1, 5),
            new DateOnly(2026, 1, 9),
            new DateOnly(2026, 1, 12),
            new DateOnly(2026, 1, 16));
    }
}
