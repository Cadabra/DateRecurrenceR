using DateRecurrenceR.Extensions;

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
        DayOfWeek day7) : this()
    {
        _ds[(int)day1] = true;
        _ds[(int)day2] = true;
        _ds[(int)day3] = true;
        _ds[(int)day4] = true;
        _ds[(int)day5] = true;
        _ds[(int)day6] = true;
        _ds[(int)day7] = true;
        CountOfSelected = GetCountOfSelected();
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Create an instance with seven predefined days of the week.
    /// </summary>
    /// <param name="daysArray"><see cref="WeekDaysArray" /></param>
    public WeekDays(WeekDaysArray daysArray)
    {
        _ds = daysArray;
        CountOfSelected = _ds.GetCountOfSelected();
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
        bool isSaturday) : this()
    {
        _ds[0] = isSunday;
        _ds[1] = isMonday;
        _ds[2] = isTuesday;
        _ds[3] = isWednesday;
        _ds[4] = isThursday;
        _ds[5] = isFriday;
        _ds[6] = isSaturday;
        CountOfSelected = GetCountOfSelected();
    }

    /// <summary>
    /// Returns an integer bitmask representing the selected days of the week.
    /// Bit 0 = Sunday, bit 1 = Monday, …, bit 6 = Saturday.
    /// </summary>
    public int ToFlag()
    {
        var result = 0;

        result |= _ds[0] ? 1 << 0 : 0; // 1 Sunday
        result |= _ds[1] ? 1 << 1 : 0; // 2 Monday
        result |= _ds[2] ? 1 << 2 : 0; // 4 Tuesday
        result |= _ds[3] ? 1 << 3 : 0; // 8 Wednesday
        result |= _ds[4] ? 1 << 4 : 0; // 16 Thursday
        result |= _ds[5] ? 1 << 5 : 0; // 32 Friday
        result |= _ds[6] ? 1 << 6 : 0; // 64 Saturday

        return result;
    }

    /// <summary>
    /// Returns a minimal defined day of the week. 
    /// </summary>
    public DayOfWeek GetMinByFirstDayOfWeek(DayOfWeek firstDayOfWeek)
    {
        for (var i = 0; i < DaysInWeek - 1; i++)
        {
            var shift = (7 + (int)firstDayOfWeek + i) % 7;
            if (_ds[shift]) return (DayOfWeek)shift;
        }

        return (DayOfWeek)((7 + (int)firstDayOfWeek + 6) % 7);
    }

    /// <summary>
    /// Returns a maximum defined day of the week. 
    /// </summary>
    public DayOfWeek GetMaxDay(DayOfWeek firstDayOfWeek)
    {
        for (var i = DaysInWeek - 1; i > 0; i--)
        {
            var shift = (7 + (int)firstDayOfWeek + i) % 7;
            if (_ds[shift]) return (DayOfWeek)shift;
        }

        return (DayOfWeek)((7 + (int)firstDayOfWeek) % 7);
    }

    /// <summary>
    /// Gets the total number of selected days in the week.
    /// </summary>
    public int CountOfSelected { get; }

    private int GetCountOfSelected()
    {
        return (_ds[0] ? 1 : 0)
               + (_ds[1] ? 1 : 0)
               + (_ds[2] ? 1 : 0)
               + (_ds[3] ? 1 : 0)
               + (_ds[4] ? 1 : 0)
               + (_ds[5] ? 1 : 0)
               + (_ds[6] ? 1 : 0);
    }

    /// <summary>
    /// Returns the number of selected days up to and including <paramref name="maxDay"/>,
    /// ordered relative to <paramref name="firstDayOfWeek"/>.
    /// </summary>
    /// <param name="maxDay">The inclusive upper bound day of the week.</param>
    /// <param name="firstDayOfWeek">The first day of the week used as the ordering origin.</param>
    public int GetCountOfSelected(DayOfWeek maxDay, DayOfWeek firstDayOfWeek)
    {
        var fdwIndex = (int)firstDayOfWeek;
        var maxDayIndex = (7 + (int)maxDay - (int)firstDayOfWeek) % 7;
        return (_ds[(7 + fdwIndex) % 7] ? (7 + 0) % 7 <= maxDayIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 1) % 7] ? (7 + 1) % 7 <= maxDayIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 2) % 7] ? (7 + 2) % 7 <= maxDayIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 3) % 7] ? (7 + 3) % 7 <= maxDayIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 4) % 7] ? (7 + 4) % 7 <= maxDayIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 5) % 7] ? (7 + 5) % 7 <= maxDayIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 6) % 7] ? (7 + 6) % 7 <= maxDayIndex ? 1 : 0 : 0);
    }

    /// <summary>
    /// Returns the number of selected days whose shifted index (relative to <paramref name="firstDayOfWeek"/>)
    /// is less than or equal to <paramref name="maxDayIndex"/>.
    /// </summary>
    /// <param name="maxDayIndex">The inclusive upper bound as a shifted day index (0-based from <paramref name="firstDayOfWeek"/>).</param>
    /// <param name="firstDayOfWeek">The first day of the week used as the ordering origin.</param>
    public int GetCountOfSelected(int maxDayIndex, DayOfWeek firstDayOfWeek)
    {
        var fdwIndex = (int)firstDayOfWeek;
        return (_ds[(7 + fdwIndex) % 7] ? (7 + 0) % 7 <= maxDayIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 1) % 7] ? (7 + 1) % 7 <= maxDayIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 2) % 7] ? (7 + 2) % 7 <= maxDayIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 3) % 7] ? (7 + 3) % 7 <= maxDayIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 4) % 7] ? (7 + 4) % 7 <= maxDayIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 5) % 7] ? (7 + 5) % 7 <= maxDayIndex ? 1 : 0 : 0)
               + (_ds[(7 + fdwIndex + 6) % 7] ? (7 + 6) % 7 <= maxDayIndex ? 1 : 0 : 0);
    }

    /// <summary>
    /// Returns whether the day of the week has been defined.
    /// </summary>
    /// <param name="dayOfWeek"></param>
    public bool this[DayOfWeek dayOfWeek] => _ds[(int)dayOfWeek];

    /// <summary>
    /// Tries to get the day of the week at the given selected index (relative to <paramref name="firstDayOfWeek"/>),
    /// counting only the days that are set in this instance.
    /// </summary>
    /// <param name="selectedIndex">Zero-based index among the selected days (ordered from <paramref name="firstDayOfWeek"/>).</param>
    /// <param name="firstDayOfWeek">The first day of the week to use as the ordering origin.</param>
    /// <param name="result">The matched day of the week, or <see langword="default"/> if not found.</param>
    /// <returns><see langword="true"/> if a matching day was found; otherwise <see langword="false"/>.</returns>
    public bool TryGet(int selectedIndex, DayOfWeek firstDayOfWeek, out DayOfWeek result)
    {
        var currentIndex = 0;
        for (var i = 0; i < DaysInWeek; i++)
        {
            var day = (DayOfWeek)((7 + (int)firstDayOfWeek + i) % 7);
            if (!_ds[(int)day]) continue;
            if (currentIndex++ == selectedIndex)
            {
                result = day;
                return true;
            }
        }

        result = default;
        return false;
    }

}
