using QuartzTask.db;
using QuartzTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NewProject.Repositories;
using Quartz;
using Task = System.Threading.Tasks.Task;

namespace QuartzTask.Api.QuartzJobs;

public class CreateJob: IJob
{
    private readonly IGeneralRepository<Round, Guid> _roundRepository;
    private readonly ILogger<CreateJob> _logger;
    private readonly MainDbContext _mainDbContext;
    public CreateJob(ILogger<CreateJob> logger, IGeneralRepository<Round, Guid> roundRepository, MainDbContext mainDbContext)
    {
        _logger = logger;
        _roundRepository = roundRepository;
        _mainDbContext = mainDbContext;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("{UtcNow}", SaveRoundList());
        return Task.CompletedTask;
    }

    private async Task<bool> SaveRoundList()
    {
        var rCount = _roundRepository.Count();
        var lastRound = _roundRepository.Get(orderby: round1 => round1.CreatedAt.Value, orderbyDirection: "DESC").Select(x=>x.CreatedAt.Value).FirstOrDefault();

        var createDate = lastRound.AddDays(1);
        
        for (int r = rCount; r <= rCount+ DependencyInjection.RandomNum(5, 15); r++)
        {
            var round = new Round()
            {
                Name = $"Round_{rCount+1}",
                Start = DateTime.Now.AddDays(rCount - 1).Date.ToString("yyyy-MM-dd"),
                End = DateTime.Now.AddDays(rCount + 1).Date.ToString("yyyy-MM-dd"),
                CreatedAt = createDate,
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
            _roundRepository.Add(round);
        }
        return _roundRepository.SaveChanges() == 0;
    }
}