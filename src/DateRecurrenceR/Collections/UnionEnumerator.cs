using System.Collections;

namespace DateRecurrenceR.Collections;

internal struct UnionEnumerator : IEnumerator<DateOnly>
{
    private readonly EWrapper[] _enumerators;
    private DateOnly? _current = null;

    public UnionEnumerator(IEnumerator<DateOnly> e1, IEnumerator<DateOnly> e2)
    {
        var hash = new HashSet<EWrapper>();
        
        if (e1 is UnionEnumerator ue1)
        {
            for (var j = 0; j < ue1._enumerators.Length; j++)
            {
                hash.Add(ue1._enumerators[j]);
            }
        }
        else
        {
            hash.Add(new EWrapper(e1));
        }
        
        if (e2 is UnionEnumerator ue2)
        {
            for (var j = 0; j < ue2._enumerators.Length; j++)
            {
                hash.Add(ue2._enumerators[j]);
            }
        }
        else
        {
            hash.Add(new EWrapper(e2));
        }

        _enumerators = hash.ToArray();

        Current = default;
    }

    public UnionEnumerator(IEnumerator<DateOnly> e1, IEnumerator<DateOnly> e2, IEnumerator<DateOnly> e3)
    {
        var hash = new HashSet<EWrapper>();
        
        if (e1 is UnionEnumerator ue1)
        {
            for (var j = 0; j < ue1._enumerators.Length; j++)
            {
                hash.Add(ue1._enumerators[j]);
            }
        }
        else
        {
            hash.Add(new EWrapper(e1));
        }
        
        if (e2 is UnionEnumerator ue2)
        {
            for (var j = 0; j < ue2._enumerators.Length; j++)
            {
                hash.Add(ue2._enumerators[j]);
            }
        }
        else
        {
            hash.Add(new EWrapper(e2));
        }
        
        if (e3 is UnionEnumerator ue3)
        {
            for (var j = 0; j < ue3._enumerators.Length; j++)
            {
                hash.Add(ue3._enumerators[j]);
            }
        }
        else
        {
            hash.Add(new EWrapper(e3));
        }

        _enumerators = hash.ToArray();

        Current = default;
    }

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

    public UnionEnumerator(IEnumerable<IEnumerator<DateOnly>> enumerators)
    {
        var hash = new HashSet<EWrapper>();

        foreach (var enumerator in enumerators)
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

        _enumerators = hash.ToArray();

        Current = default;
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

    private struct EWrapper
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

        public override int GetHashCode()
        {
            return Enum.GetHashCode();
        }
    }
}