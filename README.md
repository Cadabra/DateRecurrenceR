# DateRecurrenceR

[![Release](https://github.com/Cadabra/DateRecurrenceR/actions/workflows/release.yml/badge.svg)](https://github.com/Cadabra/DateRecurrenceR/actions/workflows/release.yml)  [![Qodana](https://github.com/Cadabra/DateRecurrenceR/actions/workflows/qodana_code_quality.yml/badge.svg)](https://github.com/Cadabra/DateRecurrenceR/actions/workflows/qodana_code_quality.yml) [![Nuget](https://img.shields.io/nuget/v/DateRecurrenceR?logo=NuGet)](https://www.nuget.org/packages/DateRecurrenceR)

_No AI, just math._

**DateRecurrenceR** is a .NET library for recurring dates: daily, weekly, monthly, and yearly schedules.
It calculates dates directly with math instead of looping day by day, so it stays fast even for series
that span many years.

## Key Features

* **Fast by design.** Getting the first date, the last date, or the total count of a series is an O(1)
  operation. The same is true for checking if a date belongs to the series. Nothing is enumerated.
* **Only the dates you need.** Generate any part of a series without generating the dates before it.
* **No time zones.** Built on `DateOnly`, so there are no time or localization problems.
* **Low memory use.** Recurrences are structs with struct enumerators. Enumeration does not allocate.
* **Compact string format.** Any recurrence can be saved as a short string like `20260101D2C10`
  and restored later.

## Installation

Install via [NuGet](https://www.nuget.org/packages/DateRecurrenceR):

```shell
dotnet add package DateRecurrenceR
```

or with the Package Manager Console:

```shell
Install-Package DateRecurrenceR
```

## Quick Start

```csharp
using DateRecurrenceR.Core;
using DateRecurrenceR.Recurrences;

// Every second Monday and Friday, starting 2026-01-01, 10 occurrences
var pattern = new WeeklyByWeekDaysPattern(
    new Interval(2),
    new WeekDays(DayOfWeek.Monday, DayOfWeek.Friday),
    firstDayOfWeek: DayOfWeek.Monday);

var recurrence = WeeklyByWeekDaysRecurrence.New(
    new DateRange(new DateOnly(2026, 1, 1), count: 10),
    pattern);

foreach (var date in recurrence)
{
    Console.WriteLine(date); // 2026-01-02, 2026-01-12, 2026-01-16, ...
}
```

## Recurrence Objects: `IRecurrence`

A recurrence object represents a whole series. It combines a pattern (for example, "every second Friday")
with a `DateRange`: a start date plus either an end date or a number of occurrences.
There is one struct per pattern kind: `DailyRecurrence`, `WeeklyByWeekDaysRecurrence`,
`MonthlyByDayOfMonthRecurrence`, and so on. All of them implement `IRecurrence`:

```csharp
public interface IRecurrence
{
    DateOnly StartDate { get; }
    DateOnly StopDate { get; }
    int Count { get; }

    bool Contains(DateOnly dateToCheck);

    IRecurrence GetSubRange(int takeCount);
    IRecurrence GetSubRange(DateOnly fromDate, int takeCount);
    IRecurrence GetSubRange(DateOnly fromDate, DateOnly toDate);

    IEnumerator<DateOnly> GetEnumerator();
}
```

### `StopDate` and `Count` are known immediately

Both values are calculated once, when the recurrence is created:

* If you set a **count**, the library also knows the date of the last occurrence (`StopDate`).
* If you set an **end date**, the library also knows how many occurrences fit (`Count`).

Reading these properties later costs nothing, because the answer is already there.
Questions like "when does this series end?" or "how many events are there?" do not depend
on how long the series is.

```csharp
recurrence.StartDate; // 2026-01-02, the first real occurrence
recurrence.StopDate;  // the date of the last occurrence
recurrence.Count;     // 10
```

### `Contains()` needs no loops

`Contains(date)` tells you if a date is part of the series. It is a direct calculation,
not a search through the dates. Checking a date against "every 3rd Tuesday for 100 years"
costs the same as checking it against "every day this week".

```csharp
recurrence.Contains(new DateOnly(2026, 1, 30)); // true, O(1)
```

### Sub-ranges and enumeration

`GetSubRange` returns a new, smaller recurrence, limited by count or by dates.
`GetEnumerator` lets you use a recurrence directly in `foreach`:

```csharp
foreach (var date in recurrence.GetSubRange(new DateOnly(2026, 2, 1), takeCount: 5))
{
    Console.WriteLine(date);
}
```

## Compact String Representation

Every pattern can be written as a short, culture-independent string. This is useful for storing
a recurrence in a database column, a URL, or a message.

### Pattern strings

Days of the week are digits from `0` (Sunday) to `6` (Saturday).
The index of a day in a month is a digit from `1` to `4`, or `L` for the last one.

| format                                    | meaning               | example          |
|-------------------------------------------|-----------------------|------------------|
| `D[interval]`                             | daily                 | `D2`             |
| `W[interval]D[days](S[firstDay])`         | weekly by week days   | `W2D15S1`        |
| `M[interval]D[dayOfMonth]`                | monthly by day        | `M1D15`          |
| `M[interval]I[index][dayOfWeek]`          | monthly by week day   | `M1I22`, `M1IL5` |
| `Y[interval]M[month]D[dayOfMonth]`        | yearly by day         | `Y1M3D10`        |
| `Y[interval]M[month]I[index][dayOfWeek]`  | yearly by week day    | `Y1M3I22`        |
| `Y[interval]D[dayOfYear]`                 | yearly by day of year | `Y1D100`         |

For example, `W2D15S1` means: **w**eekly, every **2** weeks, on **d**ays `1` and `5`
(Monday and Friday), the week **s**tarts on `1` (Monday).

### Full recurrence strings

A full string also contains the start date (`yyyyMMdd`) before the pattern, and an optional limit
after it. The limit is a count (`C[count]`) or an end date (`U[yyyyMMdd]`):

```text
20260101D2C10              every 2nd day from 2026-01-01, 10 occurrences
20260101W2D15S1U20261231   every 2nd Monday and Friday from 2026-01-01 until 2026-12-31
```

### Saving and restoring

Every pattern struct has `ToString()` / `Parse()`, and a full string can be turned back into
a recurrence with `Recurrence.FromString`:

```csharp
var pattern = new DailyPattern(new Interval(2));
var dateRange = new DateRange(new DateOnly(2026, 1, 1), count: 10);

pattern.ToString();          // "D2" (pattern only)
pattern.ToString(dateRange); // "20260101D2C10" (pattern + range, self-contained)
DailyPattern.Parse("D2");    // pattern back from a string

var recurrence = Recurrence.FromString("20260101D2C10");      // IRecurrence
var recurrence2 = Recurrence.FromString("D2", dateRange);     // pattern string + range

// non-throwing variants
if (Recurrence.TryFromString("20260101W2D15S1U20261231", out var parsed))
{
    // ...
}
```

All parsing methods also accept `ReadOnlySpan<char>`.

## Enumerator Methods

If you only need to iterate dates, the static `Recurrence` class returns an
`IEnumerator<DateOnly>` directly:

| method  | description                                                                                  |
|---------|----------------------------------------------------------------------------------------------|
| Daily   | Gets a daily recurrence by `interval`                                                        |
| Weekly  | Gets a weekly recurrence by `days of week` and `interval`                                    |
| Monthly | Gets a monthly recurrence by `days of week` or `day of month` and `interval`                 |
| Yearly  | Gets a yearly recurrence by `days of week` or `day of month` or `day of year` and `interval` |

```csharp
var enumerator = Recurrence.Daily(beginDate, endDate, fromDate, toDate, interval); // IEnumerator<DateOnly>
```

### Parameters

| parameter      | description                                                                                              |
|----------------|----------------------------------------------------------------------------------------------------------|
| beginDate      | The date when the recurrence begins.                                                                     |
| endDate        | The date when the recurrence ends.                                                                       |
| interval       | The interval between occurrences.                                                                        |
| fromDate       | The date when a specific range starts.                                                                   |
| toDate         | The date when a specific range finishes.                                                                 |
| count          | The maximum number of contiguous dates.                                                                  |
| dayOfWeek      | The day of the week.                                                                                     |
| dayOfMonth     | The day of the month. Takes the last day of the month if `dayOfMonth` is more than days in the month.    |
| dayOfYear      | The day of the year.                                                                                     |
| weekDays       | Days of the week.                                                                                        |
| indexOfDay     | Index of `dayOfWeek` in the month.                                                                       |
| numberOfMonth  | The number of the month.                                                                                 |
| firstDayOfWeek | The first day of the week.                                                                               |

### Sub-range rules

All enumerator methods support two ways to take a part of the series:

* **By dates** (`fromDate` and `toDate`): takes dates from the intersection
  `[beginDate, endDate] ∩ [fromDate, toDate]`.
* **By count** (`fromDate` and `count`): takes the first `count` dates starting from `fromDate`,
  within `[beginDate, endDate]`.
