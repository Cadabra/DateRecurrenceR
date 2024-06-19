# DateRecurrenceR

[![Release](https://github.com/Cadabra/DateRecurrenceR/actions/workflows/release.yml/badge.svg)](https://github.com/Cadabra/DateRecurrenceR/actions/workflows/release.yml) [![Nuget](https://img.shields.io/nuget/v/DateRecurrenceR?logo=NuGet)](https://www.nuget.org/packages/DateRecurrenceR)

## Efficient Date Recurrence Library for .NET

**DateRecurrenceR** is a .NET library designed to handle and manage date recurrence patterns efficiently.

### Key Features

* **Dynamic Subrange Generation:** Generate only the dates you need within a specified range, optimizing performance and
  memory usage.
* **Time Localization Simplification:** Uses DateOnly, eliminating issues related to time localization.
* **Flexible Collection Handling:** Returns an enumerator, allowing developers to choose the collection type.
* **Optimal Performance:** Boasts a Big O notation of O(1), ensuring efficient operations.

### Methods for recurrences

| method  | description                                                                                   |
|---------|-----------------------------------------------------------------------------------------------|
| Daily   | Gets a daily recurrence by `interval`                                                         |
| Weekly  | Gets a weekly recurrence by `days of week` and `interval`                                     |
| Monthly | Gets a monthly recurrence by `days of week` or `day of month` and `interval`                  |
| Yearly  | Gets an yearly recurrence by `days of week` or `day of month` or `day of year` and `interval` |

All these methods return an instance of `IEnumerator<DateOnly>`.

### All parameters

| parameter      | description                                                                                |
|----------------|--------------------------------------------------------------------------------------------|
| beginDate      | The date of recurrence begins.                                                             |
| endDate        | The date of recurrence ends.                                                               |
| interval       | The interval between occurrences.                                                          |
| fromDate       | The date of specific range starts.                                                         |
| toDate         | The date of specific range finishes.                                                       |
| takeCount      | The maximum number of contiguous dates.                                                    |
| dayOfWeek      | The day of week.                                                                           |
| dayOfMonth     | The day of month. Takes the last day of month if `dayOfMonth` more than days in the month. |
| dayOfYear      | The day of year.                                                                           |
| weekDays       | Days of week.                                                                              |
| firstDayOfWeek | The first day of week.                                                                     |
| numberOfWeek   | The number of week. First Week of a Month starting from the first day of the month.        |
| monthOfYear    | The number of month.                                                                       |

### Subranges

All methods for recurrences support the following subrange rules:

#### By dates

* **Parameters:** `fromDate` and `toDate`
* **Rule:** takes dates from intersection `[beginDate, endDate] ∪ [fromDate, toDate]`

#### By count

* **Parameters:** `fromDate` and `takeCount`
* **Rule:** takes dates from intersection `[beginDate, endDate] ∪ [fromDate, DateOnly.MaxValue]` and takes
  first `takeCount` dates

## Examples of using

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