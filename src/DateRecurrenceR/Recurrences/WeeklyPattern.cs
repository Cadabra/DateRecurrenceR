using DateRecurrenceR.Collections;
using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR.Recurrences;

public interface IRecurrence
{
    DateOnly StartDate { get; }
    DateOnly StopDate { get; }
    int Count { get; }

    bool Contains(DateOnly date);
    IEnumerator<DateOnly> GetEnumerator();
    IEnumerator<DateOnly> GetEnumerator(int takeCount);
    IEnumerator<DateOnly> GetEnumerator(DateOnly fromDate, int takeCount);
    IEnumerator<DateOnly> GetEnumerator(DateOnly fromDate, DateOnly toDate);
}

public readonly struct WeeklyPattern
{
    public WeeklyPattern(Interval interval, WeekDays weekDays, DayOfWeek firstDayOfWeek)
    {
        Interval = interval;
        WeekDays = weekDays;
        FirstDayOfWeek = firstDayOfWeek;
    }

    public Interval Interval { get; }
    public WeekDays WeekDays { get; }
    public DayOfWeek FirstDayOfWeek { get; }
}

public readonly struct WeeklyRecurrence : IRecurrence
{
    private readonly WeeklyPattern _pattern;
    private readonly DateOnly _startDate;
    private readonly DateOnly _stopDate;
    private readonly int _count;

    public WeeklyRecurrence(Range range, WeeklyPattern pattern)
    {
        _pattern = pattern;

        var canStart = RecurrenceExt.TryGetStartDate(
            range.BeginDate,
            pattern.WeekDays,
            pattern.Interval,
            pattern.FirstDayOfWeek,
            out _startDate);

        if (!canStart)
        {
            _count = 0;
            return;
        }

        if (range.EndDate is not null)
        {
            _stopDate = RecurrenceExt.GetStopDate(
                _startDate,
                range.EndDate!.Value,
                pattern.WeekDays,
                pattern.Interval,
                pattern.FirstDayOfWeek);

            _count = RecurrenceExt.GetCount(
                _startDate,
                range.EndDate!.Value,
                pattern.WeekDays,
                pattern.Interval,
                pattern.FirstDayOfWeek);
        }
        else
        {
            _stopDate = RecurrenceExt.GetStopDate(
                _startDate,
                range.EndDate!.Value,
                pattern.WeekDays,
                pattern.Interval,
                pattern.FirstDayOfWeek);

            _count = RecurrenceExt.GetCount(
                _startDate,
                range.EndDate!.Value,
                pattern.WeekDays,
                pattern.Interval,
                pattern.FirstDayOfWeek);
        }
    }

    public DateOnly StartDate => _startDate;
    public DateOnly StopDate => _stopDate;
    public int Count => _count;

    public bool Contains(DateOnly date)
    {
        if (!_pattern.WeekDays[date.DayOfWeek]) return false;

        if (date < StartDate || StopDate < date) return false;

        if ((date.DayNumber - StartDate.DayNumber) % (_pattern.Interval * 7) > 7) return false;

        return true;
    }

    public IEnumerator<DateOnly> GetEnumerator()
    {
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(
            _pattern.WeekDays,
            _pattern.Interval,
            _pattern.FirstDayOfWeek);

        return new WeeklyEnumeratorLimitByDate(_startDate, _stopDate, patternHash);
    }

    public IEnumerator<DateOnly> GetEnumerator(int takeCount)
    {
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(
            _pattern.WeekDays,
            _pattern.Interval,
            _pattern.FirstDayOfWeek);

        return new WeeklyEnumeratorLimitByCount(_startDate, takeCount, patternHash);
    }

    public IEnumerator<DateOnly> GetEnumerator(DateOnly fromDate, int takeCount)
    {
        return Recurrence.Weekly(_startDate, takeCount, _pattern.WeekDays, _pattern.FirstDayOfWeek, _pattern.Interval);
    }

    public IEnumerator<DateOnly> GetEnumerator(DateOnly fromDate, DateOnly toDate)
    {
        return Recurrence.Weekly(_startDate, _stopDate, fromDate, toDate, _pattern.WeekDays, _pattern.FirstDayOfWeek,
            _pattern.Interval);
    }
}

public readonly struct RecurrenceExt
{
    public static bool TryGetStartDate(in DateOnly beginDate,
        in WeekDays weekDays,
        in Interval interval,
        in DayOfWeek firstDayOfWeek,
        out DateOnly startDate)
    {
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(
            weekDays,
            interval,
            firstDayOfWeek);

        return WeeklyRecurrenceHelper.TryGetStartDate(
            beginDate, //beginDate,
            beginDate, //fromDate,
            patternHash,
            weekDays,
            firstDayOfWeek,
            interval,
            out startDate);
    }

    public static DateOnly GetStopDate(in DateOnly startDate,
        in DateOnly endDate,
        in WeekDays weekDays,
        in Interval interval,
        in DayOfWeek firstDayOfWeek)
    {
        var startWeekDayNumber = startDate.DayNumber - (int) startDate.DayOfWeek + (int) firstDayOfWeek;
        var daysInRange = endDate.DayNumber - startWeekDayNumber + 1;
        var daysInInterval = DaysInWeek * interval;
        var tail = daysInRange % daysInInterval;

        var stopDateDayNumber = (daysInRange / daysInInterval) * daysInInterval;
        stopDateDayNumber += tail > DaysInWeek
            ? weekDays.TryGetDayFromLeft((DayOfWeek) ((7 + 6 + (int) firstDayOfWeek) % 7),
                firstDayOfWeek, out var sd)
                ? (7 + (int) sd - (int) firstDayOfWeek) % 7
                : 0
            : weekDays.TryGetDayFromLeft((DayOfWeek) (tail),
                firstDayOfWeek, out var sd2)
                ? (7 + (int) sd2 - (int) firstDayOfWeek) % 7
                : 0;

        return DateOnly.FromDayNumber(stopDateDayNumber);
    }

    public static int GetCount(in DateOnly startDate,
        in DateOnly endDate,
        in WeekDays weekDays,
        in Interval interval,
        in DayOfWeek firstDayOfWeek)
    {
        var selectedDaysInWeek = weekDays.GetCountSelectedDays();

        var startWeekDayNumber = startDate.DayNumber - (int) startDate.DayOfWeek + (int) firstDayOfWeek;
        var daysInRange = endDate.DayNumber - startWeekDayNumber + 1;
        var daysInInterval = DaysInWeek * interval;
        var fullIntervalsExceptFirst = (daysInRange / daysInInterval) * selectedDaysInWeek;
        var tail = daysInRange % daysInInterval;

        var t0 = weekDays.GetCountSelectedDays(endDate.DayOfWeek, firstDayOfWeek);
        var t1 = weekDays.GetCountSelectedDays2(startDate.DayOfWeek, firstDayOfWeek);
        fullIntervalsExceptFirst += tail > DaysInWeek ? selectedDaysInWeek : tail == 0 ? 0 : t0;
        fullIntervalsExceptFirst -= t1;

        return fullIntervalsExceptFirst;
    }

    public static int GetCount(in DateOnly startDate,
        in int expectedCount,
        in WeekDays weekDays,
        in Interval interval,
        in DayOfWeek firstDayOfWeek)
    {
        var selectedDaysInWeek = weekDays.GetCountSelectedDays();

        var fullWeekDays = expectedCount / selectedDaysInWeek * 7 * interval;
        var tail = expectedCount % selectedDaysInWeek;
        
        if (startDate.DayOfWeek == weekDays.GetMinByFirstDayOfWeek(firstDayOfWeek)){}

        if (tail == 0)
        {
            fullWeekDays -= 7 * interval;
        }
        else if (tail > 7)
        {
            var weekDaysMax = weekDays.GetMaxByFirstDayOfWeek(firstDayOfWeek);
        }
        else
        {
            var weekDaysMax = weekDays.GetMaxByFirstDayOfWeek(firstDayOfWeek);
        }

        return 0;
    }
}