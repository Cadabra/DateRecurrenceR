namespace DateRecurrenceR.Collections.Val;

public partial struct DateEnumerator
{
    public DateEnumerator()
    {
        _eType = EType.Empty;
    }

    private bool MoveNextEmpty()
    {
        return false;
    }
}