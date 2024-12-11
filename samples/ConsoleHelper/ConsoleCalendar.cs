namespace ConsoleHelper;

public sealed class ConsoleCalendar
{
    private readonly DayOfWeek _firstDayOfWeek;
    private readonly ICalendarTextWriter _writer = (ICalendarTextWriter) new ConsoleCalendarTextWriter();

    private readonly ConsoleCalendarOptions _options = new ConsoleCalendarOptions()
    {
        MonthPerLine = 4,
        CharsBetweenDays = 1,
        CharsBetweenMonth = 2
    };

    public ConsoleCalendar(DayOfWeek firstDayOfWeek)
    {
        _firstDayOfWeek = firstDayOfWeek;
    }

    public void Write(DateOnly startDate, DateOnly endDate)
    {
        var yearEnumerator = new YearEnumerator(startDate, endDate, _firstDayOfWeek);
        while (yearEnumerator.MoveNext())
        {
            _writer.WriteYear(yearEnumerator.Current.Year);
            var current = yearEnumerator.Current;
            var flag = current.MoveNext();
            while (flag)
            {
                var weeks = new List<WeekEnumerator>();
                var num = 0;
                do
                {
                    weeks.Add(current.GetWeekEnumerator());

                    if (num != 0)
                    {
                        _writer.WriteMonthGap();
                    }

                    _writer.WriteMonth(current.Current.Month, current.Current.AffectedWeeks);
                    ++num;
                    flag = current.MoveNext();
                } while (flag && num % _options.MonthPerLine > 0);

                _writer.WriteLine();
                WriteWeekDaysLine(weeks);
                _writer.WriteLine();
                WriteWeeksLine(weeks);
            }

            _writer.WriteLine();
        }
    }

    private void WriteWeekDaysLine(IList<WeekEnumerator> weeks)
    {
        for (var j = 0; j < weeks.Count; ++j)
        {
            if (j != 0)
                _writer.WriteMonthGap();
            _writer.WriteWeekDays(_firstDayOfWeek);
        }
    }

    private void WriteWeeksLine(IList<WeekEnumerator> weeks)
    {
        var num = weeks.Max(w => w.AffectedWeeks);
        for (var i = 0; i < num; ++i)
        {
            for (var j = 0; j < weeks.Count; ++j)
            {
                weeks[j].MoveNext();
                if (j != 0)
                    _writer.WriteMonthGap();
                _writer.WriteWeek(weeks[j].GetDayEnumerator(_firstDayOfWeek));
            }

            _writer.WriteLine();
        }
    }
}