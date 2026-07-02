using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR.Internals;

/// <summary>
/// Resolves the concrete date of a yearly occurrence for a given year.
/// Stored inline in <see cref="Collections.YearlyEnumerator"/> so that enumeration
/// stays allocation-free.
/// </summary>
internal readonly struct YearlyDateResolver
{
    private enum Mode : byte
    {
        ByDayOfYear = 0,
        ByDayOfMonth = 1,
        ByDayOfWeek = 2
    }

    private readonly Mode _mode;
    private readonly int _dayOfYear;
    private readonly int _month;
    private readonly int _dayOfMonth;
    private readonly DayOfWeek _dayOfWeek;
    private readonly IndexOfDay _indexOfDay;

    private YearlyDateResolver(Mode mode,
        int dayOfYear,
        int month,
        int dayOfMonth,
        DayOfWeek dayOfWeek,
        IndexOfDay indexOfDay)
    {
        _mode = mode;
        _dayOfYear = dayOfYear;
        _month = month;
        _dayOfMonth = dayOfMonth;
        _dayOfWeek = dayOfWeek;
        _indexOfDay = indexOfDay;
    }

    public static YearlyDateResolver ByDayOfYear(int dayOfYear)
    {
        return new YearlyDateResolver(Mode.ByDayOfYear, dayOfYear, default, default, default, default);
    }

    public static YearlyDateResolver ByDayOfMonth(int month, int dayOfMonth)
    {
        return new YearlyDateResolver(Mode.ByDayOfMonth, default, month, dayOfMonth, default, default);
    }

    public static YearlyDateResolver ByDayOfWeek(int month, DayOfWeek dayOfWeek, IndexOfDay indexOfDay)
    {
        return new YearlyDateResolver(Mode.ByDayOfWeek, default, month, default, dayOfWeek, indexOfDay);
    }

    public DateOnly GetDate(int year)
    {
        return _mode switch
        {
            Mode.ByDayOfYear => DateOnlyHelper.GetDateByDayOfYear(year, _dayOfYear),
            Mode.ByDayOfMonth => DateOnlyHelper.GetDateByDayOfMonth(year, _month, _dayOfMonth),
            _ => DateOnlyHelper.GetDateByDayOfMonth(year, _month, _dayOfWeek, _indexOfDay)
        };
    }
}
