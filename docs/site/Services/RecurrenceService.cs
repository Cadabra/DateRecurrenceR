using DateRecurrenceR.Core;
using DateRecurrenceR.Docs.Models;
using System.Text;

namespace DateRecurrenceR.Docs.Services;

public class RecurrenceService
{
    private const int MaxResults = 500;

    // ─────────────────────────────────────────────
    // Daily
    // ─────────────────────────────────────────────

    public RecurrenceResult GetDaily(
        DateOnly beginDate,
        DateOnly endDate,
        int interval,
        SubrangeMode subrangeMode,
        DateOnly? fromDate = null,
        DateOnly? toDate = null,
        int? count = null)
    {
        var iv = new Interval(interval);
        IEnumerator<DateOnly> enumerator;

        switch (subrangeMode)
        {
            case SubrangeMode.ByDates:
                enumerator = Recurrence.Daily(beginDate, endDate, fromDate!.Value, toDate!.Value, iv);
                break;
            case SubrangeMode.ByCount:
                enumerator = fromDate.HasValue
                    ? Recurrence.Daily(beginDate, fromDate.Value, count!.Value, iv)
                    : Recurrence.Daily(beginDate, count!.Value, iv);
                break;
            default:
                enumerator = Recurrence.Daily(beginDate, endDate, iv);
                break;
        }

        var snippet = BuildDailySnippet(beginDate, endDate, interval, subrangeMode, fromDate, toDate, count);
        return Enumerate(enumerator, snippet);
    }

    private static string BuildDailySnippet(
        DateOnly beginDate, DateOnly endDate, int interval,
        SubrangeMode mode, DateOnly? fromDate, DateOnly? toDate, int? count)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using DateRecurrenceR;");
        sb.AppendLine();
        sb.AppendLine($"var beginDate = new DateOnly({beginDate.Year}, {beginDate.Month}, {beginDate.Day});");
        sb.AppendLine($"var interval  = new Interval({interval});");

        switch (mode)
        {
            case SubrangeMode.ByDates:
                sb.AppendLine($"var endDate  = new DateOnly({endDate.Year}, {endDate.Month}, {endDate.Day});");
                sb.AppendLine($"var fromDate = new DateOnly({fromDate!.Value.Year}, {fromDate.Value.Month}, {fromDate.Value.Day});");
                sb.AppendLine($"var toDate   = new DateOnly({toDate!.Value.Year}, {toDate.Value.Month}, {toDate.Value.Day});");
                sb.AppendLine();
                sb.AppendLine("var enumerator = Recurrence.Daily(beginDate, endDate, fromDate, toDate, interval);");
                break;
            case SubrangeMode.ByCount:
                if (fromDate.HasValue)
                {
                    sb.AppendLine($"var fromDate = new DateOnly({fromDate.Value.Year}, {fromDate.Value.Month}, {fromDate.Value.Day});");
                    sb.AppendLine();
                    sb.AppendLine($"var enumerator = Recurrence.Daily(beginDate, fromDate, {count}, interval);");
                }
                else
                {
                    sb.AppendLine();
                    sb.AppendLine($"var enumerator = Recurrence.Daily(beginDate, {count}, interval);");
                }
                break;
            default:
                sb.AppendLine($"var endDate = new DateOnly({endDate.Year}, {endDate.Month}, {endDate.Day});");
                sb.AppendLine();
                sb.AppendLine("var enumerator = Recurrence.Daily(beginDate, endDate, interval);");
                break;
        }

        AppendLoop(sb);
        return sb.ToString();
    }

    // ─────────────────────────────────────────────
    // Weekly
    // ─────────────────────────────────────────────

    public RecurrenceResult GetWeekly(
        DateOnly beginDate,
        DateOnly endDate,
        int interval,
        bool sun, bool mon, bool tue, bool wed, bool thu, bool fri, bool sat,
        DayOfWeek firstDayOfWeek,
        SubrangeMode subrangeMode,
        DateOnly? fromDate = null,
        DateOnly? toDate = null,
        int? count = null)
    {
        var iv = new Interval(interval);
        var weekDays = new WeekDays(sun, mon, tue, wed, thu, fri, sat);
        IEnumerator<DateOnly> enumerator;

        switch (subrangeMode)
        {
            case SubrangeMode.ByDates:
                enumerator = Recurrence.Weekly(beginDate, endDate, fromDate!.Value, toDate!.Value, weekDays, firstDayOfWeek, iv);
                break;
            case SubrangeMode.ByCount:
                enumerator = fromDate.HasValue
                    ? Recurrence.Weekly(beginDate, fromDate.Value, count!.Value, weekDays, firstDayOfWeek, iv)
                    : Recurrence.Weekly(beginDate, count!.Value, weekDays, firstDayOfWeek, iv);
                break;
            default:
                enumerator = Recurrence.Weekly(beginDate, endDate, weekDays, firstDayOfWeek, iv);
                break;
        }

        var snippet = BuildWeeklySnippet(beginDate, endDate, interval, sun, mon, tue, wed, thu, fri, sat,
            firstDayOfWeek, subrangeMode, fromDate, toDate, count);
        return Enumerate(enumerator, snippet);
    }

    private static string BuildWeeklySnippet(
        DateOnly beginDate, DateOnly endDate, int interval,
        bool sun, bool mon, bool tue, bool wed, bool thu, bool fri, bool sat,
        DayOfWeek firstDayOfWeek, SubrangeMode mode,
        DateOnly? fromDate, DateOnly? toDate, int? count)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using DateRecurrenceR;");
        sb.AppendLine();
        sb.AppendLine($"var beginDate     = new DateOnly({beginDate.Year}, {beginDate.Month}, {beginDate.Day});");
        sb.AppendLine($"var interval      = new Interval({interval});");
        sb.AppendLine($"var weekDays      = new WeekDays({sun.ToString().ToLower()}, {mon.ToString().ToLower()}, {tue.ToString().ToLower()}, {wed.ToString().ToLower()}, {thu.ToString().ToLower()}, {fri.ToString().ToLower()}, {sat.ToString().ToLower()});");
        sb.AppendLine($"var firstDayOfWeek = DayOfWeek.{firstDayOfWeek};");

        switch (mode)
        {
            case SubrangeMode.ByDates:
                sb.AppendLine($"var endDate  = new DateOnly({endDate.Year}, {endDate.Month}, {endDate.Day});");
                sb.AppendLine($"var fromDate = new DateOnly({fromDate!.Value.Year}, {fromDate.Value.Month}, {fromDate.Value.Day});");
                sb.AppendLine($"var toDate   = new DateOnly({toDate!.Value.Year}, {toDate.Value.Month}, {toDate.Value.Day});");
                sb.AppendLine();
                sb.AppendLine("var enumerator = Recurrence.Weekly(beginDate, endDate, fromDate, toDate, weekDays, firstDayOfWeek, interval);");
                break;
            case SubrangeMode.ByCount:
                if (fromDate.HasValue)
                {
                    sb.AppendLine($"var fromDate = new DateOnly({fromDate.Value.Year}, {fromDate.Value.Month}, {fromDate.Value.Day});");
                    sb.AppendLine();
                    sb.AppendLine($"var enumerator = Recurrence.Weekly(beginDate, fromDate, {count}, weekDays, firstDayOfWeek, interval);");
                }
                else
                {
                    sb.AppendLine();
                    sb.AppendLine($"var enumerator = Recurrence.Weekly(beginDate, {count}, weekDays, firstDayOfWeek, interval);");
                }
                break;
            default:
                sb.AppendLine($"var endDate = new DateOnly({endDate.Year}, {endDate.Month}, {endDate.Day});");
                sb.AppendLine();
                sb.AppendLine("var enumerator = Recurrence.Weekly(beginDate, endDate, weekDays, firstDayOfWeek, interval);");
                break;
        }

        AppendLoop(sb);
        return sb.ToString();
    }

    // ─────────────────────────────────────────────
    // Monthly — By Day of Month
    // ─────────────────────────────────────────────

    public RecurrenceResult GetMonthlyByDayOfMonth(
        DateOnly beginDate,
        DateOnly endDate,
        int interval,
        int dayOfMonth,
        SubrangeMode subrangeMode,
        DateOnly? fromDate = null,
        DateOnly? toDate = null,
        int? count = null)
    {
        var iv = new Interval(interval);
        var dom = new DayOfMonth(dayOfMonth);
        IEnumerator<DateOnly> enumerator;

        switch (subrangeMode)
        {
            case SubrangeMode.ByDates:
                enumerator = Recurrence.Monthly(beginDate, endDate, fromDate!.Value, toDate!.Value, dom, iv);
                break;
            case SubrangeMode.ByCount:
                enumerator = fromDate.HasValue
                    ? Recurrence.Monthly(beginDate, fromDate.Value, count!.Value, dom, iv)
                    : Recurrence.Monthly(beginDate, count!.Value, dom, iv);
                break;
            default:
                enumerator = Recurrence.Monthly(beginDate, endDate, dom, iv);
                break;
        }

        var snippet = BuildMonthlyDomSnippet(beginDate, endDate, interval, dayOfMonth, subrangeMode, fromDate, toDate, count);
        return Enumerate(enumerator, snippet);
    }

    private static string BuildMonthlyDomSnippet(
        DateOnly beginDate, DateOnly endDate, int interval, int dayOfMonth,
        SubrangeMode mode, DateOnly? fromDate, DateOnly? toDate, int? count)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using DateRecurrenceR;");
        sb.AppendLine();
        sb.AppendLine($"var beginDate  = new DateOnly({beginDate.Year}, {beginDate.Month}, {beginDate.Day});");
        sb.AppendLine($"var interval   = new Interval({interval});");
        sb.AppendLine($"var dayOfMonth = new DayOfMonth({dayOfMonth});");

        switch (mode)
        {
            case SubrangeMode.ByDates:
                sb.AppendLine($"var endDate  = new DateOnly({endDate.Year}, {endDate.Month}, {endDate.Day});");
                sb.AppendLine($"var fromDate = new DateOnly({fromDate!.Value.Year}, {fromDate.Value.Month}, {fromDate.Value.Day});");
                sb.AppendLine($"var toDate   = new DateOnly({toDate!.Value.Year}, {toDate.Value.Month}, {toDate.Value.Day});");
                sb.AppendLine();
                sb.AppendLine("var enumerator = Recurrence.Monthly(beginDate, endDate, fromDate, toDate, dayOfMonth, interval);");
                break;
            case SubrangeMode.ByCount:
                if (fromDate.HasValue)
                {
                    sb.AppendLine($"var fromDate = new DateOnly({fromDate.Value.Year}, {fromDate.Value.Month}, {fromDate.Value.Day});");
                    sb.AppendLine();
                    sb.AppendLine($"var enumerator = Recurrence.Monthly(beginDate, fromDate, {count}, dayOfMonth, interval);");
                }
                else
                {
                    sb.AppendLine();
                    sb.AppendLine($"var enumerator = Recurrence.Monthly(beginDate, {count}, dayOfMonth, interval);");
                }
                break;
            default:
                sb.AppendLine($"var endDate = new DateOnly({endDate.Year}, {endDate.Month}, {endDate.Day});");
                sb.AppendLine();
                sb.AppendLine("var enumerator = Recurrence.Monthly(beginDate, endDate, dayOfMonth, interval);");
                break;
        }

        AppendLoop(sb);
        return sb.ToString();
    }

    // ─────────────────────────────────────────────
    // Monthly — By Day of Week + Index
    // ─────────────────────────────────────────────

    public RecurrenceResult GetMonthlyByDayOfWeek(
        DateOnly beginDate,
        DateOnly endDate,
        int interval,
        DayOfWeek dayOfWeek,
        IndexOfDay indexOfDay,
        SubrangeMode subrangeMode,
        DateOnly? fromDate = null,
        DateOnly? toDate = null,
        int? count = null)
    {
        var iv = new Interval(interval);
        IEnumerator<DateOnly> enumerator;

        switch (subrangeMode)
        {
            case SubrangeMode.ByDates:
                enumerator = Recurrence.Monthly(beginDate, endDate, fromDate!.Value, toDate!.Value, dayOfWeek, indexOfDay, iv);
                break;
            case SubrangeMode.ByCount:
                enumerator = fromDate.HasValue
                    ? Recurrence.Monthly(beginDate, fromDate.Value, count!.Value, dayOfWeek, indexOfDay, iv)
                    : Recurrence.Monthly(beginDate, count!.Value, dayOfWeek, indexOfDay, iv);
                break;
            default:
                enumerator = Recurrence.Monthly(beginDate, endDate, dayOfWeek, indexOfDay, iv);
                break;
        }

        var snippet = BuildMonthlyDowSnippet(beginDate, endDate, interval, dayOfWeek, indexOfDay, subrangeMode, fromDate, toDate, count);
        return Enumerate(enumerator, snippet);
    }

    private static string BuildMonthlyDowSnippet(
        DateOnly beginDate, DateOnly endDate, int interval,
        DayOfWeek dayOfWeek, IndexOfDay indexOfDay,
        SubrangeMode mode, DateOnly? fromDate, DateOnly? toDate, int? count)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using DateRecurrenceR;");
        sb.AppendLine();
        sb.AppendLine($"var beginDate  = new DateOnly({beginDate.Year}, {beginDate.Month}, {beginDate.Day});");
        sb.AppendLine($"var interval   = new Interval({interval});");
        sb.AppendLine($"var dayOfWeek  = DayOfWeek.{dayOfWeek};");
        sb.AppendLine($"var indexOfDay = IndexOfDay.{indexOfDay};");

        switch (mode)
        {
            case SubrangeMode.ByDates:
                sb.AppendLine($"var endDate  = new DateOnly({endDate.Year}, {endDate.Month}, {endDate.Day});");
                sb.AppendLine($"var fromDate = new DateOnly({fromDate!.Value.Year}, {fromDate.Value.Month}, {fromDate.Value.Day});");
                sb.AppendLine($"var toDate   = new DateOnly({toDate!.Value.Year}, {toDate.Value.Month}, {toDate.Value.Day});");
                sb.AppendLine();
                sb.AppendLine("var enumerator = Recurrence.Monthly(beginDate, endDate, fromDate, toDate, dayOfWeek, indexOfDay, interval);");
                break;
            case SubrangeMode.ByCount:
                if (fromDate.HasValue)
                {
                    sb.AppendLine($"var fromDate = new DateOnly({fromDate.Value.Year}, {fromDate.Value.Month}, {fromDate.Value.Day});");
                    sb.AppendLine();
                    sb.AppendLine($"var enumerator = Recurrence.Monthly(beginDate, fromDate, {count}, dayOfWeek, indexOfDay, interval);");
                }
                else
                {
                    sb.AppendLine();
                    sb.AppendLine($"var enumerator = Recurrence.Monthly(beginDate, {count}, dayOfWeek, indexOfDay, interval);");
                }
                break;
            default:
                sb.AppendLine($"var endDate = new DateOnly({endDate.Year}, {endDate.Month}, {endDate.Day});");
                sb.AppendLine();
                sb.AppendLine("var enumerator = Recurrence.Monthly(beginDate, endDate, dayOfWeek, indexOfDay, interval);");
                break;
        }

        AppendLoop(sb);
        return sb.ToString();
    }

    // ─────────────────────────────────────────────
    // Yearly — By Day and Month
    // ─────────────────────────────────────────────

    public RecurrenceResult GetYearlyByDayAndMonth(
        DateOnly beginDate,
        DateOnly endDate,
        int interval,
        int dayOfMonth,
        int monthOfYear,
        SubrangeMode subrangeMode,
        DateOnly? fromDate = null,
        DateOnly? toDate = null,
        int? count = null)
    {
        var iv = new Interval(interval);
        var dom = new DayOfMonth(dayOfMonth);
        var moy = new MonthOfYear(monthOfYear);
        IEnumerator<DateOnly> enumerator;

        switch (subrangeMode)
        {
            case SubrangeMode.ByDates:
                enumerator = Recurrence.Yearly(beginDate, endDate, fromDate!.Value, toDate!.Value, dom, moy, iv);
                break;
            case SubrangeMode.ByCount:
                enumerator = fromDate.HasValue
                    ? Recurrence.Yearly(beginDate, fromDate.Value, count!.Value, dom, moy, iv)
                    : Recurrence.Yearly(beginDate, count!.Value, dom, moy, iv);
                break;
            default:
                enumerator = Recurrence.Yearly(beginDate, endDate, dom, moy, iv);
                break;
        }

        var snippet = BuildYearlyDayMonthSnippet(beginDate, endDate, interval, dayOfMonth, monthOfYear, subrangeMode, fromDate, toDate, count);
        return Enumerate(enumerator, snippet);
    }

    private static string BuildYearlyDayMonthSnippet(
        DateOnly beginDate, DateOnly endDate, int interval,
        int dayOfMonth, int monthOfYear, SubrangeMode mode,
        DateOnly? fromDate, DateOnly? toDate, int? count)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using DateRecurrenceR;");
        sb.AppendLine();
        sb.AppendLine($"var beginDate   = new DateOnly({beginDate.Year}, {beginDate.Month}, {beginDate.Day});");
        sb.AppendLine($"var interval    = new Interval({interval});");
        sb.AppendLine($"var dayOfMonth  = new DayOfMonth({dayOfMonth});");
        sb.AppendLine($"var monthOfYear = new MonthOfYear({monthOfYear});");

        switch (mode)
        {
            case SubrangeMode.ByDates:
                sb.AppendLine($"var endDate  = new DateOnly({endDate.Year}, {endDate.Month}, {endDate.Day});");
                sb.AppendLine($"var fromDate = new DateOnly({fromDate!.Value.Year}, {fromDate.Value.Month}, {fromDate.Value.Day});");
                sb.AppendLine($"var toDate   = new DateOnly({toDate!.Value.Year}, {toDate.Value.Month}, {toDate.Value.Day});");
                sb.AppendLine();
                sb.AppendLine("var enumerator = Recurrence.Yearly(beginDate, endDate, fromDate, toDate, dayOfMonth, monthOfYear, interval);");
                break;
            case SubrangeMode.ByCount:
                if (fromDate.HasValue)
                {
                    sb.AppendLine($"var fromDate = new DateOnly({fromDate.Value.Year}, {fromDate.Value.Month}, {fromDate.Value.Day});");
                    sb.AppendLine();
                    sb.AppendLine($"var enumerator = Recurrence.Yearly(beginDate, fromDate, {count}, dayOfMonth, monthOfYear, interval);");
                }
                else
                {
                    sb.AppendLine();
                    sb.AppendLine($"var enumerator = Recurrence.Yearly(beginDate, {count}, dayOfMonth, monthOfYear, interval);");
                }
                break;
            default:
                sb.AppendLine($"var endDate = new DateOnly({endDate.Year}, {endDate.Month}, {endDate.Day});");
                sb.AppendLine();
                sb.AppendLine("var enumerator = Recurrence.Yearly(beginDate, endDate, dayOfMonth, monthOfYear, interval);");
                break;
        }

        AppendLoop(sb);
        return sb.ToString();
    }

    // ─────────────────────────────────────────────
    // Yearly — By Day of Week + Index + Month
    // ─────────────────────────────────────────────

    public RecurrenceResult GetYearlyByDayOfWeek(
        DateOnly beginDate,
        DateOnly endDate,
        int interval,
        DayOfWeek dayOfWeek,
        IndexOfDay indexOfDay,
        int monthOfYear,
        SubrangeMode subrangeMode,
        DateOnly? fromDate = null,
        DateOnly? toDate = null,
        int? count = null)
    {
        var iv = new Interval(interval);
        var moy = new MonthOfYear(monthOfYear);
        IEnumerator<DateOnly> enumerator;

        switch (subrangeMode)
        {
            case SubrangeMode.ByDates:
                enumerator = Recurrence.Yearly(beginDate, endDate, fromDate!.Value, toDate!.Value, dayOfWeek, indexOfDay, moy, iv);
                break;
            case SubrangeMode.ByCount:
                enumerator = fromDate.HasValue
                    ? Recurrence.Yearly(beginDate, fromDate.Value, count!.Value, dayOfWeek, indexOfDay, moy, iv)
                    : Recurrence.Yearly(beginDate, count!.Value, dayOfWeek, indexOfDay, moy, iv);
                break;
            default:
                enumerator = Recurrence.Yearly(beginDate, endDate, dayOfWeek, indexOfDay, moy, iv);
                break;
        }

        var snippet = BuildYearlyDowSnippet(beginDate, endDate, interval, dayOfWeek, indexOfDay, monthOfYear, subrangeMode, fromDate, toDate, count);
        return Enumerate(enumerator, snippet);
    }

    private static string BuildYearlyDowSnippet(
        DateOnly beginDate, DateOnly endDate, int interval,
        DayOfWeek dayOfWeek, IndexOfDay indexOfDay, int monthOfYear,
        SubrangeMode mode, DateOnly? fromDate, DateOnly? toDate, int? count)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using DateRecurrenceR;");
        sb.AppendLine();
        sb.AppendLine($"var beginDate   = new DateOnly({beginDate.Year}, {beginDate.Month}, {beginDate.Day});");
        sb.AppendLine($"var interval    = new Interval({interval});");
        sb.AppendLine($"var dayOfWeek   = DayOfWeek.{dayOfWeek};");
        sb.AppendLine($"var indexOfDay  = IndexOfDay.{indexOfDay};");
        sb.AppendLine($"var monthOfYear = new MonthOfYear({monthOfYear});");

        switch (mode)
        {
            case SubrangeMode.ByDates:
                sb.AppendLine($"var endDate  = new DateOnly({endDate.Year}, {endDate.Month}, {endDate.Day});");
                sb.AppendLine($"var fromDate = new DateOnly({fromDate!.Value.Year}, {fromDate.Value.Month}, {fromDate.Value.Day});");
                sb.AppendLine($"var toDate   = new DateOnly({toDate!.Value.Year}, {toDate.Value.Month}, {toDate.Value.Day});");
                sb.AppendLine();
                sb.AppendLine("var enumerator = Recurrence.Yearly(beginDate, endDate, fromDate, toDate, dayOfWeek, indexOfDay, monthOfYear, interval);");
                break;
            case SubrangeMode.ByCount:
                if (fromDate.HasValue)
                {
                    sb.AppendLine($"var fromDate = new DateOnly({fromDate.Value.Year}, {fromDate.Value.Month}, {fromDate.Value.Day});");
                    sb.AppendLine();
                    sb.AppendLine($"var enumerator = Recurrence.Yearly(beginDate, fromDate, {count}, dayOfWeek, indexOfDay, monthOfYear, interval);");
                }
                else
                {
                    sb.AppendLine();
                    sb.AppendLine($"var enumerator = Recurrence.Yearly(beginDate, {count}, dayOfWeek, indexOfDay, monthOfYear, interval);");
                }
                break;
            default:
                sb.AppendLine($"var endDate = new DateOnly({endDate.Year}, {endDate.Month}, {endDate.Day});");
                sb.AppendLine();
                sb.AppendLine("var enumerator = Recurrence.Yearly(beginDate, endDate, dayOfWeek, indexOfDay, monthOfYear, interval);");
                break;
        }

        AppendLoop(sb);
        return sb.ToString();
    }

    // ─────────────────────────────────────────────
    // Yearly — By Day of Year
    // ─────────────────────────────────────────────

    public RecurrenceResult GetYearlyByDayOfYear(
        DateOnly beginDate,
        DateOnly endDate,
        int interval,
        int dayOfYear,
        SubrangeMode subrangeMode,
        DateOnly? fromDate = null,
        DateOnly? toDate = null,
        int? count = null)
    {
        var iv = new Interval(interval);
        var doy = new DayOfYear(dayOfYear);
        IEnumerator<DateOnly> enumerator;

        switch (subrangeMode)
        {
            case SubrangeMode.ByDates:
                enumerator = Recurrence.Yearly(beginDate, endDate, fromDate!.Value, toDate!.Value, doy, iv);
                break;
            case SubrangeMode.ByCount:
                enumerator = fromDate.HasValue
                    ? Recurrence.Yearly(beginDate, fromDate.Value, count!.Value, doy, iv)
                    : Recurrence.Yearly(beginDate, count!.Value, doy, iv);
                break;
            default:
                enumerator = Recurrence.Yearly(beginDate, endDate, doy, iv);
                break;
        }

        var snippet = BuildYearlyDoySnippet(beginDate, endDate, interval, dayOfYear, subrangeMode, fromDate, toDate, count);
        return Enumerate(enumerator, snippet);
    }

    private static string BuildYearlyDoySnippet(
        DateOnly beginDate, DateOnly endDate, int interval, int dayOfYear,
        SubrangeMode mode, DateOnly? fromDate, DateOnly? toDate, int? count)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using DateRecurrenceR;");
        sb.AppendLine();
        sb.AppendLine($"var beginDate = new DateOnly({beginDate.Year}, {beginDate.Month}, {beginDate.Day});");
        sb.AppendLine($"var interval  = new Interval({interval});");
        sb.AppendLine($"var dayOfYear = new DayOfYear({dayOfYear});");

        switch (mode)
        {
            case SubrangeMode.ByDates:
                sb.AppendLine($"var endDate  = new DateOnly({endDate.Year}, {endDate.Month}, {endDate.Day});");
                sb.AppendLine($"var fromDate = new DateOnly({fromDate!.Value.Year}, {fromDate.Value.Month}, {fromDate.Value.Day});");
                sb.AppendLine($"var toDate   = new DateOnly({toDate!.Value.Year}, {toDate.Value.Month}, {toDate.Value.Day});");
                sb.AppendLine();
                sb.AppendLine("var enumerator = Recurrence.Yearly(beginDate, endDate, fromDate, toDate, dayOfYear, interval);");
                break;
            case SubrangeMode.ByCount:
                if (fromDate.HasValue)
                {
                    sb.AppendLine($"var fromDate = new DateOnly({fromDate.Value.Year}, {fromDate.Value.Month}, {fromDate.Value.Day});");
                    sb.AppendLine();
                    sb.AppendLine($"var enumerator = Recurrence.Yearly(beginDate, fromDate, {count}, dayOfYear, interval);");
                }
                else
                {
                    sb.AppendLine();
                    sb.AppendLine($"var enumerator = Recurrence.Yearly(beginDate, {count}, dayOfYear, interval);");
                }
                break;
            default:
                sb.AppendLine($"var endDate = new DateOnly({endDate.Year}, {endDate.Month}, {endDate.Day});");
                sb.AppendLine();
                sb.AppendLine("var enumerator = Recurrence.Yearly(beginDate, endDate, dayOfYear, interval);");
                break;
        }

        AppendLoop(sb);
        return sb.ToString();
    }

    // ─────────────────────────────────────────────
    // Union
    // ─────────────────────────────────────────────

    public RecurrenceResult GetUnion(List<UnionPatternSlot> slots)
    {
        var dateSources = new Dictionary<DateOnly, List<int>>();

        for (int i = 0; i < slots.Count; i++)
        {
            var enumerator = BuildEnumeratorForSlot(slots[i]);
            try
            {
                while (enumerator.MoveNext())
                {
                    var date = enumerator.Current;
                    if (!dateSources.TryGetValue(date, out var list))
                        dateSources[date] = list = new List<int>();
                    if (!list.Contains(i))
                        list.Add(i);
                }
            }
            finally { enumerator.Dispose(); }
        }

        var allDates = dateSources.Keys.OrderBy(d => d).ToList();
        var snippet = BuildUnionSnippet(slots);

        return new RecurrenceResult
        {
            CSharpSnippet = snippet,
            TotalCount = allDates.Count,
            IsTruncated = allDates.Count > MaxResults,
            Dates = allDates.Take(MaxResults).ToList(),
            DateSources = dateSources
        };
    }

    private IEnumerator<DateOnly> BuildEnumeratorForSlot(UnionPatternSlot slot)
    {
        var iv = new Interval(slot.Interval);

        return slot.PatternType switch
        {
            PatternType.Daily => Recurrence.Daily(slot.BeginDate, slot.EndDate, iv),
            PatternType.Weekly => Recurrence.Weekly(slot.BeginDate, slot.EndDate,
                new WeekDays(slot.Sun, slot.Mon, slot.Tue, slot.Wed, slot.Thu, slot.Fri, slot.Sat),
                slot.FirstDayOfWeek, iv),
            PatternType.Monthly => slot.MonthlySubMode == MonthlySubMode.ByDayOfMonth
                ? Recurrence.Monthly(slot.BeginDate, slot.EndDate, new DayOfMonth(slot.DayOfMonth), iv)
                : Recurrence.Monthly(slot.BeginDate, slot.EndDate, slot.DayOfWeek, slot.IndexOfDay, iv),
            PatternType.Yearly => Recurrence.Yearly(slot.BeginDate, slot.EndDate,
                new DayOfMonth(slot.DayOfMonth), new MonthOfYear(slot.MonthOfYear), iv),
            _ => throw new InvalidOperationException()
        };
    }

    private static string BuildUnionSnippet(List<UnionPatternSlot> slots)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using DateRecurrenceR;");
        sb.AppendLine();

        for (int i = 0; i < slots.Count; i++)
        {
            var s = slots[i];
            var iv = s.Interval;
            var begin = $"new DateOnly({s.BeginDate.Year}, {s.BeginDate.Month}, {s.BeginDate.Day})";
            var end = $"new DateOnly({s.EndDate.Year}, {s.EndDate.Month}, {s.EndDate.Day})";
            sb.Append($"var e{i + 1} = ");
            switch (s.PatternType)
            {
                case PatternType.Daily:
                    sb.AppendLine($"Recurrence.Daily({begin}, {end}, new Interval({iv}));");
                    break;
                case PatternType.Weekly:
                    sb.AppendLine($"Recurrence.Weekly({begin}, {end},");
                    sb.AppendLine($"    new WeekDays({s.Sun.ToString().ToLower()}, {s.Mon.ToString().ToLower()}, {s.Tue.ToString().ToLower()}, {s.Wed.ToString().ToLower()}, {s.Thu.ToString().ToLower()}, {s.Fri.ToString().ToLower()}, {s.Sat.ToString().ToLower()}),");
                    sb.AppendLine($"    DayOfWeek.{s.FirstDayOfWeek}, new Interval({iv}));");
                    break;
                case PatternType.Monthly when s.MonthlySubMode == MonthlySubMode.ByDayOfMonth:
                    sb.AppendLine($"Recurrence.Monthly({begin}, {end}, new DayOfMonth({s.DayOfMonth}), new Interval({iv}));");
                    break;
                case PatternType.Monthly:
                    sb.AppendLine($"Recurrence.Monthly({begin}, {end}, DayOfWeek.{s.DayOfWeek}, IndexOfDay.{s.IndexOfDay}, new Interval({iv}));");
                    break;
                case PatternType.Yearly:
                    sb.AppendLine($"Recurrence.Yearly({begin}, {end}, new DayOfMonth({s.DayOfMonth}), new MonthOfYear({s.MonthOfYear}), new Interval({iv}));");
                    break;
            }
        }

        var args = string.Join(", ", Enumerable.Range(1, slots.Count).Select(i => $"e{i}"));
        sb.AppendLine();
        sb.AppendLine($"var enumerator = Recurrence.Union({args});");
        AppendLoop(sb);
        return sb.ToString();
    }

    // ─────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────

    private static RecurrenceResult Enumerate(IEnumerator<DateOnly> enumerator, string snippet)
    {
        var result = new RecurrenceResult { CSharpSnippet = snippet };
        int totalCount = 0;

        try
        {
            while (enumerator.MoveNext())
            {
                totalCount++;
                if (totalCount <= MaxResults)
                    result.Dates.Add(enumerator.Current);
            }
        }
        finally
        {
            enumerator.Dispose();
        }

        result.TotalCount = totalCount;
        result.IsTruncated = totalCount > MaxResults;
        return result;
    }

    private static void AppendLoop(StringBuilder sb)
    {
        sb.AppendLine();
        sb.AppendLine("while (enumerator.MoveNext())");
        sb.AppendLine("    Console.WriteLine(enumerator.Current.ToString(\"yyyy-MM-dd\"));");
    }
}
