namespace DateRecurrenceR;

public sealed class WeekDays
{
    private readonly bool[] _ds = new bool[7];

    public WeekDays(DayOfWeek dayOfWeek)
    {
        MinDay = (DayOfWeek) 7;

        _ds[(int) dayOfWeek] = true;
        UpdateMinDay(dayOfWeek);
    }

    public WeekDays(params DayOfWeek[] daysArray)
    {
        MinDay = (DayOfWeek) 7;

        for (var i = 0; i < daysArray.Length; i++)
        {
            _ds[(int) daysArray[i]] = true;
            UpdateMinDay(daysArray[i]);
        }
    }

    public DayOfWeek MinDay { get; private set; }

    public bool this[DayOfWeek dayOfWeek] => _ds[(int) dayOfWeek];

    private void UpdateMinDay(DayOfWeek dayOfWeek)
    {
        if (MinDay > dayOfWeek) MinDay = dayOfWeek;
    }

    public DayOfWeek GetNextDay(DayOfWeek dayOfWeek)
    {
        while (!_ds[(int) dayOfWeek])
        {
            dayOfWeek = (DayOfWeek)(((int)dayOfWeek + 1) % 7);
        }

        return dayOfWeek;
    }
}