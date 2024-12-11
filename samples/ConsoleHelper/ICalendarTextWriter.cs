namespace ConsoleHelper;

public interface ICalendarTextWriter
{
    void WriteYearTitle(int year);

    void WriteMonthTitle(int month);

    void WriteWeek(DayEnumerator dayEnumerator);

    void WriteWeekDays(DayOfWeek firstDayOfWeek);

    void WriteDay(DayInfo dayInfo);

    void WriteMonthGap();

    void WriteLine();
}