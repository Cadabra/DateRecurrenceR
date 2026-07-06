using System.Numerics;

namespace DateRecurrenceR.Core;

#if NET8_0_OR_GREATER
/// <summary>
/// Defines the common contract for the library's <see cref="int" />-based value types,
/// such as <see cref="Interval" /> and <see cref="DayOfMonth" />: value equality,
/// equality operators, and the <c>MinValue</c>/<c>MaxValue</c> bounds of the valid range.
/// </summary>
/// <typeparam name="TSelf">The implementing value type itself.</typeparam>
public interface IInt32Based<TSelf> : IEquatable<TSelf>, IEqualityOperators<TSelf, TSelf, bool>, IMinMaxValue<TSelf>
    where TSelf : IInt32Based<TSelf>;
#endif