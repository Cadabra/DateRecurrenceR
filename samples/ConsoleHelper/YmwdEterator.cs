namespace ConsoleHelper;

public sealed class YmwdEterator
{
    private readonly DateOnly _startDate;
    private readonly DateOnly _endDate;

    public YmwdEterator(DateOnly startDate, DateOnly endDate)
    {
        this._startDate = startDate;
        this._endDate = endDate;
    }
}