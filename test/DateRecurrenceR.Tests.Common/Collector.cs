using DateRecurrenceR.Recurrences;

namespace DateRecurrenceR.Tests.Common;

/// <summary>
/// Collects the dates produced by an enumerator or a recurrence into a list.
/// </summary>
public static class Collector
{
    public static List<DateOnly> Collect(IEnumerator<DateOnly> enumerator)
    {
        var list = new List<DateOnly>();
        while (enumerator.MoveNext()) list.Add(enumerator.Current);
        return list;
    }

    public static List<DateOnly> Collect(IRecurrence recurrence)
    {
        return Collect(recurrence.GetEnumerator());
    }
}
