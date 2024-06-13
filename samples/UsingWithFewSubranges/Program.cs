using DateRecurrenceR;

var year = 999;
var firstDayOfWeek = DayOfWeek.Sunday;
var beginDate = new DateOnly(year, 1, 1);
var endDate = new DateOnly(year, 3, 31);
var fromDate = beginDate;
var toDate = endDate;
var weekDays = new WeekDays(DayOfWeek.Tuesday, DayOfWeek.Friday);

var fromDate1 = beginDate;
var toDate1 = new DateOnly(year, 1, 31);

var fromDate2 = new DateOnly(year, 2, 16);
var toDate2 = endDate;

var enumeratorFull = Recurrence.GetWeeklyEnumerator(beginDate, endDate, fromDate, toDate, weekDays, firstDayOfWeek, 2);

var enumeratorFirstMonth =
    Recurrence.GetWeeklyEnumerator(beginDate, endDate, fromDate1, toDate1, weekDays, firstDayOfWeek, 2);
var enumeratorSecondMonth =
    Recurrence.GetWeeklyEnumerator(beginDate, endDate, fromDate2, toDate2, weekDays, firstDayOfWeek, 2);

while (enumeratorFull.MoveNext()) Console.Write($"{enumeratorFull.Current:d} ");

Console.WriteLine();

Console.ForegroundColor = ConsoleColor.Cyan;
while (enumeratorFirstMonth.MoveNext()) Console.Write($"{enumeratorFirstMonth.Current:d} ");

// just beautify
Console.Write(new string(' ', 33));

Console.ForegroundColor = ConsoleColor.Magenta;
while (enumeratorSecondMonth.MoveNext()) Console.Write($"{enumeratorSecondMonth.Current:d} ");