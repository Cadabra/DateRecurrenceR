using System.Collections;

namespace DateRecurrenceR.Collections;

internal sealed class UnionEnumerator : IEnumerator<DateOnly>
{
    private readonly EWrapper[] _enumerators;

    private bool _isInit;

    public UnionEnumerator(IReadOnlyList<IEnumerator<DateOnly>> enumerators)
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

        var tempArray = new EWrapper[enumeratorsCount];
        var index = 0;
        for (var i = 0; i < enumerators.Count; i++)
        {
            if (enumerators[i] is UnionEnumerator ue)
            {
                for (var j = 0; j < ue._enumerators.Length; j++)
                {
                    // ref var e2 = ref ue._enumerators[j];
                    tempArray[index] = ue._enumerators[j];
                    index += 1;
                }

                // Array.Copy(ue._enumerators, 0, _enumerators, index, ue._enumerators.Length);
                // index += ue._enumerators.Length;
            }
            else
            {
                tempArray[index] = new EWrapper(enumerators[i]);
                index += 1;
            }
        }

        _enumerators = new EWrapper[enumeratorsCount];
        var index2 = 0;
        for (var i = 0; i < tempArray.Length; i++)
        {
            var isUniq = true;
            for (var j = i + 1; j < tempArray.Length; j++)
            {
                var e1 = tempArray[i].Enum;
                var e2 = tempArray[j].Enum;

                if (object.ReferenceEquals(e1, e2))
                {
                    isUniq = false;
                }
                // unsafe
                // {
                //     fixed (IEnumerator<DateOnly>* e1 = &tempArray[i].Enum, e2 = &tempArray[j].Enum)
                //     {
                //         if (e1 == e2)
                //         {
                //             isUniq = false;
                //         }
                //     }
                // }
            }

            if (isUniq)
            {
                _enumerators[index2] = tempArray[i];
                index2++;
            }
        }

        var copyArr = new EWrapper[index2];
        Array.Copy(_enumerators, 0, copyArr, 0, index2);

        _enumerators = copyArr;
        Current = default;
    }

    public bool MoveNext()
    {
        if (!_isInit)
        {
            unsafe
            {
                for (var i = 0; i < _enumerators.Length; i++)
                {
                    fixed (EWrapper* e = &_enumerators[i])
                    {
                        e->MoveNext();
                    }
                }
            }
            // for (var i = 0; i < _enumerators.Length; i++)
            // {
            //     ref var e = ref _enumerators[i];
            //     e.MoveNext();
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
    }
}