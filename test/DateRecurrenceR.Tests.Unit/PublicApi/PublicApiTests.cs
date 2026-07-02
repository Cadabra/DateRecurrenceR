using DateRecurrenceR.Core;
using FluentAssertions;

namespace DateRecurrenceR.Tests.Unit.PublicApi;

/// <summary>
/// Executes the public-API bundle documented in <c>TEST_CASES.md</c>. Each case id in the
/// theory maps 1:1 to a row in that document.
/// </summary>
public sealed class PublicApiTests
{
    private static List<DateOnly> Collect(IEnumerator<DateOnly> e)
    {
        var list = new List<DateOnly>();
        while (e.MoveNext()) list.Add(e.Current);
        return list;
    }

    public static IEnumerable<object[]> CaseIds =>
        PublicApiCases.All.Select(c => new object[] { c.Id });

    [Theory]
    [MemberData(nameof(CaseIds))]
    public void Recurrence_matches_expected_sequence(string id)
    {
        var @case = PublicApiCases.All.Single(c => c.Id == id);

        var actual = Collect(@case.Factory());

        actual.Should().Equal(@case.Expected, because: $"case {id} should match the published bundle");
    }
}
