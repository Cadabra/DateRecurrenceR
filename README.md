# DateRecurrenceR [![Nuget](https://img.shields.io/nuget/v/DateRecurrenceR?logo=NuGet)](https://www.nuget.org/packages/DateRecurrenceR)
# needs help with testing

DateRecurrenceR is a .NET library designed to handle recurrence patterns for dates efficiently. It allows you to generate and manage recurring date sequences without storing every sequences, making it ideal for applications with dynamic date range calculations.

## Core functional
The primary feature of DateRecurrenceR is its ability to extract specific subranges from a defined recurrence pattern. Instead of storing all dates from a recurrence, it calculates the necessary subset on-the-fly based on a given start date, end date, and recurrence pattern. This approach optimizes performance and memory usage.

#### Example
pattern: `Tuesday` and `Friday` every 2 weeks from `999-01-01`\
Both `X` and `A` are dates for first 3 month.\
`s` is start subrange date `999-02-16`\
`A` are subrange dates\
see the project: [UsingWithFewSubranges](https://github.com/Cadabra/DateRecurrenceR/tree/main/samples/UsingWithFewSubranges)
```
s  m  t  w  r  f  s   s  m  t  w  r  f  s   s  m  t  w  r  f  s
--------------------  --------------------  --------------------
.  .  X  .  .  X  .            .  .  X  .         A  .  .  A  .
.  .  .  .  .  .  .   .  .  .  .  .  .  .   .  .  .  .  .  .  .
.  .  X  .  .  X  .   .  .  X  .  .  X  s   .  .  A  .  .  A  .
.  .  .  .  .  .  .   .  .  .  .  .  .  .   .  .  .  .  .  .  .
.  .  X               .  .  A  .  .         .  .  A  .  .  A  .
```
### Key Features
* **Dynamic Subrange Extraction:** Generate only the dates you need within a specified range.
* **Flexible Patterns:** Support for various recurrence patterns (e.g., daily, weekly, monthly, yearly).
* **Efficient Memory Usage:** Avoids storing large sets of dates, improving performance.

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