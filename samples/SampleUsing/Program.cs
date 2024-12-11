using ConsoleHelper;
using DateRecurrenceR;
using DateRecurrenceR.Core;

var interval = new Interval(1);
var firstDayOfWeek = Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
var beginDate = new DateOnly(2020, 1, 1);
var endDate = new DateOnly(2024, 12, 31);
var weekDays = new WeekDays(DayOfWeek.Tuesday, DayOfWeek.Friday);
// sub range equal recurrence range
var fromDate = beginDate;
var toDate = endDate;

var enumerator = Recurrence.Weekly(beginDate, endDate, fromDate, toDate, weekDays, firstDayOfWeek, interval);

// while (enumerator.MoveNext()) Console.WriteLine($"{enumerator.Current:yyyy-M-d dddd}");


new ConsoleCalendar(DayOfWeek.Sunday).Write(beginDate, endDate);
var list = new List<DateOnly>();

while (enumerator.MoveNext())
{
    list.Add(enumerator.Current);
}

// PrintHelper.PrintCalendar(beginDate, endDate, list);