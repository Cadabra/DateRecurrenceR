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

        UpdateMinDay();
    }

    public WeekDays(bool day1,
        bool day2,
        bool day3,
        bool day4,
        bool day5,
        bool day6,
        bool day7)
    {
        MinDay = (DayOfWeek) DaysInWeek;

        _ds[0] = day1;
        _ds[1] = day2;
        _ds[2] = day3;
        _ds[3] = day4;
        _ds[4] = day5;
        _ds[5] = day6;
        _ds[6] = day7;

        UpdateMinDay();
    }

    public DayOfWeek MinDay { get; private set; }

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