using DateRecurrenceR;
using DateRecurrenceR.Core;

const int subRangeYears = 5;
var dayOfYear = new DayOfYear(256);
var interval = new Interval(1);
var beginDate = DateOnly.MinValue;
var endDate = DateOnly.MaxValue;
var fromDate = DateOnly.FromDateTime(DateTime.Now);
var toDate = fromDate.AddYears(subRangeYears);

var enumerator = Recurrence.Yearly(beginDate, endDate, fromDate, toDate, dayOfYear, interval);

Console.WriteLine($"Current day of year is {fromDate.DayOfYear}");
Console.WriteLine($"Range {beginDate:M/d/yy} - {endDate:M/d/yy}");
Console.WriteLine($"Sub range {fromDate:M/d/yy} - {toDate:M/d/yy}");
while (enumerator.MoveNext()) Console.WriteLine($"{enumerator.Current:yyyy-M-d dddd} ({enumerator.Current.DayOfYear})");