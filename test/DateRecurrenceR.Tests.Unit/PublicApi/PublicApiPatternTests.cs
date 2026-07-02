using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit.PublicApi;

/// <summary>
/// Consistency checks for the pattern API (<c>Recurrences</c>): the reported
/// <c>StartDate</c>/<c>StopDate</c>/<c>Count</c> and <c>Contains</c> must agree with what
/// the enumerator actually yields.
/// </summary>
public sealed class PublicApiPatternTests
{
    private static List<DateOnly> Collect(IEnumerator<DateOnly> e)
    {
        var list = new List<DateOnly>();
        while (e.MoveNext()) list.Add(e.Current);
        return list;
    }

    [Fact]
    public void Yearly_recurrence_StopDate_and_Contains_agree_with_the_enumerator()
    {
        var pattern = new YearlyByDayOfYearPattern(new Interval(1), new DayOfYear(1));
        var recurrence = YearlyByDayOfYearRecurrence.New(
            new DateRange(new DateOnly(2020, 1, 1), 5), pattern);

        var yielded = Collect(recurrence.GetEnumerator());

        yielded.Should().HaveCount(5);
        recurrence.StartDate.Should().Be(yielded[0]);
        recurrence.StopDate.Should().Be(yielded[^1], because: "StopDate must be the last yielded occurrence");
        recurrence.Count.Should().Be(yielded.Count);
        recurrence.Contains(yielded[^1]).Should().BeTrue();
        recurrence.Contains(new DateOnly(2025, 1, 1)).Should().BeFalse(
            because: "the 6th occurrence is beyond count=5 and is never yielded");
    }
}
