using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;

namespace DateRecurrenceR.Internals;

/// <summary>
/// Resolves the concrete date of a monthly occurrence for a given year and month.
/// Stored inline in <see cref="Collections.MonthlyEnumerator"/> so that enumeration
/// stays allocation-free.
/// </summary>
internal readonly struct MonthlyDateResolver
{
    private readonly int _dayOfMonth;
    private readonly DayOfWeek _dayOfWeek;
    private readonly IndexOfDay _indexOfDay;
    private readonly bool _byDayOfWeek;

    private MonthlyDateResolver(int dayOfMonth, DayOfWeek dayOfWeek, IndexOfDay indexOfDay, bool byDayOfWeek)
    {
        _dayOfMonth = dayOfMonth;
        _dayOfWeek = dayOfWeek;
        _indexOfDay = indexOfDay;
        _byDayOfWeek = byDayOfWeek;
    }

    public static MonthlyDateResolver ByDayOfMonth(int dayOfMonth)
    {
        return new MonthlyDateResolver(dayOfMonth, default, default, byDayOfWeek: false);
    }

    public static MonthlyDateResolver ByDayOfWeek(DayOfWeek dayOfWeek, IndexOfDay indexOfDay)
    {
        return new MonthlyDateResolver(default, dayOfWeek, indexOfDay, byDayOfWeek: true);
    }

    public DateOnly GetDate(int year, int month)
    {
        return _byDayOfWeek
            ? DateOnlyHelper.GetDateByDayOfMonth(year, month, _dayOfWeek, _indexOfDay)
            : DateOnlyHelper.GetDateByDayOfMonth(year, month, _dayOfMonth);
    }
}
