using System.Runtime.InteropServices.JavaScript;
using JobTaskInpress.db;
using JobTaskInpress.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NewProject.Repositories;
using Quartz;
using Task = System.Threading.Tasks.Task;

namespace JobTaskInpress.Api.QuartzJobs;

public class MargeJob: IJob
{
    private readonly IGeneralRepository<Round, Guid> _roundRepository;
    private readonly IGeneralRepository<Domain.Entities.Task, Guid> _taskRepository;
    private readonly DailyTaskContext _taskContext;
    private readonly ILogger<CreateJob> _logger;
    public MargeJob(ILogger<CreateJob> logger, IGeneralRepository<Round, Guid> roundRepository, IGeneralRepository<DayDetails, Guid> dayDetailsRepository, DailyTaskContext taskContext, IGeneralRepository<Domain.Entities.Task, Guid> taskRepository)
    {
        _logger = logger;
        _roundRepository = roundRepository;
        _taskContext = taskContext;
        _taskRepository = taskRepository;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("{UtcNow}", SaveRoundList());
        return Task.CompletedTask;
    }

    private async Task<bool> SaveRoundList()
    {
        var lastDateSummery = await _taskContext.DaySummary.LastOrDefaultAsync();
        var rCount = _roundRepository.Where(expression:task=> task.LastModified != lastDateSummery.DateSummary.Value.Date.ToString("yyyy-MM-dd")).ToList();
        
        var roundsList = rCount.Select(round => new DaySummary()
        {
            TotalTasks = DependencyInjection.RandomNum(),
            TotalRounds = DependencyInjection.RandomNum(),
            DateSummary = DateTime.Now,
            DayDetails = rCount.FirstOrDefault(round1 => round1.Id== round.Id).Tasks.Select(task => new DayDetails()
            {
                TaskId = task.Id.ToString(),
                RoundId = round.Id.ToString(),
                LastModified = task.LastModified
            }).ToList()
        });
        
        await _taskContext.DaySummary.AddRangeAsync(roundsList);
        return await _taskContext.SaveChangesAsync() == 1;
    }
}