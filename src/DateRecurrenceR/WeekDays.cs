namespace DateRecurrenceR;

/// <summary>
/// Represents weekdays for one week.
/// </summary>
public struct WeekDays
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
        MinDay = (DayOfWeek) DaysInWeek;

        _ds[(int) day1] = true;
        _ds[(int) day2] = true;
        _ds[(int) day3] = true;
        _ds[(int) day4] = true;
        _ds[(int) day5] = true;
        _ds[(int) day6] = true;
        _ds[(int) day7] = true;

        UpdateMinDay();
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Create an instance with seven predefined days of the week.
    /// </summary>
    /// <param name="daysArray"><see cref="WeekDaysArray" /></param>
    public WeekDays(WeekDaysArray daysArray)
    {
        MinDay = (DayOfWeek) DaysInWeek;

        _ds = daysArray;

        UpdateMinDay();
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
        MinDay = (DayOfWeek) DaysInWeek;

        _ds[0] = isSunday;
        _ds[1] = isMonday;
        _ds[2] = isTuesday;
        _ds[3] = isWednesday;
        _ds[4] = isThursday;
        _ds[5] = isFriday;
        _ds[6] = isSaturday;

        UpdateMinDay();
    }

    /// <summary>
    /// Returns a minimal defined day of the week. 
    /// </summary>
    public DayOfWeek MinDay { get; private set; }

    /// <summary>
    /// Returns whether the day of the week has been defined.
    /// </summary>
    /// <param name="dayOfWeek"></param>
    public bool this[DayOfWeek dayOfWeek] => _ds[(int) dayOfWeek];

    private void UpdateMinDay()
    {
        for (var i = 0; i < DaysInWeek; i++)
        {
            if (!_ds[i]) continue;
            MinDay = (DayOfWeek) i;
            break;
        }
    }
}