namespace DateRecurrenceR.Docs.Models;

/// <summary>The two C# API styles shown in code examples.</summary>
public enum CodeStyle
{
    /// <summary>The static <c>Recurrence.*</c> methods returning an enumerator.</summary>
    Enumerator,

    /// <summary>The <c>XxxRecurrence.New(new DateRange(...), new XxxPattern(...))</c> object API.</summary>
    Pattern
}
