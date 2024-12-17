namespace DateRecurrenceR.Core;

/// <summary>
///     Represents a number of the month of the year
/// </summary>
#if NET8_0_OR_GREATER
public readonly struct MonthOfYear : IInt32Based<MonthOfYear>
#else
public readonly struct MonthOfYear : IEquatable<MonthOfYear>
#endif
{
    private const int MinVal = 1;
    private const int MaxVal = MonthsInYear;

    private readonly int _value;

    /// <summary>
    ///     Create an instance of <see cref="T:DateRecurrenceR.Core.MonthOfYear" /> with minimal value
    /// </summary>
    public MonthOfYear()
    {
        this = MinValue;
    }

    /// <summary>
    ///     Create an instance of <see cref="T:DateRecurrenceR.Core.MonthOfYear" /> with specified value
    /// </summary>
    public MonthOfYear(int value)
    {
        if (value is < MinVal or > MaxVal) throw new ArgumentOutOfRangeException(nameof(value));

        _value = value;
    }

    /// <summary>Represents the smallest possible value of <see cref="T:DateRecurrenceR.Core.MonthOfYear" />.</summary>
    public static MonthOfYear MinValue { get; } = new(MinVal);

    /// <summary>Represents the largest possible value of an <see cref="T:DateRecurrenceR.Core.MonthOfYear" />.</summary>
    public static MonthOfYear MaxValue { get; } = new(MaxVal);

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
    public bool Equals(MonthOfYear other)
    {
        return _value == other._value;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return _value;
    }

    /// <summary>
    /// Converts the numeric value of this instance to its equivalent string representation.
    /// </summary>
    /// <returns>The string representation of the value of this instance, consisting of a sequence of digits ranging from 0 to 9 with no leading zeroes.</returns>
    public override string ToString()
    {
        return _value.ToString();
    }

    /// <summary>
    ///     Convert <see cref="T:DateRecurrenceR.Core.MonthOfYear" /> to <see cref="T:System.Int32" />
    /// </summary>
    /// <param name="monthOfYear"><see cref="T:System.Int32" /> representation of the value</param>
    /// <returns><see cref="T:DateRecurrenceR.Core.MonthOfYear" /> representation of the value</returns>
    public static implicit operator int(MonthOfYear monthOfYear)
    {
        return monthOfYear._value;
    }

    /// <inheritdoc />
    public static bool operator ==(MonthOfYear left, MonthOfYear right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc />
    public static bool operator !=(MonthOfYear left, MonthOfYear right)
    {
        return !(left == right);
    }
}