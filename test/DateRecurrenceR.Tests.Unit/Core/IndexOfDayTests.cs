using DateRecurrenceR.Core;
using FluentAssertions;
using JetBrains.Annotations;

namespace DateRecurrenceR.Tests.Unit.Core;

[TestSubject(typeof(IndexOfDay))]
public class IndexOfDayTests
{
    /// <summary>
    /// The numeric values are part of the contract: <c>Contains</c> implementations compare the
    /// index against <c>(day - 1) / 7</c>, and the pattern string format encodes them as digits.
    /// </summary>
    [Fact]
    public void Values_match_the_zero_based_week_index()
    {
        ((int)IndexOfDay.First).Should().Be(0);
        ((int)IndexOfDay.Second).Should().Be(1);
        ((int)IndexOfDay.Third).Should().Be(2);
        ((int)IndexOfDay.Fourth).Should().Be(3);
        ((int)IndexOfDay.Last).Should().Be(4);
    }
}
