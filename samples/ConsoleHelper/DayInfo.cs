namespace ConsoleHelper;

public sealed class DayInfo
{
    public DayInfo(int dayNumber, int month)
    {
        DayNumber = dayNumber;
        Month = month;
    }

    public int DayNumber { get; }
    public int Month { get; }
}