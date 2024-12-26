using System.Collections;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Collections;

public struct MonthlyEnumerator : IEnumerator<DateOnly>
{
    private readonly DateOnly _start;
    private readonly GetNextMonthDateDelegate _getNextDate;
    private readonly int _takeCount;
    private readonly int _interval = 0;
    private int _count = 0;

    internal MonthlyEnumerator(DateOnly startDate, int takeCount, int interval, GetNextMonthDateDelegate getNextDate)
    {
        _start = startDate;
        _takeCount = takeCount;
        _interval = interval;
        _getNextDate = getNextDate;
    }

    public bool MoveNext()
    {
        if (_count >= _takeCount)
        {
            Current = default;
            return false;
        }

        if (_count == 0)
        {
            Current = _start;
        }
        else
        {
            Current.AddMonths(_interval);
            Current = _getNextDate(Current.Year, Current.Month);
        }

        _count++;

        return true;
    }

    public void Reset()
    {
        throw new NotSupportedException();
    }

    public DateOnly Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}