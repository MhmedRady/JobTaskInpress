using JobTaskInpress.Api;
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
            var _dailyTaskContext = serviceScope.ServiceProvider.GetService<DailyTaskContext>();
            if (context.Rounds.Count() < 50)
            {
                var rCount = context.Rounds.Count();
                var round = new Round()
                {
                    Name = $"Round_{rCount+1}",
                    Start = DateTime.Now.AddDays(rCount - 1).Date.ToString("yyyy-MM-dd"),
                    End = DateTime.Now.AddDays(rCount + 1).Date.ToString("yyyy-MM-dd"),
                    CreatedAt = DateTime.Now.AddDays(DependencyInjection.RandomNum(1,50)),
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
                await context.SaveChangesAsync() ;
            }

            if (_dailyTaskContext.DaySummary.Any())
            {
                List<DaySummary> daySummaries = new List<DaySummary>();

                for (int i = 0; i < 10; i++)
                {
                    daySummaries.Add(new DaySummary()
                    {
                        DateSummary = DateTime.Today.AddDays(i),
                        TotalRounds =  DependencyInjection.RandomNum(100, 199),
                        TotalTasks =  DependencyInjection.RandomNum() * 10,
                    });
                }

                await _dailyTaskContext.AddRangeAsync(daySummaries);
                await _dailyTaskContext.SaveChangesAsync();
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