namespace JobTaskInpress.Domain.Entities;

public class Round: BaseEntity<Guid>
{
    public string? Name { get; set; }
    public string? Start { get; set; }
    public string? End { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? LastModified { get; set; }
    public ICollection<Task> Tasks { get; set; }
    
    public Round()
    {
        this.Tasks = new HashSet<Task>();
    }
}