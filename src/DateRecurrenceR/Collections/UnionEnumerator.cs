using System.Collections;

namespace DateRecurrenceR.Collections;

// A class rather than a struct: every public entry point hands it out as IEnumerator<DateOnly>,
// so it would be boxed anyway.
internal sealed class UnionEnumerator : IEnumerator<DateOnly>
{
    private readonly EWrapper[] _enumerators;
    private DateOnly? _current;

    public UnionEnumerator(IEnumerator<DateOnly> e1, IEnumerator<DateOnly> e2)
    {
        var hash = new HashSet<EWrapper>();

        Add(hash, e1);
        Add(hash, e2);

        _enumerators = hash.ToArray();

        Current = default;
    }

    public UnionEnumerator(IEnumerator<DateOnly> e1, IEnumerator<DateOnly> e2, IEnumerator<DateOnly> e3)
    {
        var hash = new HashSet<EWrapper>();

        Add(hash, e1);
        Add(hash, e2);
        Add(hash, e3);

        _enumerators = hash.ToArray();

        Current = default;
    }

    public UnionEnumerator(IReadOnlyList<IEnumerator<DateOnly>> enumerators)
    {
        var hash = new HashSet<EWrapper>();

        for (var i = 0; i < enumerators.Count; i++)
        {
            Add(hash, enumerators[i]);
        }

        _enumerators = hash.ToArray();

        Current = default;
    }

    public UnionEnumerator(IEnumerable<IEnumerator<DateOnly>> enumerators)
    {
        var hash = new HashSet<EWrapper>();

        foreach (var enumerator in enumerators)
        {
            Add(hash, enumerator);
        }

        _enumerators = hash.ToArray();

        Current = default;
    }

    private static void Add(HashSet<EWrapper> hash, IEnumerator<DateOnly> enumerator)
    {
        if (enumerator is UnionEnumerator ue)
        {
            for (var j = 0; j < ue._enumerators.Length; j++)
            {
                hash.Add(ue._enumerators[j]);
            }
        }
        else
        {
            hash.Add(new EWrapper(enumerator));
        }
    }

    public bool MoveNext()
    {
        var nextIndex = -1;

        for (var i = 0; i < _enumerators.Length; i++)
        {
            if (_current.HasValue)
            {
                while (_enumerators[i].CanMoveNext && _enumerators[i].Enum.Current <= _current.Value)
                {
                    _enumerators[i].MoveNext();
                }
            }
            else
            {
                _enumerators[i].MoveNext();
            }
        }

        for (var i = 0; i < _enumerators.Length; i++)
        {
            if (!_enumerators[i].CanMoveNext) continue;

            _current = _enumerators[i].Enum.Current;
            nextIndex = i;
            break;
        }

        for (var i = 0; i < _enumerators.Length; i++)
        {
            if (!_enumerators[i].CanMoveNext || !(_current > _enumerators[i].Enum.Current)) continue;

            _current = _enumerators[i].Enum.Current;
            nextIndex = i;
        }

        if (nextIndex < 0)
        {
            Current = default;
            return false;
        }

        Current = _enumerators[nextIndex].Enum.Current;

        return true;
    }

    public void Reset()
    {
        throw new NotSupportedException();
    }

    public DateOnly Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }

    private struct EWrapper : IEquatable<EWrapper>
    {
        public EWrapper(IEnumerator<DateOnly> @enum)
        {
            Enum = @enum;
            CanMoveNext = true;
        }

        public IEnumerator<DateOnly> Enum { get; }
        public bool CanMoveNext { get; private set; }

        public bool MoveNext()
        {
            CanMoveNext = Enum.MoveNext();
            return CanMoveNext;
        }

        public override readonly int GetHashCode()
        {
            return Enum.GetHashCode();
        }

        public readonly bool Equals(EWrapper other)
        {
            // Deduplication key: the underlying enumerator only. CanMoveNext is mutable
            // iteration state and must not affect identity (GetHashCode already ignores it).
            return Enum.Equals(other.Enum);
        }

        public override bool Equals(object? obj)
        {
            return obj is EWrapper other && Equals(other);
        }
    }
}