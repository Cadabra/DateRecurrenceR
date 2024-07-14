namespace DateRecurrenceR.Core;

/// <summary>
///     Represents a number of the day of the year
/// </summary>
#if NET6_0
public readonly struct DayOfYear
#else
public readonly struct DayOfYear : IInt32Based<DayOfYear>
#endif
{
    private const int MinVal = 1;
    private const int MaxVal = 366;

    private readonly int _value;

    /// <summary>
    ///     Create an instance of <see cref="T:DateRecurrenceR.Core.DayOfYear" /> with minimal value
    /// </summary>
    public DayOfYear()
    {
        this = MinValue;
    }

    /// <summary>
    ///     Create an instance of <see cref="T:DateRecurrenceR.Core.DayOfYear" /> with specified value
    /// </summary>
    public DayOfYear(int value)
    {
        if (value is < MinVal or > MaxVal) throw new ArgumentOutOfRangeException(nameof(value));

        _value = value;
    }

    /// <summary>Represents the smallest possible value of <see cref="T:DateRecurrenceR.Core.DayOfYear" />.</summary>
    public static DayOfYear MinValue { get; } = new(MinVal);

    /// <summary>Represents the largest possible value of an <see cref="T:DateRecurrenceR.Core.DayOfYear" />.</summary>
    public static DayOfYear MaxValue { get; } = new(MaxVal);

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    /// <summary>
    ///     Indicates whether this instance and a specified other instance are equal.
    /// </summary>
    /// <param name="other">The other instance to compare with the current instance.</param>
    /// <returns>true if obj and this instance are the same type and represent the same value; otherwise, false.</returns>
    public bool Equals(DayOfYear other)
    {
        return _value == other._value;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return _value;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return _value.ToString();
    }
    
    /// <summary>
    ///     Convert <see cref="T:DateRecurrenceR.Core.DayOfYear" /> to <see cref="T:System.Int32" />
    /// </summary>
    /// <param name="dayOfYear"><see cref="T:System.Int32" /> representation of the value</param>
    /// <returns><see cref="T:DateRecurrenceR.Core.DayOfYear" /> representation of the value</returns>
    public static implicit operator int(DayOfYear dayOfYear)
    {
        return dayOfYear._value;
    }

    /// <inheritdoc />
    public static bool operator ==(DayOfYear left, DayOfYear right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc />
    public static bool operator !=(DayOfYear left, DayOfYear right)
    {
        return !(left == right);
    }
}