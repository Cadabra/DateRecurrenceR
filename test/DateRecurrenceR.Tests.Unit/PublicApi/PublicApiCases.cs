using System.Globalization;
using DateRecurrenceR.Core;

namespace DateRecurrenceR.Tests.Unit.PublicApi;

/// <summary>
/// Registry of public-API behavioural cases mirroring <c>TEST_CASES.md</c> (case ids match 1:1).
/// Each entry pairs the exact <see cref="Recurrence"/> call with its expected sequence, so the
/// published bundle and the executed suite can never drift apart.
/// </summary>
internal static class PublicApiCases
{
    internal sealed record Case(string Id, Func<IEnumerator<DateOnly>> Factory, DateOnly[] Expected);

    private static readonly WeekDays Mwf = new(DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday);
    private static readonly WeekDays MonOnly = new(DayOfWeek.Monday);
    private static readonly WeekDays SatSun = new(DayOfWeek.Saturday, DayOfWeek.Sunday);

    private static DateOnly D(int y, int m, int d) => new(y, m, d);

    private static DateOnly[] Dates(params string[] iso) =>
        iso.Select(s => DateOnly.ParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToArray();

    public static readonly IReadOnlyList<Case> All = new List<Case>
    {
        // ---------- Daily ----------
        new("DAILY-M1", () => Recurrence.Daily(D(2024, 1, 1), 5, new Interval(1)),
            Dates("2024-01-01", "2024-01-02", "2024-01-03", "2024-01-04", "2024-01-05")),
        new("DAILY-M2", () => Recurrence.Daily(D(2024, 1, 1), 4, new Interval(3)),
            Dates("2024-01-01", "2024-01-04", "2024-01-07", "2024-01-10")),
        new("DAILY-M3", () => Recurrence.Daily(D(2024, 1, 1), D(2024, 1, 7), new Interval(2)),
            Dates("2024-01-01", "2024-01-03", "2024-01-05", "2024-01-07")),
        new("DAILY-C1", () => Recurrence.Daily(D(2024, 1, 1), 0, new Interval(1)),
            Dates()),
        new("DAILY-C2", () => Recurrence.Daily(D(2024, 1, 1), 1, new Interval(5)),
            Dates("2024-01-01")),
        new("DAILY-C3", () => Recurrence.Daily(D(2024, 1, 1), D(2024, 1, 5), 3, new Interval(2)),
            Dates("2024-01-05", "2024-01-07", "2024-01-09")),
        new("DAILY-C4", () => Recurrence.Daily(D(2024, 1, 1), D(2024, 1, 4), 3, new Interval(3)),
            Dates("2024-01-04", "2024-01-07", "2024-01-10")),
        new("DAILY-T1", () => Recurrence.Daily(D(2024, 1, 10), D(2024, 1, 20), D(2024, 1, 1), D(2024, 1, 5), new Interval(1)),
            Dates()),
        new("DAILY-T2", () => Recurrence.Daily(D(9999, 12, 30), 5, new Interval(1)),
            Dates("9999-12-30", "9999-12-31")),
        new("DAILY-T3", () => Recurrence.Daily(D(2024, 1, 1), D(2024, 2, 1), D(2024, 1, 10), D(2024, 1, 15), new Interval(2)),
            Dates("2024-01-11", "2024-01-13", "2024-01-15")),

        // ---------- Weekly ----------
        new("WEEK-M1", () => Recurrence.Weekly(D(2024, 1, 1), 6, Mwf, DayOfWeek.Monday, new Interval(1)),
            Dates("2024-01-01", "2024-01-03", "2024-01-05", "2024-01-08", "2024-01-10", "2024-01-12")),
        new("WEEK-M2", () => Recurrence.Weekly(D(2024, 1, 1), 4, MonOnly, DayOfWeek.Monday, new Interval(2)),
            Dates("2024-01-01", "2024-01-15", "2024-01-29", "2024-02-12")),
        new("WEEK-M3", () => Recurrence.Weekly(D(2024, 1, 1), D(2024, 1, 14), Mwf, DayOfWeek.Monday, new Interval(1)),
            Dates("2024-01-01", "2024-01-03", "2024-01-05", "2024-01-08", "2024-01-10", "2024-01-12")),
        new("WEEK-C1", () => Recurrence.Weekly(D(2024, 1, 2), 4, Mwf, DayOfWeek.Monday, new Interval(1)),
            Dates("2024-01-03", "2024-01-05", "2024-01-08", "2024-01-10")),
        new("WEEK-C2", () => Recurrence.Weekly(D(2024, 1, 6), 4, SatSun, DayOfWeek.Sunday, new Interval(1)),
            Dates("2024-01-06", "2024-01-07", "2024-01-13", "2024-01-14")),
        new("WEEK-T1", () => Recurrence.Weekly(D(2024, 1, 6), 4, SatSun, DayOfWeek.Monday, new Interval(1)),
            Dates("2024-01-06", "2024-01-07", "2024-01-13", "2024-01-14")),
        new("WEEK-T2", () => Recurrence.Weekly(D(2024, 12, 30), 5, Mwf, DayOfWeek.Monday, new Interval(1)),
            Dates("2024-12-30", "2025-01-01", "2025-01-03", "2025-01-06", "2025-01-08")),
        new("WEEK-T3", () => Recurrence.Weekly(D(2024, 1, 1), D(2024, 1, 31), D(2024, 1, 10), D(2024, 1, 20), Mwf, DayOfWeek.Monday, new Interval(1)),
            Dates("2024-01-10", "2024-01-12", "2024-01-15", "2024-01-17", "2024-01-19")),

        // ---------- Monthly by day-of-month ----------
        new("MONDOM-M1", () => Recurrence.Monthly(D(2024, 1, 10), 4, new DayOfMonth(15), new Interval(1)),
            Dates("2024-01-15", "2024-02-15", "2024-03-15", "2024-04-15")),
        new("MONDOM-M2", () => Recurrence.Monthly(D(2024, 1, 10), 6, new DayOfMonth(15), new Interval(2)),
            Dates("2024-01-15", "2024-03-15", "2024-05-15", "2024-07-15", "2024-09-15", "2024-11-15")),
        new("MONDOM-M3", () => Recurrence.Monthly(D(2024, 1, 1), D(2024, 6, 30), new DayOfMonth(15), new Interval(1)),
            Dates("2024-01-15", "2024-02-15", "2024-03-15", "2024-04-15", "2024-05-15", "2024-06-15")),
        new("MONDOM-C1", () => Recurrence.Monthly(D(2024, 1, 1), 4, new DayOfMonth(31), new Interval(1)),
            Dates("2024-01-31", "2024-02-29", "2024-03-31", "2024-04-30")),
        new("MONDOM-C2", () => Recurrence.Monthly(D(2024, 1, 20), 3, new DayOfMonth(15), new Interval(1)),
            Dates("2024-02-15", "2024-03-15", "2024-04-15")),
        new("MONDOM-C3", () => Recurrence.Monthly(D(2024, 1, 15), 1, new DayOfMonth(15), new Interval(1)),
            Dates("2024-01-15")),
        new("MONDOM-T1", () => Recurrence.Monthly(D(2024, 2, 1), 3, new DayOfMonth(29), new Interval(12)),
            Dates("2024-02-29", "2025-02-28", "2026-02-28")),
        new("MONDOM-T2", () => Recurrence.Monthly(D(2023, 1, 31), 4, new DayOfMonth(31), new Interval(1)),
            Dates("2023-01-31", "2023-02-28", "2023-03-31", "2023-04-30")),
        new("MONDOM-T3", () => Recurrence.Monthly(D(9999, 6, 15), 12, new DayOfMonth(15), new Interval(1)),
            Dates("9999-06-15", "9999-07-15", "9999-08-15", "9999-09-15", "9999-10-15", "9999-11-15", "9999-12-15")),

        // ---------- Monthly by day-of-week + index ----------
        new("MONDOW-M1", () => Recurrence.Monthly(D(2024, 1, 1), 3, DayOfWeek.Monday, IndexOfDay.First, new Interval(1)),
            Dates("2024-01-01", "2024-02-05", "2024-03-04")),
        new("MONDOW-M2", () => Recurrence.Monthly(D(2024, 1, 1), 3, DayOfWeek.Friday, IndexOfDay.Last, new Interval(1)),
            Dates("2024-01-26", "2024-02-23", "2024-03-29")),
        new("MONDOW-M3", () => Recurrence.Monthly(D(2024, 1, 1), D(2024, 4, 30), DayOfWeek.Sunday, IndexOfDay.Second, new Interval(1)),
            Dates("2024-01-14", "2024-02-11", "2024-03-10", "2024-04-14")),
        new("MONDOW-T1", () => Recurrence.Monthly(D(2024, 1, 1), 4, DayOfWeek.Thursday, IndexOfDay.Fourth, new Interval(1)),
            Dates("2024-01-25", "2024-02-22", "2024-03-28", "2024-04-25")),
        new("MONDOW-T2", () => Recurrence.Monthly(D(2024, 1, 1), 4, DayOfWeek.Thursday, IndexOfDay.Last, new Interval(1)),
            Dates("2024-01-25", "2024-02-29", "2024-03-28", "2024-04-25")),

        // ---------- Yearly by day-of-year ----------
        new("YRDOY-M1", () => Recurrence.Yearly(D(2024, 1, 1), 3, new DayOfYear(100), new Interval(1)),
            Dates("2024-04-09", "2025-04-10", "2026-04-10")),
        new("YRDOY-M2", () => Recurrence.Yearly(D(2024, 1, 1), 3, new DayOfYear(60), new Interval(1)),
            Dates("2024-02-29", "2025-03-01", "2026-03-01")),
        new("YRDOY-T1", () => Recurrence.Yearly(D(2023, 1, 1), 2, new DayOfYear(366), new Interval(1)),
            Dates("2023-12-31", "2024-12-31")),
        new("YRDOY-M3", () => Recurrence.Yearly(D(2024, 1, 1), D(2028, 12, 31), new DayOfYear(200), new Interval(2)),
            Dates("2024-07-18", "2026-07-19", "2028-07-18")),

        // ---------- Yearly by day-of-month + month ----------
        new("YRDOM-M1", () => Recurrence.Yearly(D(2024, 1, 1), 3, new DayOfMonth(25), new MonthOfYear(12), new Interval(1)),
            Dates("2024-12-25", "2025-12-25", "2026-12-25")),
        new("YRDOM-M2", () => Recurrence.Yearly(D(2024, 1, 1), D(2030, 12, 31), new DayOfMonth(1), new MonthOfYear(1), new Interval(2)),
            Dates("2024-01-01", "2026-01-01", "2028-01-01", "2030-01-01")),
        new("YRDOM-T1", () => Recurrence.Yearly(D(2024, 1, 1), 4, new DayOfMonth(29), new MonthOfYear(2), new Interval(1)),
            Dates("2024-02-29", "2025-02-28", "2026-02-28", "2027-02-28")),
        new("YRDOM-T2", () => Recurrence.Yearly(D(2024, 1, 1), 3, new DayOfMonth(29), new MonthOfYear(2), new Interval(4)),
            Dates("2024-02-29", "2028-02-29", "2032-02-29")),
        new("YRDOM-T3", () => Recurrence.Yearly(D(9999, 6, 15), 5, new DayOfMonth(15), new MonthOfYear(6), new Interval(1)),
            Dates("9999-06-15")),

        // ---------- Yearly by day-of-week + index + month ----------
        new("YRDOW-M1", () => Recurrence.Yearly(D(2024, 1, 1), 3, DayOfWeek.Thursday, IndexOfDay.Third, new MonthOfYear(11), new Interval(1)),
            Dates("2024-11-21", "2025-11-20", "2026-11-19")),
        new("YRDOW-T1", () => Recurrence.Yearly(D(2024, 1, 1), 3, DayOfWeek.Monday, IndexOfDay.Last, new MonthOfYear(5), new Interval(1)),
            Dates("2024-05-27", "2025-05-26", "2026-05-25")),

        // ---------- Union ----------
        new("UNION-M1", () => Recurrence.Union(
                Recurrence.Daily(D(2024, 1, 1), D(2024, 1, 10), new Interval(2)),
                Recurrence.Daily(D(2024, 1, 1), D(2024, 1, 10), new Interval(3))),
            Dates("2024-01-01", "2024-01-03", "2024-01-04", "2024-01-05", "2024-01-07", "2024-01-09", "2024-01-10")),
        new("UNION-T1", () => Recurrence.Union(
                Recurrence.Daily(D(2024, 1, 1), 3, new Interval(1)),
                Recurrence.Daily(D(2024, 1, 2), 3, new Interval(1))),
            Dates("2024-01-01", "2024-01-02", "2024-01-03", "2024-01-04")),
        new("UNION-T2", () => Recurrence.Union(
                Recurrence.Weekly(D(2024, 1, 1), D(2024, 1, 31), MonOnly, DayOfWeek.Monday, new Interval(1)),
                Recurrence.Monthly(D(2024, 1, 1), D(2024, 1, 31), new DayOfMonth(1), new Interval(1)),
                Recurrence.Daily(D(2024, 1, 15), 1, new Interval(1))),
            Dates("2024-01-01", "2024-01-08", "2024-01-15", "2024-01-22", "2024-01-29")),
    };
}
