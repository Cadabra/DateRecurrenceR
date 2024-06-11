using System;

namespace DateRecurrenceR;

public sealed class WeekDays
{
    private readonly bool[] _ds = new bool[7];

    public WeekDays(DayOfWeek dayOfWeek)
    {
        MinDay = (DayOfWeek)7;

        _ds[(int)dayOfWeek] = true;
        UpdateMinDay(dayOfWeek);
    }

    public WeekDays(params DayOfWeek[] daysArray)
    {
        MinDay = (DayOfWeek)7;

        for (var i = 0; i < daysArray.Length; i++)
        {
            _ds[(int)daysArray[i]] = true;
            UpdateMinDay(daysArray[i]);
        }
    }
    
    public DayOfWeek MinDay { get; private set; }
    
    public bool this[DayOfWeek dayOfWeek]
    {
        get => _ds[(int)dayOfWeek];
        // init => _ds[(int)dayOfWeek] = value;
    }

    private void UpdateMinDay(DayOfWeek dayOfWeek)
    {
        if (MinDay > dayOfWeek)
        {
            MinDay = dayOfWeek;
        }
    }
}