using System.Globalization;
using DateRecurrenceR.Core;
using FluentAssertions;
using Xunit;

namespace DateRecurrenceR.Tests.Common;

/// <summary>
/// Contract tests for the int-based value types (<see cref="Interval"/>, <see cref="DayOfMonth"/>,
/// <see cref="DayOfYear"/>, <see cref="MonthOfYear"/>). Derive per type and supply the factory
/// and the allowed range; the facts below run for every derived class.
/// </summary>
/// <typeparam name="T">The value type under test.</typeparam>
public abstract class Int32BasedContractTests<T> where T : struct, IInt32Based<T>
{
    /// <summary>The smallest value the constructor accepts.</summary>
    protected abstract int MinAllowed { get; }

    /// <summary>The largest value the constructor accepts.</summary>
    protected abstract int MaxAllowed { get; }

    /// <summary>Creates an instance via the type's constructor.</summary>
    protected abstract T Create(int value);

    /// <summary>Converts an instance back to <see cref="int"/> via the type's implicit conversion.</summary>
    protected abstract int ToInt32(T value);

    [Fact]
    public void Constructor_accepts_boundary_values()
    {
        ToInt32(Create(MinAllowed)).Should().Be(MinAllowed);
        ToInt32(Create(MaxAllowed)).Should().Be(MaxAllowed);
    }

    [Fact]
    public void Constructor_throws_below_minimum()
    {
        var act = () => Create(MinAllowed - 1);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [SkippableFact]
    public void Constructor_throws_above_maximum()
    {
        // When the maximum is int.MaxValue a larger value is not representable, so this fact is
        // impossible; report it as an explicit skip instead of letting it pass silently
        // (unchecked MaxAllowed + 1 would wrap and test the wrong bound).
        Skip.If(MaxAllowed == int.MaxValue, "a value above int.MaxValue is not representable");

        var act = () => Create(MaxAllowed + 1);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MinValue_and_MaxValue_match_the_allowed_range()
    {
        ToInt32(T.MinValue).Should().Be(MinAllowed);
        ToInt32(T.MaxValue).Should().Be(MaxAllowed);
    }

    [Fact]
    public void Default_instance_converts_without_throwing()
    {
        // default(T) bypasses the constructor; conversion must still work
        var act = () => ToInt32(default);

        act.Should().NotThrow();
    }

    [Fact]
    public void Equality_members_agree()
    {
        var a = Create(MinAllowed);
        var b = Create(MinAllowed);
        var c = Create(MaxAllowed);

        (a == b).Should().BeTrue();
        (a != b).Should().BeFalse();
        (a == c).Should().BeFalse();
        (a != c).Should().BeTrue();

        a.Equals(b).Should().BeTrue();
        a.Equals(c).Should().BeFalse();

        a.Equals((object)b).Should().BeTrue();
        a.Equals((object)c).Should().BeFalse();
        a.Equals(null).Should().BeFalse();
        a.Equals("not the same type").Should().BeFalse();

        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void ToString_returns_the_invariant_number()
    {
        Create(MaxAllowed).ToString().Should().Be(MaxAllowed.ToString(CultureInfo.InvariantCulture));
    }
}
