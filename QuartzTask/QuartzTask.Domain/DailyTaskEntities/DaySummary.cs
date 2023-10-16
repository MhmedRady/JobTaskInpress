namespace QuartzTask.Domain.Entities;

public class DaySummary: BaseEntity<int>
{
    public DateTime? DateSummary { get; set; }
    public int TotalRounds { get; set; }
    public int TotalTasks { get; set; }
}