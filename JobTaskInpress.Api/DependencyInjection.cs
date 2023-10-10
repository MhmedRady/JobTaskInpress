using JobTaskInpress.Api.QuartzJobs;
using JobTaskInpress.db;
using Microsoft.EntityFrameworkCore;
using NewProject.Repositories;
using Quartz;

namespace JobTaskInpress.Api;

public static class DependencyInjection
{
    public static void AddInjection(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddQuartz(opt =>
        {
            // WithCronSchedule("*/1 * * * *")
            opt.UseMicrosoftDependencyInjectionJobFactory();
            var jobCreateKey = JobKey.Create(nameof(CreateJob));
            opt.AddJob<CreateJob>(jobCreateKey)
                .AddTrigger(trigger => trigger.ForJob(jobCreateKey).WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(1).RepeatForever()));
        });
        
        serviceCollection.AddQuartzHostedService();
        #region UnitOfWork
        serviceCollection.AddScoped(typeof(IGeneralRepository<,>), typeof(GeneralRepository<,>));
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        #endregion
    }
    
    public static int RandomNum(int min = 1000, int max = 9999)
    {
        var random = new Random();
        return random.Next(min, max);
    }
}