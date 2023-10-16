
using QuartzTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Task = QuartzTask.Domain.Entities.Task;

namespace QuartzTask.db;

public class MainDbContext: DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options): base(options) {}

    public DbSet<Round> Rounds { get; set; }
    public DbSet<Task> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Round>();
        modelBuilder.Entity<Task>();
        base.OnModelCreating(modelBuilder);
    }
}