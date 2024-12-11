namespace ConsoleHelper;

public class ConsoleCalendarOptions
{
    public int DayCharCount = 2;
    public required int MonthPerLine { get; init; }
    public required int CharsBetweenDays { get; init; }
    public required int CharsBetweenMonth { get; init; }
}