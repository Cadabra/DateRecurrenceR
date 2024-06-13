using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Helpers;

internal struct WeeklyRecurrenceHelper
{
    public static WeeklyHash GetPatternHash(WeekDays weekDays, int interval)
    {
        var hash = new WeeklyHash();

        var increment = DaysInWeek * (interval - 1) + 1;

        var weekDayIndex = ((int) weekDays.MinDay + 6) % DaysInWeek;

        for (var i = DaysInWeek; i > 0; i--)
        {
            switch (weekDayIndex)
            {
                case 0:
                    if (weekDays[DayOfWeek.Sunday])
                    {
                        hash[DayOfWeek.Sunday] = increment;
                        increment = 1;
                    }
                    else
                    {
                        hash[DayOfWeek.Sunday] = 0;
                        increment++;
                    }

                    break;
                case 1:
                    if (weekDays[DayOfWeek.Monday])
                    {
                        hash[DayOfWeek.Monday] = increment;
                        increment = 1;
                    }
                    else
                    {
                        hash[DayOfWeek.Monday] = 0;
                        increment++;
                    }

                    break;
                case 2:
                    if (weekDays[DayOfWeek.Tuesday])
                    {
                        hash[DayOfWeek.Tuesday] = increment;
                        increment = 1;
                    }
                    else
                    {
                        hash[DayOfWeek.Tuesday] = 0;
                        increment++;
                    }

                    break;
                case 3:
                    if (weekDays[DayOfWeek.Wednesday])
                    {
                        hash[DayOfWeek.Wednesday] = increment;
                        increment = 1;
                    }
                    else
                    {
                        hash[DayOfWeek.Wednesday] = 0;
                        increment++;
                    }

                    break;
                case 4:
                    if (weekDays[DayOfWeek.Thursday])
                    {
                        hash[DayOfWeek.Thursday] = increment;
                        increment = 1;
                    }
                    else
                    {
                        hash[DayOfWeek.Thursday] = 0;
                        increment++;
                    }

                    break;
                case 5:
                    if (weekDays[DayOfWeek.Friday])
                    {
                        hash[DayOfWeek.Friday] = increment;
                        increment = 1;
                    }
                    else
                    {
                        hash[DayOfWeek.Friday] = 0;
                        increment++;
                    }

                    break;
                case 6:
                    if (weekDays[DayOfWeek.Saturday])
                    {
                        hash[DayOfWeek.Saturday] = increment;
                        increment = 1;
                    }
                    else
                    {
                        hash[DayOfWeek.Saturday] = 0;
                        increment++;
                    }

                    break;
            }

            weekDayIndex = (DaysInWeek + weekDayIndex - 1) % DaysInWeek;
        }

        return hash;
    }

    public static bool TryGetStartDate(DateOnly beginDate,
        DateOnly fromDate,
        WeeklyHash weeklyHash,
        WeekDays weekDays,
        DayOfWeek firstDayOfWeek,
        int interval,
        out DateOnly startDate)
    {
        if (!DateHelper.TryGetDateOfDayOfWeek(beginDate, weekDays.MinDay, firstDayOfWeek, out startDate))
        {
            startDate = default;
            return false;
        }

        if (startDate >= fromDate) return true;

        var difDays =
            DateHelper.CalculateDaysToNextInterval(startDate.DayNumber, fromDate.DayNumber, interval, weeklyHash);
        var startDay = startDate.DayNumber + difDays;

        if (startDay > DateOnly.MaxValue.DayNumber)
        {
            startDate = default;
            return false;
        }

        startDate = DateOnly.FromDayNumber(startDay);
        return true;
    }
}