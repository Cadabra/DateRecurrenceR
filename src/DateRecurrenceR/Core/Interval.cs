namespace DateRecurrenceR.Core;

/// <summary>
///     Represents a number of the interval
/// </summary>
#if NET6_0
public readonly struct Interval
#else
public readonly struct Interval : IInt32Based<Interval>
#endif
{
    private const int MinVal = 1;
    private const int MaxVal = int.MaxValue;

    private readonly int _value;

    /// <summary>
    ///     Create an instance of <see cref="T:DateRecurrenceR.Core.Interval" /> with minimal value
    /// </summary>
    public Interval()
    {
        this = MinValue;
    }

    /// <summary>
    ///     Create an instance of <see cref="T:DateRecurrenceR.Core.Interval" /> with specified value
    /// </summary>
    public Interval(int value)
    {
        if (value is < MinVal or > MaxVal) throw new ArgumentOutOfRangeException(nameof(value));

        _value = value;
    }

    /// <summary>Represents the smallest possible value of <see cref="T:DateRecurrenceR.Core.Interval" />.</summary>
    public static Interval MinValue { get; } = new(MinVal);

    /// <summary>Represents the largest possible value of an <see cref="T:DateRecurrenceR.Core.Interval" />.</summary>
    public static Interval MaxValue { get; } = new(MaxVal);

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
    public bool Equals(Interval other)
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
    ///     Convert <see cref="T:DateRecurrenceR.Core.Interval" /> to <see cref="T:System.Int32" />
    /// </summary>
    /// <param name="interval"><see cref="T:System.Int32" /> representation of the value</param>
    /// <returns><see cref="T:DateRecurrenceR.Core.Interval" /> representation of the value</returns>
    public static implicit operator int(Interval interval)
    {
        return interval._value;
    }

    /// <inheritdoc />
    public static bool operator ==(Interval left, Interval right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc />
    public static bool operator !=(Interval left, Interval right)
    {
        return !(left == right);
    }
}