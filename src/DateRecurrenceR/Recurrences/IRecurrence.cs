namespace DateRecurrenceR.Recurrences;

public interface IRecurrence
{
    DateOnly StartDate { get; }
    DateOnly StopDate { get; }
    int Count { get; }

    bool Contains(DateOnly date);

    IRecurrence GetSubRange(int takeCount);
    IRecurrence GetSubRange(DateOnly fromDate, int takeCount);
    IRecurrence GetSubRange(DateOnly fromDate, DateOnly toDate);

    IEnumerator<DateOnly> GetEnumerator();
}