namespace ConsoleHelper;

public sealed class ConsoleCalendar
{
    private readonly DayOfWeek _firstDayOfWeek;
    private readonly ICalendarTextWriter _writer = new ConsoleCalendarTextWriter();

    private readonly ConsoleCalendarOptions _options = new()
    {
        MonthPerLine = 4,
        CharsBetweenDays = 1,
        CharsBetweenMonth = 2
    };

    public ConsoleCalendar(DayOfWeek firstDayOfWeek)
    {
        _firstDayOfWeek = firstDayOfWeek;
    }

    public void Write_Old(DateOnly startDate, DateOnly endDate)
    {
        var yearEnumerator = new YearEnumerator(startDate, endDate, _firstDayOfWeek);
        while (yearEnumerator.MoveNext())
        {
            _writer.WriteYearTitle(yearEnumerator.Current.Year);
            var monthEnumerable = yearEnumerator.Current;
            var flag = monthEnumerable.MoveNext();
            while (flag)
            {
                var weeks = new List<WeekEnumerator>();
                var num = 0;
                do
                {
                    weeks.Add(monthEnumerable.GetWeekEnumerator());

                    if (num != 0)
                    {
                        _writer.WriteMonthGap();
                    }

                    _writer.WriteMonthTitle(monthEnumerable.Current.Month);
                    ++num;
                    flag = monthEnumerable.MoveNext();
                } while (flag && num % _options.MonthPerLine > 0);

                _writer.WriteLine();
                WriteWeekDaysLine(weeks);

                _writer.WriteLine();
                WriteWeeksLine(weeks);
            }

            _writer.WriteLine();
        }
    }

    public void Write(DateOnly startDate, DateOnly endDate)
    {
        var yearEnumerator = new IntEnumerator(startDate, endDate);
        while (yearEnumerator.MoveNext())
        {
            _writer.WriteYearTitle(yearEnumerator.Current);

            var monthEnumerable = new IntMonthEnumerator(yearEnumerator.Current, startDate, endDate);

            WriteMonthsLine(yearEnumerator.Current, monthEnumerable, startDate, endDate);

            // var flag = monthEnumerable.MoveNext();
            // while (flag)
            // {
            //     var weeks = new List<WeekEnumerator>();
            //     var num = 0;
            //     do
            //     {
            //         weeks.Add(new WeekEnumerator(yearEnumerator.Current, monthEnumerable.Current, startDate, endDate, _firstDayOfWeek));
            //
            //         if (num != 0)
            //         {
            //             _writer.WriteMonthGap();
            //         }
            //
            //         _writer.WriteMonthTitle(monthEnumerable.Current);
            //         ++num;
            //         flag = monthEnumerable.MoveNext();
            //     } while (flag && num % _options.MonthPerLine > 0);
            //
            //     _writer.WriteLine();
            //     WriteWeekDaysLine(weeks);
            //
            //     _writer.WriteLine();
            //     WriteWeeksLine(weeks);
            // }

            _writer.WriteLine();
        }
    }

    private void WriteMonthsLine(int year, IntMonthEnumerator monthEnumerator, DateOnly startDate, DateOnly endDate)
    {
        var flag = monthEnumerator.MoveNext();
        while (flag)
        {
            var weeks = new List<WeekEnumerator>();

            for (var i = 0; flag && i < _options.MonthPerLine; i++)
            {
                weeks.Add(new WeekEnumerator(year, monthEnumerator.Current, startDate, endDate, _firstDayOfWeek));

                // if (i != 0)
                // {
                //     _writer.WriteMonthGap();
                // }
                //
                // _writer.WriteMonthTitle(monthEnumerator.Current);
                // flag = monthEnumerator.MoveNext();
            }

            for (var i = 0; flag && i < _options.MonthPerLine; i++)
            {
                if (i != 0)
                {
                    _writer.WriteMonthGap();
                }

                _writer.WriteMonthTitle(monthEnumerator.Current);
                flag = monthEnumerator.MoveNext();
            }

            _writer.WriteLine();
            WriteWeekDaysLine(weeks);

            _writer.WriteLine();
            WriteWeeksLine(weeks);

            _writer.WriteLine();
        }
    }

    private void WriteWeekDaysLine(List<WeekEnumerator> weeks)
    {
        for (var j = 0; j < weeks.Count; ++j)
        {
            if (j != 0)
                _writer.WriteMonthGap();
            _writer.WriteWeekDays(_firstDayOfWeek);
        }
    }

    private void WriteWeeksLine(List<WeekEnumerator> weeks)
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