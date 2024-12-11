using System.Globalization;
using System.Runtime.CompilerServices;

namespace ConsoleHelper;

public sealed class ConsoleCalendarTextWriter : ICalendarTextWriter
{
    private static readonly DateTimeFormatInfo DateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;

    private readonly ConsoleCalendarOptions _options = new()
    {
        MonthPerLine = 4,
        CharsBetweenDays = 1,
        CharsBetweenMonth = 2
    };

    public void WriteYear(int year)
    {
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.White;
        Console.Write(year);
        Console.ResetColor();
        Console.WriteLine();
    }

    public void WriteMonth(int month, int weeks)
    {
        var monthName = DateTimeFormat.GetMonthName(month);
        var interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
        interpolatedStringHandler.AppendLiteral(" (");
        interpolatedStringHandler.AppendFormatted(weeks);
        interpolatedStringHandler.AppendLiteral(")");
        var stringAndClear = interpolatedStringHandler.ToStringAndClear();
        var str = monthName + stringAndClear;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.Gray;
        var totalWidth = _options.DayCharCount + (_options.DayCharCount + _options.CharsBetweenDays) * 6;
        Console.Write(str.PadRight(totalWidth));
        Console.ResetColor();
    }

    public void WriteWeek(DayEnumerator dayEnumerator)
    {
        for (var index = 0; index < 7; ++index)
        {
            if (index != 0)
                Console.Write(string.Empty.PadRight(_options.CharsBetweenDays));
            if (dayEnumerator.MoveNext())
                WriteDay(dayEnumerator.Current);
            else
                Console.Write("  ");
        }
    }

    public void WriteWeekDays(DayOfWeek firstDayOfWeek)
    {
        for (var i = 0; i < 7; ++i)
        {
            var dayIndex = (7 + (int) firstDayOfWeek + i) % 7;

            if (i != 0)
                Console.Write(string.Empty.PadRight(_options.CharsBetweenDays));

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedDayNames[dayIndex]
                .Substring(0, 2));
            Console.ResetColor();
        }
    }

    public void WriteDay(DayInfo dayInfo)
    {
        if (DateOnly.MinValue.DayNumber <= dayInfo.DayNumber && dayInfo.DayNumber <= DateOnly.MaxValue.DayNumber)
        {
            var dateOnly = DateOnly.FromDayNumber(dayInfo.DayNumber);
            if (dateOnly.Month == dayInfo.Month)
            {
                if (dateOnly.Day < 10)
                {
                    Console.Write(' ');
                }

                Console.Write(dateOnly.Day);
                // Console.Write(dateOnly.Day.ToString("D2"));
            }
            else
            {
                Console.Write("  ");
                // Console.ForegroundColor = ConsoleColor.DarkBlue;
                // Console.Write(dateOnly.Day.ToString("D2"));
                // Console.ResetColor();
            }
        }
        else
            Console.Write("  ");
    }

    public void WriteMonthGap()
    {
        Console.Write(string.Empty.PadRight(_options.CharsBetweenMonth));
    }

    public void WriteLine() => Console.WriteLine();
}