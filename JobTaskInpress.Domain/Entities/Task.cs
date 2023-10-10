using System.ComponentModel.DataAnnotations.Schema;

namespace JobTaskInpress.Domain.Entities;

public class Task: BaseEntity<Guid>
{
    public string? Name { get; set; }
    public int Result { get; set; }
    public Guid RoundId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? LastModified { get; set; }
    
    [ForeignKey("RoundId")]
    public Round? Round { get; set; }
}