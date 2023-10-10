using System.ComponentModel.DataAnnotations.Schema;

namespace JobTaskInpress.Domain.Entities;

public class DayDetails: BaseEntity<int>
{
    public string? RoundId { get; set; }
    public string? TaskId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? LastModified { get; set; }
    public int? DaySummaryId { get; set; }
    [ForeignKey("DaySummaryId")]
    public DaySummary DaySummary {get; set; }
}