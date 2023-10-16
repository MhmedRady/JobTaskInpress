using QuartzTask.Api;
using QuartzTask.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace QuartzTask.db;

public class DataInitialize
{

    public static async Task Initialize(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            await DataMigration(applicationBuilder);
            var context = serviceScope.ServiceProvider.GetService<MainDbContext>();
            var _dailyTaskContext = serviceScope.ServiceProvider.GetService<DailyTaskContext>();
            if (context.Rounds.Count() < 50)
            {
                var rCount = context.Rounds.Count();
                var round = new Round()
                {
                    Name = $"Round_{rCount+1}",
                    Start = DateTime.Now.AddDays(rCount - 1).Date.ToString("yyyy-MM-dd"),
                    End = DateTime.Now.AddDays(rCount + 1).Date.ToString("yyyy-MM-dd"),
                    CreatedAt = DateTime.Now.AddDays(-15),
                    LastModified = DateTime.Now.AddDays(rCount).Date.ToString("yyyy-MM-dd"),
                };
        
                for (int i = 1; i <= 10; i++)
                {
                    round.Tasks.Add(new Domain.Entities.Task()
                    {
                        Name = $"Task_Round_{rCount+i}",
                        Result = DependencyInjection.RandomNum(),
                        LastModified = DateTime.Now.AddDays(rCount).Date.ToString("yyyy-MM-dd"),
                        CreatedAt = DateTime.Now
                    });
                }

                await context.Rounds.AddAsync(round);
                var roundSave = await context.SaveChangesAsync() == 1;

                if (!roundSave)
                {
                    var daySummary = new DaySummary()
                    {
                        DateSummary = round.CreatedAt.Value,
                        TotalRounds = 1,
                        TotalTasks = round.Tasks.Count() * 10,
                    };
                    if (_dailyTaskContext != null)
                    {
                        await _dailyTaskContext.AddAsync(daySummary);
                        await _dailyTaskContext.SaveChangesAsync();
                    }
                }
                
            }
        }
    }
    public static async Task<bool> DataMigration(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var mainDbContext = serviceScope.ServiceProvider.GetService<MainDbContext>();
            var dailyTaskContext = serviceScope.ServiceProvider.GetService<DailyTaskContext>();
            try
            {
                await mainDbContext.Database.MigrateAsync();
                await dailyTaskContext.Database.MigrateAsync();
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