using QuartzTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace QuartzTask.db;

public class DailyTaskContext: DbContext
{
    public DailyTaskContext(DbContextOptions<DailyTaskContext> options) : base(options)
    {
    }

    public DbSet<DayDetails> DayDetails { get; set; }
    public DbSet<DaySummary> DaySummary { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DayDetails>().ToTable("DayDetails");
        modelBuilder.Entity<DaySummary>();
        base.OnModelCreating(modelBuilder);
    }
}