using System.Numerics;

namespace DateRecurrenceR.Core;

#if NET8_0_OR_GREATER
/// <summary>
/// 
/// </summary>
/// <typeparam name="TSelf"></typeparam>
public interface IInt32Based<TSelf> : IEquatable<TSelf>, IEqualityOperators<TSelf, TSelf, bool>, IMinMaxValue<TSelf>
    where TSelf : IInt32Based<TSelf>;
#endif