using System.Collections;
using DateRecurrenceR.Internals;

namespace DateRecurrenceR.Collections.Val;

/// <summary>
/// 
/// </summary>
public partial struct DateEnumerator : IEnumerator<DateOnly>
{
    // common
    private readonly int _interval = 0;
    private int _count = 0;
    private bool _canMoveNext = true;
    private DateOnly _iterator;
    private int _intIterator = 0;

    // by count | by end date
    private readonly int _takeCount = 0;
    private readonly DateOnly _stopDate;

    // specific
    private readonly WeeklyHash _hash = new();
    private readonly GetNextMonthDateDelegate _getNextMonthDate = null!;
    private readonly GetNextYearDateDelegate _getNextYearDate = null!;

    private readonly EType _eType;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public bool MoveNext()
    {
        if (!_canMoveNext)
        {
            return false;
        }

        return _eType switch
        {
            EType.DailyLimitByCount => MoveNextDailyLimitByCount(),
            EType.DailyLimitByDate => MoveNextDailyLimitByDate(),
            EType.MonthlyLimitByCount => MoveNextMonthlyLimitByCount(),
            EType.MonthlyLimitByDate => MoveNextMonthlyLimitByDate(),
            EType.WeeklyLimitByCount => MoveNextWeeklyLimitByCount(),
            EType.WeeklyLimitByDate => MoveNextWeeklyLimitByDate(),
            EType.YearlyLimitByCount => MoveNextYearlyLimitByCount(),
            EType.YearlyLimitByDate => MoveNextYearlyLimitByDate(),
            EType.Empty => MoveNextEmpty(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="NotSupportedException"></exception>
    public void Reset()
    {
        throw new NotSupportedException();
    }

    public DateOnly Current { get; private set; }

    object IEnumerator.Current => Current;

    /// <inheritdoc />
    public void Dispose()
    {
    }

    private enum EType
    {
        Empty,
        DailyLimitByCount,
        DailyLimitByDate,
        MonthlyLimitByCount,
        MonthlyLimitByDate,
        WeeklyLimitByCount,
        WeeklyLimitByDate,
        YearlyLimitByCount,
        YearlyLimitByDate
    }
}