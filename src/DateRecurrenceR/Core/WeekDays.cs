namespace DateRecurrenceR.Core;

/// <summary>
/// Represents weekdays for one week.
/// </summary>
public readonly struct WeekDays
{
#if NET8_0_OR_GREATER
    private readonly WeekDaysArray _ds;
#else
    private readonly bool[] _ds = new bool[DaysInWeek];
#endif

    /// <summary>
    /// Create an instance with one defined day of the week.
    /// </summary>
    /// <param name="day"><see cref="DayOfWeek" /></param>
    public WeekDays(DayOfWeek day)
        : this(day, day)
    {
    }

    /// <summary>
    /// Create an instance with two defined days of the week.
    /// </summary>
    /// <param name="day1"><see cref="DayOfWeek" /></param>
    /// <param name="day2"><see cref="DayOfWeek" /></param>
    public WeekDays(DayOfWeek day1, DayOfWeek day2)
        : this(day1, day2, day2)
    {
    }

    /// <summary>
    /// Create an instance with three defined days of the week.
    /// </summary>
    /// <param name="day1"><see cref="DayOfWeek" /></param>
    /// <param name="day2"><see cref="DayOfWeek" /></param>
    /// <param name="day3"><see cref="DayOfWeek" /></param>
    public WeekDays(DayOfWeek day1, DayOfWeek day2, DayOfWeek day3)
        : this(day1, day2, day3, day3)
    {
    }

    /// <summary>
    /// Create an instance with four defined days of the week.
    /// </summary>
    /// <param name="day1"><see cref="DayOfWeek" /></param>
    /// <param name="day2"><see cref="DayOfWeek" /></param>
    /// <param name="day3"><see cref="DayOfWeek" /></param>
    /// <param name="day4"><see cref="DayOfWeek" /></param>
    public WeekDays(DayOfWeek day1, DayOfWeek day2, DayOfWeek day3, DayOfWeek day4)
        : this(day1, day2, day3, day4, day4)
    {
    }

    /// <summary>
    /// Create an instance with five defined days of the week.
    /// </summary>
    /// <param name="day1"><see cref="DayOfWeek" /></param>
    /// <param name="day2"><see cref="DayOfWeek" /></param>
    /// <param name="day3"><see cref="DayOfWeek" /></param>
    /// <param name="day4"><see cref="DayOfWeek" /></param>
    /// <param name="day5"><see cref="DayOfWeek" /></param>
    public WeekDays(DayOfWeek day1, DayOfWeek day2, DayOfWeek day3, DayOfWeek day4, DayOfWeek day5)
        : this(day1, day2, day3, day4, day5, day5)
    {
    }

    /// <summary>
    /// Create an instance with six defined days of the week.
    /// </summary>
    /// <param name="day1"><see cref="DayOfWeek" /></param>
    /// <param name="day2"><see cref="DayOfWeek" /></param>
    /// <param name="day3"><see cref="DayOfWeek" /></param>
    /// <param name="day4"><see cref="DayOfWeek" /></param>
    /// <param name="day5"><see cref="DayOfWeek" /></param>
    /// <param name="day6"><see cref="DayOfWeek" /></param>
    public WeekDays(DayOfWeek day1, DayOfWeek day2, DayOfWeek day3, DayOfWeek day4, DayOfWeek day5, DayOfWeek day6)
        : this(day1, day2, day3, day4, day5, day6, day6)
    {
    }

    /// <summary>
    /// Create an instance with seven defined days of the week.
    /// </summary>
    /// <param name="day1"><see cref="DayOfWeek" /></param>
    /// <param name="day2"><see cref="DayOfWeek" /></param>
    /// <param name="day3"><see cref="DayOfWeek" /></param>
    /// <param name="day4"><see cref="DayOfWeek" /></param>
    /// <param name="day5"><see cref="DayOfWeek" /></param>
    /// <param name="day6"><see cref="DayOfWeek" /></param>
    /// <param name="day7"><see cref="DayOfWeek" /></param>
    public WeekDays(DayOfWeek day1,
        DayOfWeek day2,
        DayOfWeek day3,
        DayOfWeek day4,
        DayOfWeek day5,
        DayOfWeek day6,
        DayOfWeek day7)
    {
        _ds[(int) day1] = true;
        _ds[(int) day2] = true;
        _ds[(int) day3] = true;
        _ds[(int) day4] = true;
        _ds[(int) day5] = true;
        _ds[(int) day6] = true;
        _ds[(int) day7] = true;
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Create an instance with seven predefined days of the week.
    /// </summary>
    /// <param name="daysArray"><see cref="WeekDaysArray" /></param>
    public WeekDays(WeekDaysArray daysArray)
    {
        _ds = daysArray;
    }
#endif

    /// <summary>
    /// Create an instance with seven defined days of the week.
    /// </summary>
    /// <param name="isSunday">Set this parameter to True for Sunday.</param>
    /// <param name="isMonday">Set this parameter to True for Monday.</param>
    /// <param name="isTuesday">Set this parameter to True for Tuesday.</param>
    /// <param name="isWednesday">Set this parameter to True for Wednesday.</param>
    /// <param name="isThursday">Set this parameter to True for Thursday.</param>
    /// <param name="isFriday">Set this parameter to True for Friday.</param>
    /// <param name="isSaturday">Set this parameter to True for Saturday.</param>
    public WeekDays(bool isSunday,
        bool isMonday,
        bool isTuesday,
        bool isWednesday,
        bool isThursday,
        bool isFriday,
        bool isSaturday)
    {
        _ds[0] = isSunday;
        _ds[1] = isMonday;
        _ds[2] = isTuesday;
        _ds[3] = isWednesday;
        _ds[4] = isThursday;
        _ds[5] = isFriday;
        _ds[6] = isSaturday;
    }

    /// <summary>
    /// Returns a minimal defined day of the week. 
    /// </summary>
    public DayOfWeek GetMinByFirstDayOfWeek(DayOfWeek firstDayOfWeek)
    {
        var shift = (7 + (int) firstDayOfWeek) % 7;
        if (_ds[shift]) return (DayOfWeek) shift;

        shift = (7 + (int) firstDayOfWeek + 1) % 7;
        if (_ds[shift]) return (DayOfWeek) shift;

        shift = (7 + (int) firstDayOfWeek + 2) % 7;
        if (_ds[shift]) return (DayOfWeek) shift;

        shift = (7 + (int) firstDayOfWeek + 3) % 7;
        if (_ds[shift]) return (DayOfWeek) shift;

        shift = (7 + (int) firstDayOfWeek + 4) % 7;
        if (_ds[shift]) return (DayOfWeek) shift;

        shift = (7 + (int) firstDayOfWeek + 5) % 7;
        if (_ds[shift]) return (DayOfWeek) shift;

        shift = (7 + (int) firstDayOfWeek + 6) % 7;
        return (DayOfWeek) shift;
    }

    public DayOfWeek GetMaxDay(DayOfWeek firstDayOfWeek)
    {
        var shift = (7 + (int) firstDayOfWeek + 6) % 7;
        if (_ds[shift]) return (DayOfWeek) shift;

        shift = (7 + (int) firstDayOfWeek + 5) % 7;
        if (_ds[shift]) return (DayOfWeek) shift;

        shift = (7 + (int) firstDayOfWeek + 4) % 7;
        if (_ds[shift]) return (DayOfWeek) shift;

        shift = (7 + (int) firstDayOfWeek + 3) % 7;
        if (_ds[shift]) return (DayOfWeek) shift;

        shift = (7 + (int) firstDayOfWeek + 2) % 7;
        if (_ds[shift]) return (DayOfWeek) shift;

        shift = (7 + (int) firstDayOfWeek + 1) % 7;
        if (_ds[shift]) return (DayOfWeek) shift;

        shift = (7 + (int) firstDayOfWeek) % 7;
        return (DayOfWeek) shift;
    }

    // todo: turn into property Count
    public int GetCount()
    {
        return (_ds[0] ? 1 : 0)
               + (_ds[1] ? 1 : 0)
               + (_ds[2] ? 1 : 0)
               + (_ds[3] ? 1 : 0)
               + (_ds[4] ? 1 : 0)
               + (_ds[5] ? 1 : 0)
               + (_ds[6] ? 1 : 0);
    }

    public int GetCountFrom(DayOfWeek fromDay, DayOfWeek firstDayOfWeek)
    {
        var fdwIndex = (int) firstDayOfWeek;
        var fIndex = (7 + (int) fromDay) % 7;
        return (_ds[(7 + fdwIndex) % 7] ? (7 + fIndex) % 7 == fIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 1) % 7] ? (7 + fdwIndex + 1) % 7 <= fIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 2) % 7] ? (7 + fdwIndex + 2) % 7 <= fIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 3) % 7] ? (7 + fdwIndex + 3) % 7 <= fIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 4) % 7] ? (7 + fdwIndex + 4) % 7 <= fIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 5) % 7] ? (7 + fdwIndex + 5) % 7 <= fIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 6) % 7] ? (7 + fdwIndex + 6) % 7 <= fIndex ? 1 : 0 : 0);
    }

    public int GetCountBefore(DayOfWeek beforeDay, DayOfWeek firstDayOfWeek)
    {
        var fdwIndex = (int) firstDayOfWeek;
        var bIndex = (7 + (int) beforeDay - (int) firstDayOfWeek) % 7;
        return (_ds[(7 - fdwIndex) % 7] ? (7 - bIndex) % 7 < bIndex ? 1 : 0 : 0)
               + (_ds[(7 - fdwIndex + 1) % 7] ? (7 - fdwIndex + 1) % 7 < bIndex ? 1 : 0 : 0)
               + (_ds[(7 - fdwIndex + 2) % 7] ? (7 - fdwIndex + 2) % 7 < bIndex ? 1 : 0 : 0)
               + (_ds[(7 - fdwIndex + 3) % 7] ? (7 - fdwIndex + 3) % 7 < bIndex ? 1 : 0 : 0)
               + (_ds[(7 - fdwIndex + 4) % 7] ? (7 - fdwIndex + 4) % 7 < bIndex ? 1 : 0 : 0)
               + (_ds[(7 - fdwIndex + 5) % 7] ? (7 - fdwIndex + 5) % 7 < bIndex ? 1 : 0 : 0)
               + (_ds[(7 - fdwIndex + 6) % 7] ? (7 - fdwIndex + 6) % 7 < bIndex ? 1 : 0 : 0);
    }

    public bool TryGetDayFromLeft(DayOfWeek day, DayOfWeek firstDayOfWeek, out DayOfWeek result)
    {
        if (_ds[(int) day])
        {
            result = day;
            return true;
        }

        return TryGetFirstSelectedBefore(day, firstDayOfWeek, out result);
    }

    /// <summary>
    /// Returns whether the day of the week has been defined.
    /// </summary>
    /// <param name="dayOfWeek"></param>
    public bool this[DayOfWeek dayOfWeek] => _ds[(int) dayOfWeek];

    private bool TryGetFirstSelectedBefore(DayOfWeek day, DayOfWeek firstDayOfWeek, out DayOfWeek result)
    {
        var dayIndex = GetShiftedIndex(day, firstDayOfWeek);

        var tIndex = dayIndex - 1;
        if (tIndex > 0 && _ds[tIndex])
        {
            result = (DayOfWeek) GetNormalIndex(tIndex, firstDayOfWeek);
            return true;
        }

        tIndex = dayIndex - 2;
        if (tIndex > 0 && _ds[tIndex])
        {
            result = (DayOfWeek) GetNormalIndex(tIndex, firstDayOfWeek);
            return true;
        }

        tIndex = dayIndex - 3;
        if (tIndex > 0 && _ds[tIndex])
        {
            result = (DayOfWeek) GetNormalIndex(tIndex, firstDayOfWeek);
            return true;
        }

        tIndex = dayIndex - 4;
        if (tIndex > 0 && _ds[tIndex])
        {
            result = (DayOfWeek) GetNormalIndex(tIndex, firstDayOfWeek);
            return true;
        }

        tIndex = dayIndex - 5;
        if (tIndex > 0 && _ds[tIndex])
        {
            result = (DayOfWeek) GetNormalIndex(tIndex, firstDayOfWeek);
            return true;
        }

        result = default;

        return false;
    }

    private static int GetShiftedIndex(DayOfWeek day, DayOfWeek firstDayOfWeek)
    {
        return (7 + (int) day - (int) firstDayOfWeek) % 7;
    }

    private static int GetNormalIndex(int dayIndex, DayOfWeek firstDayOfWeek)
    {
        return (7 + dayIndex + (int) firstDayOfWeek) % 7;
    }
}