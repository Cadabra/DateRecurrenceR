using DateRecurrenceR;
using DateRecurrenceR.Core;

var interval = new Interval(1);
var firstDayOfWeek = Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
var beginDate = new DateOnly(2000, 1, 1);
var endDate = new DateOnly(2000, 1, 31);
var weekDays = new WeekDays(DayOfWeek.Tuesday, DayOfWeek.Friday);
// sub range equal recurrence range
var fromDate = beginDate;
var toDate = endDate;

var enumerator = RecurrenceVal.Weekly(beginDate, endDate, fromDate, toDate, weekDays, firstDayOfWeek, interval);

while (enumerator.MoveNext()) Console.WriteLine($"{enumerator.Current:yyyy-M-d dddd}");