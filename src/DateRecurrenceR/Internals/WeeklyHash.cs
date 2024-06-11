using System;

namespace DateRecurrenceR.Internals;

internal struct WeeklyHash
{
    private int _sunday;
    private int _monday;
    private int _tuesday;
    private int _wednesday;
    private int _thursday;
    private int _friday;
    private int _saturday;

    public int this[DayOfWeek dayOfWeek]
    {
        get
        {
            return dayOfWeek switch
            {
                DayOfWeek.Sunday => _sunday,
                DayOfWeek.Monday => _monday,
                DayOfWeek.Tuesday => _tuesday,
                DayOfWeek.Wednesday => _wednesday,
                DayOfWeek.Thursday => _thursday,
                DayOfWeek.Friday => _friday,
                DayOfWeek.Saturday => _saturday,
                _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
            };
        }
        set
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    _sunday = value;
                    break;
                case DayOfWeek.Monday:
                    _monday = value;
                    break;
                case DayOfWeek.Tuesday:
                    _tuesday = value;
                    break;
                case DayOfWeek.Wednesday:
                    _wednesday = value;
                    break;
                case DayOfWeek.Thursday:
                    _thursday = value;
                    break;
                case DayOfWeek.Friday:
                    _friday = value;
                    break;
                case DayOfWeek.Saturday:
                    _saturday = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null);
            }
        }
    }
}