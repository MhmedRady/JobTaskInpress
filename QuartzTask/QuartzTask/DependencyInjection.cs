using QuartzTask.Api.QuartzJobs;
using NewProject.Repositories;
using Quartz;
using QuartzTask.Repositories;

namespace QuartzTask.Api;

public static class DependencyInjection
{
    public static void AddInjection(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddQuartz(opt =>
        {
            // WithCronSchedule("*/1 * * * *")
            opt.UseMicrosoftDependencyInjectionJobFactory();
            
            var jobCreateKey = JobKey.Create(nameof(CreateJob));
            //var jobMargeKey = JobKey.Create(nameof(MargeJob));
            
            opt.AddJob<CreateJob>(jobCreateKey)
                .AddTrigger(trigger => trigger.ForJob(jobCreateKey).WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(1).RepeatForever()));
            
            // opt.AddJob<MargeJob>(jobMargeKey)
            //     .AddTrigger(trigger => trigger.ForJob(jobMargeKey).WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(60).RepeatForever()));
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