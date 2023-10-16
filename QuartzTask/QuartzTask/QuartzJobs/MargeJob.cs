using QuartzTask.Api;
using QuartzTask.Api.QuartzJobs;
using QuartzTask.db;
using QuartzTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NewProject.Repositories;
using Quartz;
using QuartzTask.Controllers;
using Task = System.Threading.Tasks.Task;

namespace QuartzTask.QuartzJobs;

public class MargeJob: IJob
{
    private readonly IGeneralRepository<Round, Guid> _roundRepository;
    private readonly IGeneralRepository<QuartzTask.Domain.Entities.Task, Guid> _taskRepository;
    private readonly DailyTaskContext _dailyTaskContext;
    private readonly ILogger<CreateJob> _logger;
    public MargeJob(ILogger<CreateJob> logger, IGeneralRepository<Round, Guid> roundRepository, IGeneralRepository<DayDetails, Guid> dayDetailsRepository, DailyTaskContext taskContext, IGeneralRepository<QuartzTask.Domain.Entities.Task, Guid> taskRepository)
    {
        _logger = logger;
        _roundRepository = roundRepository;
        _dailyTaskContext = taskContext;
        _taskRepository = taskRepository;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("{UtcNow}", SaveSummaryList());
        return Task.CompletedTask;
    }

    private async Task<bool> SaveSummaryList()
    {
        var lastDateSummery = await _dailyTaskContext.DaySummary.OrderByDescending(summary => summary.DateSummary).FirstOrDefaultAsync();
        var _rounds = await _roundRepository.Where(round =>round.CreatedAt.Value.Date > lastDateSummery.DateSummary, "Tasks")
            .GroupBy(x=>x.CreatedAt.Value.Date)
            .Select(y=>new RoundSummary
            {
                CreatedAt = y.Key,
                RoundCount = y.Count(),
                TotalTaskCount = y.Count()*10
            }).OrderBy(r=>r.CreatedAt).Select(round => new DaySummary()
            {
                TotalTasks = round.TotalTaskCount,
                TotalRounds = round.RoundCount,
                DateSummary = round.CreatedAt
            }).ToListAsync();

        if (_rounds.Any())
        {
            await _dailyTaskContext.DaySummary.AddRangeAsync(_rounds);
            await _dailyTaskContext.SaveChangesAsync();
        }

        return false;
    }
}