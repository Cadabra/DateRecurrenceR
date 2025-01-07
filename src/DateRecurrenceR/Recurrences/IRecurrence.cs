namespace DateRecurrenceR.Recurrences;

public interface IRecurrence
{
    DateOnly StartDate { get; }
    DateOnly StopDate { get; }
    int Count { get; }

    bool Contains(DateOnly date);
    IEnumerator<DateOnly> GetEnumerator();
    IEnumerator<DateOnly> GetEnumerator(int takeCount);
    IEnumerator<DateOnly> GetEnumerator(DateOnly fromDate, int takeCount);
    IEnumerator<DateOnly> GetEnumerator(DateOnly fromDate, DateOnly toDate);
}

public interface IRecurrence<T> : IRecurrence where T : IEnumerator<DateOnly>
{
    T GetEnumerator();
    T GetEnumerator(int takeCount);
    T GetEnumerator(DateOnly fromDate, int takeCount);
    T GetEnumerator(DateOnly fromDate, DateOnly toDate);
}