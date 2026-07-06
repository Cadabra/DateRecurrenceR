using DateRecurrenceR.Collections;
using DateRecurrenceR.Internals;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Collections;

[TestSubject(typeof(MonthlyEnumerator))]
public class MonthlyEnumeratorTests : EnumeratorContractTests
{
    protected override IEnumerator<DateOnly> Create(DateOnly startDate, int takeCount)
    {
        return new MonthlyEnumerator(startDate, takeCount, interval: 1,
            MonthlyDateResolver.ByDayOfMonth(startDate.Day));
    }

    [Fact]
    public void Dates_advance_by_months_and_clamp_short_months()
    {
        var sut = new MonthlyEnumerator(new DateOnly(2026, 1, 31), 3, interval: 1,
            MonthlyDateResolver.ByDayOfMonth(31));

        Collector.Collect(sut).Should().Equal(
            new DateOnly(2026, 1, 31),
            new DateOnly(2026, 2, 28),
            new DateOnly(2026, 3, 31));
    }
}
