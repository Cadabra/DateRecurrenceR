namespace DateRecurrenceR.Recurrences;

public interface IRecurrence<T, TEnum> : IRecurrence
    where T :  struct, IRecurrence
    where TEnum : struct, IEnumerator<DateOnly>
{
    T GetSubRange(int takeCount);
    T GetSubRange(DateOnly fromDate, int takeCount);
    T GetSubRange(DateOnly fromDate, DateOnly toDate);
    TEnum GetEnumerator();
}