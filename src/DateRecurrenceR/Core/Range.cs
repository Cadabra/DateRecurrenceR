namespace DateRecurrenceR.Core;

public readonly struct Range
{
    public Range(DateOnly beginDate)
    {
        BeginDate = beginDate;
        EndDate = DateOnly.MaxValue;
        Count = null;
    }

    public Range(DateOnly beginDate, DateOnly? endDate)
    {
        BeginDate = beginDate;
        EndDate = endDate;
        Count = null;
    }

    public Range(DateOnly beginDate, int count)
    {
        if (count < 0)
        {
            throw new ArgumentException();
        }

        BeginDate = beginDate;
        EndDate = null;
        Count = count;
    }

    public DateOnly BeginDate { get; }
    public DateOnly? EndDate { get; }
    public int? Count { get; }
}