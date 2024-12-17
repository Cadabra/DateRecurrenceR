# DateRecurrenceR

[![Release](https://github.com/Cadabra/DateRecurrenceR/actions/workflows/release.yml/badge.svg)](https://github.com/Cadabra/DateRecurrenceR/actions/workflows/release.yml) [![Nuget](https://img.shields.io/nuget/v/DateRecurrenceR?logo=NuGet)](https://www.nuget.org/packages/DateRecurrenceR)

## Efficient Date Recurrence Library for .NET

**DateRecurrenceR** is a .NET library designed to handle and manage recurrence date patterns efficiently.

### Key Features

* **Dynamic Subrange Generation:** Generate only the needed dates within a specified range, optimizing performance and memory usage.
* **Time Localization Simplification:** Uses DateOnly, eliminating issues related to time localization.
* **Flexible Collection Handling:** Returns an enumerator, allowing developers to choose the collection type.
* **Optimal Performance:** Boasts a Big O notation of O(1), ensuring efficient operations.

### Methods for Recurrences

| method  | description                                                                                   |
|---------|-----------------------------------------------------------------------------------------------|
| Daily   | Gets a daily recurrence by `interval`                                                         |
| Weekly  | Gets a weekly recurrence by `days of week` and `interval`                                     |
| Monthly | Gets a monthly recurrence by `days of week` or `day of month` and `interval`                  |
| Yearly  | Gets an yearly recurrence by `days of week` or `day of month` or `day of year` and `interval` |

All these methods return an instance of `IEnumerator<DateOnly>`.

### All parameters

| parameter      | description                                                                                               |
|----------------|-----------------------------------------------------------------------------------------------------------|
| beginDate      | The date when the recurrence begins.                                                                      |
| endDate        | The date when the recurrence ends.                                                                        |
| interval       | The interval between occurrences.                                                                         |
| fromDate       | The date when a specific range starts.                                                                    |
| toDate         | The date when a specific range finishes.                                                                  |
| count          | The maximum number of contiguous dates.                                                                   |
| dayOfWeek      | The day of the week.                                                                                      |
| dayOfMonth     | The day of the month. Takes the last day of the month if `dayOfMonth` is more than the days in the month. |
| dayOfYear      | The day of the year.                                                                                      |
| weekDays       | Days of the week.                                                                                         |
| numberOfWeek   | Index of dayOfWeek in the month.                                                                          |
| numberOfMonth  | The number of the month.                                                                                  |
| firstDayOfWeek | The first day of the week.                                                                                |

### Subranges

All methods for recurrences support the following subrange rules:

#### By dates

* **Parameters:** `fromDate` and `toDate`
* **Rule:** Takes dates from the intersection `[beginDate, endDate] ∪ [fromDate, toDate]`

#### By count

* **Parameters:** `fromDate` and `count`
* **Rule:** Takes dates from the intersection `[beginDate, endDate] ∪ [fromDate, DateOnly.MaxValue]` and takes the
  first `count` dates

## Examples of Use

```csharp
var enumerator = Recurrence.Daily(beginDate, endDate, fromDate, toDate, interval); // IEnumerator<DateOnly>
```

## Installing DateRecurrenceR

You can install DateRecurrenceR via [NuGet](https://www.nuget.org/packages/DateRecurrenceR):

**Package Manager Console:**

```shell
Install-Package DateRecurrenceR
```

**.NET Core CLI:**

```shell
dotnet add package DateRecurrenceR
```