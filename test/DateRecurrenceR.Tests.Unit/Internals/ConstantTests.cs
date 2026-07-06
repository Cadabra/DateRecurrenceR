using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Internals;

[TestSubject(typeof(DateRecurrenceR.Internals.Constant))]
public class ConstantTests
{
    [Fact]
    public void Calendar_constants_have_expected_values()
    {
        DaysInWeek.Should().Be(7);
        MonthsInYear.Should().Be(12);
    }
}
