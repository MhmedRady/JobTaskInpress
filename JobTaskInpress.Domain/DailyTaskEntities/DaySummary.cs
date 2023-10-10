namespace JobTaskInpress.Domain.Entities;

public class DaySummary: BaseEntity<int>
{
    public DaySummary()
    {
        DayDetails = new HashSet<DayDetails>();
    }

    public DateTime? DateSummary { get; set; }
    public int TotalRounds { get; set; }
    public int TotalTasks { get; set; }
    public ICollection<DayDetails> DayDetails { get; set; }
}