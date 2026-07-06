using DateRecurrenceR.Collections;
using DateRecurrenceR.Tests.Common;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Collections;

[TestSubject(typeof(DailyEnumerator))]
public class DailyEnumeratorTests : EnumeratorContractTests
{
    protected override IEnumerator<DateOnly> Create(DateOnly startDate, int takeCount)
    {
        return new DailyEnumerator(startDate, takeCount, interval: 2);
    }

    [Fact]
    public void Dates_advance_by_the_interval()
    {
        var sut = new DailyEnumerator(new DateOnly(2026, 1, 1), 3, interval: 2);

        Collector.Collect(sut).Should().Equal(
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 3),
            new DateOnly(2026, 1, 5));
    }
}
