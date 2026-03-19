using DateRecurrenceR.Core;

namespace DateRecurrenceR.Docs.Models;

public enum PatternType { Daily, Weekly, Monthly, Yearly }
public enum MonthlySubMode { ByDayOfMonth, ByDayOfWeek }

public class UnionPatternSlot
{
    public PatternType PatternType { get; set; } = PatternType.Daily;

    // Common
    public DateOnly BeginDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddYears(1));
    public int Interval { get; set; } = 1;

    // Weekly
    public bool Mon { get; set; } = true;
    public bool Tue { get; set; }
    public bool Wed { get; set; }
    public bool Thu { get; set; }
    public bool Fri { get; set; }
    public bool Sat { get; set; }
    public bool Sun { get; set; }
    public DayOfWeek FirstDayOfWeek { get; set; } = DayOfWeek.Monday;

    // Monthly
    public MonthlySubMode MonthlySubMode { get; set; } = MonthlySubMode.ByDayOfMonth;
    public int DayOfMonth { get; set; } = 1;
    public DayOfWeek DayOfWeek { get; set; } = DayOfWeek.Monday;
    public IndexOfDay IndexOfDay { get; set; } = IndexOfDay.First;

    // Yearly
    public int MonthOfYear { get; set; } = 1;
}
