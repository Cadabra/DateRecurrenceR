using DateRecurrenceR.Collections;
using DateRecurrenceR.Internals;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Collections;

[TestSubject(typeof(YearlyEnumerator))]
public class YearlyEnumeratorTests : EnumeratorContractTests
{
    protected override IEnumerator<DateOnly> Create(DateOnly startDate, int takeCount)
    {
        return new YearlyEnumerator(startDate.Year, takeCount, interval: 1,
            YearlyDateResolver.ByDayOfMonth(startDate.Month, startDate.Day));
    }

    [Fact]
    public void Dates_advance_by_years()
    {
        var sut = new YearlyEnumerator(2026, 3, interval: 2,
            YearlyDateResolver.ByDayOfMonth(3, 10));

        Collector.Collect(sut).Should().Equal(
            new DateOnly(2026, 3, 10),
            new DateOnly(2028, 3, 10),
            new DateOnly(2030, 3, 10));
    }
}
