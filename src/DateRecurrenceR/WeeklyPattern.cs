using DateRecurrenceR.Collections;
using DateRecurrenceR.Core;
using DateRecurrenceR.Helpers;
using Range = DateRecurrenceR.Core.Range;

namespace DateRecurrenceR;

public interface IRecurrence
{
    public IEnumerator<DateOnly> GetEnumerator();
    public IEnumerator<DateOnly> GetEnumerator(int takeCount);
    public IEnumerator<DateOnly> GetEnumerator(DateOnly fromDate, int takeCount);
    public IEnumerator<DateOnly> GetEnumerator(DateOnly fromDate, DateOnly toDate);

    bool Contains(DateOnly date);
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
        var patternHash = WeeklyRecurrenceHelper.GetPatternHash(pattern.WeekDays, pattern.Interval, pattern.FirstDayOfWeek);
        var canStart = WeeklyRecurrenceHelper.TryGetStartDate(
            range.BeginDate, //beginDate,
            range.BeginDate, //fromDate,
            patternHash,
            pattern.WeekDays,
            pattern.FirstDayOfWeek,
            pattern.Interval,
            out _startDate);

        var selectedDaysInWeek = pattern.WeekDays.GetCountSelectedDays();

        var startWeekDayNumber = _startDate.DayNumber - (int) _startDate.DayOfWeek + (int) pattern.FirstDayOfWeek;
        var daysInRange = range.EndDate!.Value.DayNumber - startWeekDayNumber + 1;
        var daysInInterval = DaysInWeek * pattern.Interval;
        var fullIntervalsExceptFirst = (daysInRange / daysInInterval) * selectedDaysInWeek;
        var tail = daysInRange % daysInInterval;

        var t0 = pattern.WeekDays.GetCountSelectedDays(range.EndDate!.Value.DayOfWeek, pattern.FirstDayOfWeek);
        var t1 = pattern.WeekDays.GetCountSelectedDays2(_startDate.DayOfWeek, pattern.FirstDayOfWeek);
        fullIntervalsExceptFirst += tail > DaysInWeek ? selectedDaysInWeek : tail == 0 ? 0 : t0;
        fullIntervalsExceptFirst -= t1;

        _count = fullIntervalsExceptFirst;
        // _range = range;
        _pattern = pattern;
    }

    // public DateOnly StartDate => _startDate;
    public int Count => _count;

    public IEnumerator<DateOnly> GetEnumerator()
    {
        // return new WeeklyEnumeratorLimitByDate(startDate, stopDate, patternHash);

        throw new NotImplementedException();
    }

    public IEnumerator<DateOnly> GetEnumerator(int takeCount)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<DateOnly> GetEnumerator(DateOnly fromDate, int takeCount)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<DateOnly> GetEnumerator(DateOnly fromDate, DateOnly toDate)
    {
        throw new NotImplementedException();
    }

    public bool Contains(DateOnly date)
    {
        throw new NotImplementedException();

        // if (!WeekDays[date.DayOfWeek]) return false;
        //
        // if (date < BeginDate || EndDate < date) return false;
        //
        // if ((date.DayNumber - BeginDate.DayNumber) % (Interval * 7) > 7) return false;
        //
        // return true;
    }
}