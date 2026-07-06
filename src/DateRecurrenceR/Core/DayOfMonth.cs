using System.Globalization;

namespace DateRecurrenceR.Core;

/// <summary>
///     Represents a number of the day of the month
/// </summary>
public readonly struct DayOfMonth : IInt32Based<DayOfMonth>
{
    private const int MinVal = 1;
    private const int MaxVal = 31;

    private readonly int _value;

    /// <summary>
    ///     Create an instance of <see cref="DayOfMonth" /> with minimal value
    /// </summary>
    public DayOfMonth()
    {
        this = MinValue;
    }

    /// <summary>
    ///     Create an instance of <see cref="DayOfMonth" /> with specified value
    /// </summary>
    public DayOfMonth(int value)
    {
        if (value is < MinVal or > MaxVal) throw new ArgumentOutOfRangeException(nameof(value));

        _value = value;
    }

    /// <summary>Represents the smallest possible value of <see cref="DayOfMonth" />.</summary>
    public static DayOfMonth MinValue { get; } = new(MinVal);

    /// <summary>Represents the largest possible value of <see cref="DayOfMonth" />.</summary>
    public static DayOfMonth MaxValue { get; } = new(MaxVal);

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is DayOfMonth other && Equals(other);
    }

    /// <summary>
    ///     Indicates whether this instance and a specified other instance are equal.
    /// </summary>
    /// <param name="other">The other instance to compare with the current instance.</param>
    /// <returns>true if obj and this instance are the same type and represent the same value; otherwise, false.</returns>
    public bool Equals(DayOfMonth other)
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
        return _value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Convert <see cref="DayOfMonth" /> to <see cref="int" />
    /// </summary>
    /// <param name="dayOfMonth"><see cref="int" /> representation of the value</param>
    /// <returns><see cref="DayOfMonth" /> representation of the value</returns>
    public static implicit operator int(DayOfMonth dayOfMonth)
    {
        return dayOfMonth._value;
    }

    /// <inheritdoc />
    public static bool operator ==(DayOfMonth left, DayOfMonth right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc />
    public static bool operator !=(DayOfMonth left, DayOfMonth right)
    {
        return !(left == right);
    }
}
