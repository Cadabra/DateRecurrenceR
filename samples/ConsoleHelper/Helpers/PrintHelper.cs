using System.Globalization;

namespace ConsoleHelper.Helpers;

public static class PrintHelper
{
    private const int WeekBorderSpace = 1;
    private const int MonthInLine = 4;
    private const int BorderSpace = 2;
    private const int WeekLineLength = 20;
    private static readonly DateTimeFormatInfo DateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;

    public static void PrintList(IEnumerable<DateOnly> list)
    {
        foreach (var dateOnly in list)
            Console.WriteLine(dateOnly);
    }

    public static void PrintCalendar(DateOnly startDate, DateOnly endDate, IList<DateOnly> list)
    {
        var monthPerRow = Math.Min(MonthInLine, 12);
        var printStartDate = GetPrintStartDate(startDate, monthPerRow);
        var monthsFromBegin = printStartDate.Year * 12 + printStartDate.Month;
        var num = endDate.Year * 12 + endDate.Month;
        Console.WriteLine(startDate);
        for (var year = startDate.Year; year <= endDate.Year; ++year)
        {
            WriteYear(year);
            for (var numberOfMonth = GetNumberOfMonth(monthsFromBegin);
                 numberOfMonth < 12;
                 numberOfMonth += monthPerRow)
            {
                var monthCount = Math.Min(12 - numberOfMonth / monthPerRow * monthPerRow, monthPerRow);
                PrintMonthHeaders(new DateOnly(year, numberOfMonth, 1), monthCount, startDate, endDate);
                PrintWeeks(new DateOnly(year, numberOfMonth, 1), monthCount, list, startDate, endDate);
            }

            Console.WriteLine();
            monthsFromBegin = monthsFromBegin + 12 - (monthsFromBegin - 1) % 12;
        }
    }

    private static DateOnly GetPrintStartDate(DateOnly startDate, int monthPerRow)
    {
        return startDate.AddMonths(-(startDate.Month - 1) % monthPerRow);
    }

    private static void PrintMonthHeaders(DateOnly startPrintDate,
        int monthCount,
        DateOnly startDate,
        DateOnly endDate)
    {
        for (var index = 0; index < monthCount; ++index)
            WriteMonth(startPrintDate.AddMonths(index), startDate, endDate);
        Console.WriteLine();
    }

    private static void PrintWeeks(DateOnly startPrintDate,
        int monthCount,
        IList<DateOnly> list,
        DateOnly startDate,
        DateOnly endDate)
    {
        for (var index1 = 0; index1 < 6; ++index1)
        {
            for (var index2 = 0; index2 < monthCount; ++index2)
            {
                var date = startPrintDate.AddMonths(index2);
                var month = date.Month;
                date = date.AddDays(-(int) date.DayOfWeek + 7 * index1);
                PrintOneWeekDays(month, date, startDate, endDate, list);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    private static void PrintOneWeekDays(int monthNumber,
        DateOnly date,
        DateOnly fromDate,
        DateOnly toDate,
        IList<DateOnly> selectedDates)
    {
        var num = 0;
        while (num < 7)
        {
            var count = num == 6 ? 2 : 1;
            int day;
            if (date.Month != monthNumber)
                Console.Write(new string(' ', count + 2));
            else if (date < fromDate || toDate < date)
            // else if (!DateOnly.op_GreaterThanOrEqual(date, fromDate) || !DateOnly.op_GreaterThanOrEqual(toDate, date))
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                day = date.Day;
                Console.Write(day.ToString("D2") + new string(' ', count));
                Console.ForegroundColor = foregroundColor;
            }
            else if (selectedDates.Contains(date))
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                day = date.Day;
                Console.Write(day.ToString("D2") + new string(' ', count));
                Console.ForegroundColor = foregroundColor;
            }
            else
            {
                day = date.Day;
                Console.Write(day.ToString("D2") + new string(' ', count));
            }

            ++num;
            date = date.AddDays(1);
        }
    }

    private static int GetNumberOfMonth(int monthsFromBegin) => (monthsFromBegin - 1) % 12 + 1;

    private static void WriteYear(int year)
    {
        var backgroundColor1 = Console.BackgroundColor;
        var foregroundColor1 = (int) Console.ForegroundColor;
        Console.ForegroundColor = backgroundColor1;
        Console.BackgroundColor = (ConsoleColor) foregroundColor1;
        Console.Write(year);
        var backgroundColor2 = Console.BackgroundColor;
        var foregroundColor2 = (int) Console.ForegroundColor;
        Console.ForegroundColor = backgroundColor2;
        Console.BackgroundColor = (ConsoleColor) foregroundColor2;
        Console.WriteLine();
    }

    private static void WriteMonth(DateOnly date, DateOnly startDate, DateOnly endDate)
    {
        var monthName = DateTimeFormat.GetMonthName(date.Month);
        if (startDate.Year == date.Year && startDate.Month <= date.Month ||
            startDate.Year < date.Year && date.Year < endDate.Year ||
            endDate.Year == date.Year && date.Month <= endDate.Month)
        {
            Console.Write(monthName.PadRight(22));
        }
        else
        {
            var foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(monthName.PadRight(22));
            Console.ForegroundColor = foregroundColor;
        }
    }
}