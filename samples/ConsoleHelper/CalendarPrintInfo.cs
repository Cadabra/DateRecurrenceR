namespace ConsoleHelper;

public sealed class CalendarPrintInfo
{
    public required DateOnly BeginDate { get; init; }
    public required DateOnly EndDate { get; init; }
    public required DateOnly FromDate { get; init; }
    public required DateOnly ToDate { get; init; }
    public required int MonthPerLine { get; init; }
}