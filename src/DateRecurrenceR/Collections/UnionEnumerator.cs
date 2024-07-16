using System.Collections;

namespace DateRecurrenceR.Collections;

internal sealed class UnionEnumerator : IEnumerator<DateOnly>
{
    private readonly EWrapper[] _enumerators;

    private bool _isInit;

    public UnionEnumerator(IReadOnlyList<IEnumerator<DateOnly>> enumerators)
    {
        var hash = new HashSet<EWrapper>();

        for (var i = 0; i < enumerators.Count; i++)
        {
            if (enumerators[i] is UnionEnumerator ue)
            {
                for (var j = 0; j < ue._enumerators.Length; j++)
                {
                    hash.Add(ue._enumerators[j]);
                }
            }
            else
            {
                hash.Add(new EWrapper(enumerators[i]));
            }
        }

        _enumerators = hash.ToArray();

        Current = default;
    }

    public bool MoveNext()
    {
        var next = Current;

        var minByIndex = -1;
        for (var i = 0; i < _enumerators.Length; i++)
        {
            if (_enumerators[i].CanMoveNext)
            {
                minByIndex = i;
            }
        }

        if (minByIndex < 0)
        {
            return false;
        }

        for (var i = 0; i < _enumerators.Length; i++)
        {
            if (!_enumerators[i].CanMoveNext) continue;

            if (_enumerators[minByIndex].Enum.Current > _enumerators[i].Enum.Current)
            {
                minByIndex = i;
            }
        }

        Current = _enumerators[minByIndex].Enum.Current;
        _enumerators[minByIndex].MoveNext();

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

    private struct EWrapper
    {
        public EWrapper(IEnumerator<DateOnly> @enum)
        {
            Enum = @enum;
            CanMoveNext = false;
        }

        public IEnumerator<DateOnly> Enum { get; }
        public bool CanMoveNext { get; private set; }

        public bool MoveNext()
        {
            CanMoveNext = Enum.MoveNext();
            return CanMoveNext;
        }

        public override int GetHashCode()
        {
            return Enum.GetHashCode();
        }
    }
}