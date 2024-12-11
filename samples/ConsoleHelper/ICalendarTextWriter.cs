namespace ConsoleHelper;

public interface ICalendarTextWriter
{
    void WriteYear(int year);

    void WriteMonth(int month, int weeks);

    void WriteWeek(DayEnumerator dayEnumerator);

    void WriteWeekDays(DayOfWeek firstDayOfWeek);

    void WriteDay(DayInfo dayInfo);

    void WriteMonthGap();

    void WriteLine();
}