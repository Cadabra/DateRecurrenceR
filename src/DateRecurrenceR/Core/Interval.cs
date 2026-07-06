using System.Globalization;

namespace DateRecurrenceR.Core;

/// <summary>
///     Represents a number of the interval
/// </summary>
public readonly struct Interval : IInt32Based<Interval>
{
    private const int MinVal = 1;
    private const int MaxVal = int.MaxValue;

    // Stored biased by -1 so that default(Interval), which bypasses the constructor,
    // equals MinValue instead of holding the invalid value 0.
    private readonly int _value;

    /// <summary>
    ///     Create an instance of <see cref="Interval" /> with minimal value
    /// </summary>
    public Interval()
    {
        this = MinValue;
    }

    /// <summary>
    ///     Create an instance of <see cref="Interval" /> with specified value
    /// </summary>
    public Interval(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, MinVal);

        _value = value - 1;
    }

    /// <summary>Represents the smallest possible value of <see cref="Interval" />.</summary>
    public static Interval MinValue { get; } = new(MinVal);

    /// <summary>Represents the largest possible value of <see cref="Interval" />.</summary>
    public static Interval MaxValue { get; } = new(MaxVal);

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Interval other && Equals(other);
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

    /// <summary>
    /// Converts the numeric value of this instance to its equivalent string representation.
    /// </summary>
    /// <returns>The string representation of the value of this instance, consisting of a sequence of digits ranging from 0 to 9 with no leading zeroes.</returns>
    public override string ToString()
    {
        return (_value + 1).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Convert <see cref="Interval" /> to <see cref="int" />
    /// </summary>
    /// <param name="interval"><see cref="int" /> representation of the value</param>
    /// <returns><see cref="Interval" /> representation of the value</returns>
    public static implicit operator int(Interval interval)
    {
        return interval._value + 1;
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
