using DateRecurrenceR.Collections;

namespace DateRecurrenceR;

public partial struct Recurrence
{
    /// <summary>
    ///     Produces the union enumerator of two enumerators.
    /// </summary>
    /// <param name="e1"><see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />. Expects that parameter will provide values in ascending order</param>
    /// <param name="e2"><see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />. Expects that parameter will provide values in ascending order</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Union(IEnumerator<DateOnly> e1, IEnumerator<DateOnly> e2)
    {
        return new UnionEnumerator(e1, e2);
    }

    /// <summary>
    ///     Produces the union enumerator of three enumerators.
    /// </summary>
    /// <param name="e1"><see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />. Expects that parameter will provide values in ascending order</param>
    /// <param name="e2"><see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />. Expects that parameter will provide values in ascending order</param>
    /// <param name="e3"><see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />. Expects that parameter will provide values in ascending order</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Union(IEnumerator<DateOnly> e1, IEnumerator<DateOnly> e2,
        IEnumerator<DateOnly> e3)
    {
        return new UnionEnumerator(e1, e2, e3);
    }

    /// <summary>
    ///     Produces the union enumerator of enumerators.
    /// </summary>
    /// <param name="enumerators">Array of <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />. Expects that parameters will provide values in ascending order</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
    public static IEnumerator<DateOnly> Union(params IEnumerator<DateOnly>[] enumerators)
    {
        return new UnionEnumerator(enumerators);
    }

    /// <summary>
    ///     Produces the union enumerator of enumerators.
    /// </summary>
    /// <param name="enumerators">Enumerable of <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />. Expects that parameters will provide values in ascending order</param>
    /// <returns>
    ///     <see cref="IEnumerator{T}" /> type of <see cref="DateOnly" />
    /// </returns>
#if NET9_0_OR_GREATER
    public static IEnumerator<DateOnly> Union(params IEnumerable<IEnumerator<DateOnly>> enumerators)
#else
    public static IEnumerator<DateOnly> Union(IEnumerable<IEnumerator<DateOnly>> enumerators)
#endif
    {
        return new UnionEnumerator(enumerators);
    }
}