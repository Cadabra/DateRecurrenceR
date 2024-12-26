namespace DateRecurrenceR.Recurrences;

public interface IRecurrence
{
    DateOnly StartDate { get; }
    DateOnly StopDate { get; }
    int Count { get; }

    bool Contains(DateOnly date);
    IEnumerator<DateOnly> GetEnumerator();
}