namespace DateRecurrenceR;

public sealed class WeekDays
{
    private readonly bool[] _ds = new bool[DaysInWeek];

    public WeekDays(DayOfWeek dayOfWeek)
    {
        MinDay = (DayOfWeek) DaysInWeek;

        _ds[(int) dayOfWeek] = true;
        UpdateMinDay(dayOfWeek);
    }

    public WeekDays(params DayOfWeek[] daysArray)
    {
        MinDay = (DayOfWeek) DaysInWeek;

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
}