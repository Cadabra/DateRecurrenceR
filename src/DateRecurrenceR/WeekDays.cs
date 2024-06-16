namespace DateRecurrenceR;

public sealed class WeekDays
{
    private readonly bool[] _ds = new bool[DaysInWeek];

    public WeekDays(DayOfWeek day1)
        : this(day1, day1)
    {
    }

    public WeekDays(DayOfWeek day1, DayOfWeek day2)
        : this(day1, day2, day2)
    {
    }

    public WeekDays(DayOfWeek day1, DayOfWeek day2, DayOfWeek day3)
        : this(day1, day2, day3, day3)
    {
    }

    public WeekDays(DayOfWeek day1, DayOfWeek day2, DayOfWeek day3, DayOfWeek day4)
        : this(day1, day2, day3, day4, day4)
    {
    }

    public WeekDays(DayOfWeek day1, DayOfWeek day2, DayOfWeek day3, DayOfWeek day4, DayOfWeek day5)
        : this(day1, day2, day3, day4, day5, day5)
    {
    }

    public WeekDays(DayOfWeek day1, DayOfWeek day2, DayOfWeek day3, DayOfWeek day4, DayOfWeek day5, DayOfWeek day6)
        : this(day1, day2, day3, day4, day5, day6, day6)
    {
    }

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

        UpdateMinDay(day1, day2, day3, day4, day5, day6, day7);
    }

    public DayOfWeek MinDay { get; private set; }

    public bool this[DayOfWeek dayOfWeek] => _ds[(int) dayOfWeek];

    private void UpdateMinDay(DayOfWeek day1,
        DayOfWeek day2,
        DayOfWeek day3,
        DayOfWeek day4,
        DayOfWeek day5,
        DayOfWeek day6,
        DayOfWeek day7)
    {
        MinDay = (DayOfWeek) Math.Min((int) day1,
            Math.Min((int) day2,
                Math.Min((int) day3,
                    Math.Min((int) day4,
                        Math.Min((int) day5,
                            Math.Min((int) day6, (int) day7))))));
    }
}