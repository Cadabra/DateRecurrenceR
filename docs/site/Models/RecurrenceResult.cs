namespace DateRecurrenceR.Docs.Models;

public class RecurrenceResult
{
    public List<DateOnly> Dates { get; set; } = new();
    public bool IsTruncated { get; set; }
    public int TotalCount { get; set; }
    public string CSharpSnippet { get; set; } = string.Empty;
    /// <summary>Union only: maps each date to the 0-based slot indices that produced it.</summary>
    public Dictionary<DateOnly, List<int>>? DateSources { get; set; }
}
