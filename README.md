# DateRecurrenceR

[![Release](https://github.com/Cadabra/DateRecurrenceR/actions/workflows/release.yml/badge.svg)](https://github.com/Cadabra/DateRecurrenceR/actions/workflows/release.yml) [![Nuget](https://img.shields.io/nuget/v/DateRecurrenceR?logo=NuGet)](https://www.nuget.org/packages/DateRecurrenceR)

## Efficient Date Recurrence Library for .NET

**DateRecurrenceR** is a .NET library designed to handle and manage date recurrence patterns efficiently.

## Key Features
  
* **Dynamic Subrange Generation:** Generate only the dates you need within a specified range, optimizing performance and
  memory usage.
* **Time Localization Simplification:** Uses DateOnly, eliminating issues related to time localization.
* **Flexible Collection Handling:** Returns an enumerator, allowing developers to choose the collection type.
* **Optimal Performance:** Boasts a Big O notation of O(1), ensuring efficient operations.

### Installing DateRecurrenceR

You can install DateRecurrenceR via [NuGet](https://www.nuget.org/packages/DateRecurrenceR):

**Package Manager Console:**

```shell
Install-Package DateRecurrenceR
```

**.NET Core CLI:**

```shell
dotnet add package DateRecurrenceR
```