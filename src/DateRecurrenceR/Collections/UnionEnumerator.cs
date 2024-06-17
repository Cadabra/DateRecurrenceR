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

    // todo: try to remove '_isInit' 
    public bool MoveNext()
    {
        if (!_isInit)
        {
            for (var i = 0; i < _enumerators.Length; i++)
            {
                _enumerators[i].MoveNext();
            }

            // unsafe
            // {
            //     fixed (EWrapper* e = _enumerators)
            //     {
            //         for (var i = 0; i < _enumerators.Length; i++)
            //         {
            //             e[i].MoveNext();
            //         }
            //     }
            // }

            _isInit = true;
        }

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


    private static unsafe int GetFlattenCount(IEnumerator<DateOnly>[] enumerators)
    {
        var enumeratorsCount = 0;
        fixed (IEnumerator<DateOnly>* e = enumerators)
        {
            for (var i = 0; i < enumerators.Length; i++)
            {
                if (e[i] is UnionEnumerator ue)
                {
                    enumeratorsCount += ue._enumerators.Length;
                }
                else
                {
                    enumeratorsCount += 1;
                }
            }
        }

        return enumeratorsCount;
    }

    private static unsafe int GetFlattenCount_old(IReadOnlyList<IEnumerator<DateOnly>> enumerators)
    {
        var enumeratorsCount = 0;
        for (var i = 0; i < enumerators.Count; i++)
        {
            if (enumerators[i] is UnionEnumerator ue)
            {
                enumeratorsCount += ue._enumerators.Length;
            }
            else
            {
                enumeratorsCount += 1;
            }
        }

        return enumeratorsCount;
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