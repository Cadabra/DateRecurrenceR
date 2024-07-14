using System.Numerics;

namespace DateRecurrenceR.Core;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TSelf"></typeparam>
#if NET8_0
public interface IInt32Based<TSelf> : IEquatable<TSelf>, IEqualityOperators<TSelf, TSelf, bool>, IMinMaxValue<TSelf>
    where TSelf : IInt32Based<TSelf>;
#endif