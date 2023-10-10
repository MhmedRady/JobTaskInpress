using JobTaskInpress.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace JobTaskInpress.db;

public class DataInitialize
{

    public static async Task Initialize(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            await DataMigration(applicationBuilder);
            var context = serviceScope.ServiceProvider.GetService<MainDbContext>();
            if (context.Rounds.Count() < 50)
            {
                int total = context.Rounds.Count() + 10;
                var roundsList = new List<Round>();
                for (int i = 1; i <= total; i++)
                {
                    roundsList.Add(new Round()
                    {
                        Name = $"Round_{i+1}",
                        Start = DateTime.Now.AddDays(total - i).Date.ToString("yyyy-MM-dd"),
                        End = DateTime.Now.AddDays(total + i).Date.ToString("yyyy-MM-dd"),
                        CreatedAt = DateTime.Now,
                        LastModified = DateTime.Now.AddDays(total).Date.ToString("yyyy-MM-dd"),
                        Tasks = new List<Domain.Entities.Task>()
                        {
                            new Domain.Entities.Task()
                            {
                                Name = $"Task_Round_{i+1}",
                                Result = total-(total-i) == 0? 1: total-(total-i),
                                LastModified = DateTime.Now.AddDays(total).Date.ToString("yyyy-MM-dd"),
                                CreatedAt = DateTime.Now
                            }
                        }
                    });
                }
            }
        }
    }
    public static async Task<bool> DataMigration(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<MainDbContext>();
            try
            {
                await context.Database.MigrateAsync();
                return true;
            }
            catch (Exception ex)
            {
                var logger = applicationBuilder.ApplicationServices.GetRequiredService<ILogger<DataInitialize>>();
                logger.LogError(ex, "An error occured During Migration");
                return false;
            }
        }
    }
}